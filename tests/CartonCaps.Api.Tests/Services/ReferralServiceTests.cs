using Microsoft.Extensions.Logging;
using Moq;
using CartonCaps.Api.Models.Enums;
using CartonCaps.Api.Services;

namespace CartonCaps.Api.Tests.Services;

public class ReferralServiceTests
{
    private readonly Mock<ILogger<ReferralService>> _loggerMock;
    private readonly ReferralService _service;

    public ReferralServiceTests()
    {
        _loggerMock = new Mock<ILogger<ReferralService>>();
        _service = new ReferralService(_loggerMock.Object);
    }

    [Fact]
    public async Task GetReferralCode_ReturnsValidCode()
    {
        // Act
        var result = await _service.GetReferralCodeAsync("test-user");

        // Assert
        result.Should().NotBeNull();
        result.Code.Should().Be("XY7G4D");
    }

    [Theory]
    [InlineData(ShareMethodType.TEXT)]
    [InlineData(ShareMethodType.EMAIL)]
    [InlineData(ShareMethodType.SHARE)]
    public async Task GenerateReferralLink_WithValidShareMethod_ReturnsLink(ShareMethodType shareMethod)
    {
        // Act
        var result = await _service.GenerateReferralLinkAsync("test-user", shareMethod);

        // Assert
        result.Should().NotBeNull();
        result.ReferralLink.Should().StartWith("https://cartoncaps.link/");
        result.ReferralLink.Should().Contain("referral_code=");
    }

    [Fact]
    public async Task GetReferralHistory_ReturnsNonEmptyHistory()
    {
        // Act
        var result = await _service.GetReferralHistoryAsync("test-user");

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result.Should().AllSatisfy(r =>
        {
            r.Status.Should().Be(ReferralStatus.Complete);
            r.ReferredUserName.Should().NotBeNullOrEmpty();
            r.ReferrerSchool.Should().NotBeNullOrEmpty();
            r.CompletedAt.Should().NotBeNull();
            r.CreatedAt.Should().BeBefore(r.CompletedAt!.Value);
        });
    }

    [Fact]
    public async Task ValidateReferralCode_WithValidCode_ReturnsValidation()
    {
        // Act
        var result = await _service.ValidateReferralCodeAsync("XY7G4D");

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.ReferrerSchool.Should().Be("Lincoln Elementary");
        result.OnboardingFlow.Should().Be("referred_user");
    }

    [Fact]
    public async Task ValidateReferralCode_WithInvalidCode_ThrowsException()
    {
        // Act & Assert
        var act = () => _service.ValidateReferralCodeAsync("INVALID");
        
        await act.Should().ThrowAsync<ReferralException>()
            .Where(e => e.ErrorCode == ReferralErrorCode.invalid_code);
    }
} 