namespace CartonCaps.Api.Models.Responses;

/// <summary>
/// Validation result for a referral code.
/// Used to customize the onboarding experience for referred users.
/// </summary>
public record ReferralValidation(
    bool IsValid,
    string? ReferrerSchool,  // The school the referrer is supporting
    string? OnboardingFlow   // The type of onboarding flow to show (e.g., "referred_user", "standard")
); 