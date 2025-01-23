using CartonCaps.Api.Models;
using CartonCaps.Api.Models.Enums;
using CartonCaps.Api.Models.Responses;

namespace CartonCaps.Api.Services;

/// <summary>
/// Implementation of the referral service.
/// </summary>
public class ReferralService : IReferralService
{
    private readonly ILogger<ReferralService> _logger;

    public ReferralService(ILogger<ReferralService> logger)
    {
        _logger = logger;
    }

    public Task<ReferralCode> GetReferralCodeAsync(string userId)
    {
        _logger.LogInformation("Getting referral code for user {UserId}", userId);
        
        // In a real implementation, we would:
        // 1. Look up the user's referral code in the database
        // 2. Generate one if it doesn't exist
        // 3. Return the code
        var code = new ReferralCode("XY7G4D");
        
        return Task.FromResult(code);
    }

    public Task<GenerateReferralLinkResponse> GenerateReferralLinkAsync(string userId, ShareMethodType shareMethod)
    {
        _logger.LogInformation(
            "Generating referral link for user {UserId} with share method {ShareMethod}", 
            userId,
            shareMethod
        );
        
        // In a real implementation, we would:
        // 1. Call the deep link vendor's API
        // 2. Include necessary tracking parameters in their format
        // 3. Get back a fully formed deep link
        var response = new GenerateReferralLinkResponse(
            ReferralLink: "https://cartoncaps.link/abfilefa90p?referral_code=XY7G4D"
        );
        
        return Task.FromResult(response);
    }

    public Task<IEnumerable<ReferralHistory>> GetReferralHistoryAsync(string userId)
    {
        _logger.LogInformation("Getting referral history for user {UserId}", userId);
        
        // In a real implementation, we would:
        // 1. Query the database for all referrals made by this user
        // 2. Join with user table to get referred user names
        // 3. Join with school table to get school names
        var now = DateTimeOffset.UtcNow;
        var history = new List<ReferralHistory>
        {
            new(
                ReferredUserName: "Jenny S.",
                Status: ReferralStatus.Complete,
                ShareMethod: ShareMethodType.TEXT,
                CreatedAt: now.AddDays(-5),
                CompletedAt: now.AddDays(-4),
                ReferrerSchool: "Lincoln Elementary"
            ),
            new(
                ReferredUserName: "Archer K.",
                Status: ReferralStatus.Complete,
                ShareMethod: ShareMethodType.EMAIL,
                CreatedAt: now.AddDays(-3),
                CompletedAt: now.AddDays(-2),
                ReferrerSchool: "Washington Middle School"
            ),
            new(
                ReferredUserName: "Helen Y.",
                Status: ReferralStatus.Complete,
                ShareMethod: ShareMethodType.SHARE,
                CreatedAt: now.AddDays(-1),
                CompletedAt: now,
                ReferrerSchool: "Lincoln Elementary"
            )
        };
        
        return Task.FromResult(history.AsEnumerable());
    }

    public Task<ReferralValidation> ValidateReferralCodeAsync(string code)
    {
        _logger.LogInformation("Validating referral code {code}", code);

        // Mock validation - in real implementation would check against database
        if (code == "XY7G4D")
        {
            return Task.FromResult(new ReferralValidation(
                IsValid: true,
                ReferrerSchool: "Lincoln Elementary",
                OnboardingFlow: "referred_user"
            ));
        }

        throw new ReferralException(ReferralErrorCode.invalid_code, "The specified referral code was not found");
    }
} 