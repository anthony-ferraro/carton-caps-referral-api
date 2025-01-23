namespace CartonCaps.Api.Models.Responses;

/// <summary>
/// Response for generating a referral link.
/// The link is generated by the third-party deep link vendor
/// and includes all necessary tracking parameters.
/// </summary>
public record GenerateReferralLinkResponse(
    string ReferralLink
); 