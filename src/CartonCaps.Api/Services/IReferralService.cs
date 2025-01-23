using CartonCaps.Api.Models;
using CartonCaps.Api.Models.Enums;
using CartonCaps.Api.Models.Responses;

namespace CartonCaps.Api.Services;

/// <summary>
/// Service for managing referral-related operations.
/// </summary>
public interface IReferralService
{
    /// <summary>
    /// Gets the referral code for a user.
    /// </summary>
    Task<ReferralCode> GetReferralCodeAsync(string userId);
    
    /// <summary>
    /// Generates a referral link for sharing.
    /// </summary>
    Task<GenerateReferralLinkResponse> GenerateReferralLinkAsync(string userId, ShareMethodType shareMethod);
    
    /// <summary>
    /// Gets the referral history for a user.
    /// </summary>
    Task<IEnumerable<ReferralHistory>> GetReferralHistoryAsync(string userId);
    
    /// <summary>
    /// Validates a referral code.
    /// </summary>
    Task<ReferralValidation> ValidateReferralCodeAsync(string code);
} 