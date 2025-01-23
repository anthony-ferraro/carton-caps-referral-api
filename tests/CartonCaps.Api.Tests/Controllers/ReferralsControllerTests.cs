using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CartonCaps.Api.Controllers;
using CartonCaps.Api.Models;
using CartonCaps.Api.Models.Enums;
using CartonCaps.Api.Models.Responses;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CartonCaps.Api.Tests.Controllers;

public class ReferralsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly JsonSerializerOptions _jsonOptions;

    public ReferralsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    [Fact]
    public async Task GetMyReferralCode_ReturnsSuccessAndReferralCode()
    {
        // Arrange
        var client = _factory.CreateClient();
        // Act
        var response = await client.GetAsync("/v1/referrals/my-code");
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ReferralCode>(content, _jsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.Code.Should().Be("XY7G4D");
    }

    [Theory]
    [InlineData("text")]
    [InlineData("email")]
    [InlineData("share")]
    public async Task GenerateReferralLink_WithValidShareMethod_ReturnsLink(string shareMethod)
    {
        // Arrange
        var client = _factory.CreateClient();
        // Act
        var response = await client.PostAsync($"/v1/referrals/generate-link/{shareMethod}", null);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<GenerateReferralLinkResponse>(content, _jsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Should().NotBeNull();
        result!.ReferralLink.Should().StartWith("https://cartoncaps.link/");
        result.ReferralLink.Should().Contain("referral_code=");
    }

    [Fact]
    public async Task GetReferralHistory_ReturnsHistory()
    {
        // Arrange
        var client = _factory.CreateClient();
        // Act
        var response = await client.GetAsync("/v1/referrals/history");
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<IEnumerable<ReferralHistory>>(content, _jsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result.Should().HaveCountGreaterThan(0);
        
        var firstReferral = result?.FirstOrDefault();
        firstReferral.Should().NotBeNull();
        firstReferral!.ReferredUserName.Should().NotBeNullOrEmpty();
        firstReferral.Status.Should().Be(ReferralStatus.Complete);
    }

    [Theory]
    [InlineData("XY7G4D", true)]  // Valid code
    [InlineData("INVALID", false)] // Invalid code
    public async Task ValidateReferralCode_ReturnsExpectedResult(string code, bool expectedValid)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/v1/referrals/validate/{code}");
        
        if (expectedValid)
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ReferralValidation>(content, _jsonOptions);
            
            result.Should().NotBeNull();
            result!.IsValid.Should().BeTrue();
            result.ReferrerSchool.Should().NotBeNullOrEmpty();
            result.OnboardingFlow.Should().Be("referred_user");
        }
        else
        {
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var content = await response.Content.ReadAsStringAsync();
            var error = JsonSerializer.Deserialize<Error>(content, _jsonOptions);
            
            error.Should().NotBeNull();
            error!.Code.Should().Be("invalid_code");
        }
    }

    [Theory]
    [InlineData("XY7G4D", true)]     // Valid format
    [InlineData("xy7g4d", false)]    // Lowercase not allowed
    [InlineData("XY7-G4D", false)]   // Special characters not allowed
    [InlineData("XY7 G4D", false)]   // Spaces not allowed
    [InlineData("SHORT", false)]     // Too short
    [InlineData("TOOLONGCODE", false)] // Too long
    public async Task ValidateReferralCode_WithVariousFormats(string code, bool shouldBeValid)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/v1/referrals/validate/{code}");
        
        // Assert
        if (shouldBeValid)
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ReferralValidation>(content, _jsonOptions);
            
            result.Should().NotBeNull();
            result!.IsValid.Should().BeTrue();
            result.ReferrerSchool.Should().NotBeNullOrEmpty();
            result.OnboardingFlow.Should().Be("referred_user");
        }
        else
        {
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var content = await response.Content.ReadAsStringAsync();
            var error = JsonSerializer.Deserialize<Error>(content, _jsonOptions);
            
            error.Should().NotBeNull();
            error!.Code.Should().Be("invalid_code");
            error.Message.Should().NotBeNullOrEmpty();
        }
    }

    [Theory]
    [InlineData("/v1/referrals/validate/INVALID", "invalid_code", HttpStatusCode.NotFound)]
    public async Task ErrorResponse_HasConsistentFormat(string endpoint, string expectedErrorCode, HttpStatusCode expectedStatus)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(endpoint);
        
        // Assert
        response.StatusCode.Should().Be(expectedStatus);
        
        var content = await response.Content.ReadAsStringAsync();
        var error = JsonSerializer.Deserialize<Error>(content, _jsonOptions);
        
        error.Should().NotBeNull();
        error!.Code.Should().Be(expectedErrorCode);
        error.Message.Should().NotBeNullOrEmpty();
        
        // Verify JSON format
        var rawJson = await response.Content.ReadAsStringAsync();
        rawJson.Should().Contain("\"code\":");
        rawJson.Should().Contain("\"message\":");
    }
} 