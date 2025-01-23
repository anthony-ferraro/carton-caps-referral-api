using CartonCaps.Api.Models.Enums;

namespace CartonCaps.Api.Services;

/// <summary>
/// Exception thrown when a referral operation fails.
/// </summary>
public class ReferralException : Exception
{
    /// <summary>
    /// The error code associated with this exception.
    /// </summary>
    public ReferralErrorCode ErrorCode { get; }

    public ReferralException(ReferralErrorCode errorCode, string message)
        : base(message)
    {
        ErrorCode = errorCode;
    }
} 