using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace CartonCaps.Api.Models.Enums;

/// <summary>
/// Error codes for the referral system.
/// Used to provide specific error information for debugging and client handling.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ReferralErrorCode
{
    [EnumMember(Value = "invalid_code")]
    invalid_code,
    
    [EnumMember(Value = "invalid_share_method")]
    invalid_share_method,
    
    [EnumMember(Value = "rate_limit_exceeded")]
    rate_limit_exceeded,
    
    [EnumMember(Value = "link_generation_failed")]
    link_generation_failed
} 