using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace CartonCaps.Api.Models.Enums;

/// <summary>
/// Status of a referral shown in the UI.
/// Display text should be handled by the frontend for localization.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ReferralStatus
{
    [EnumMember(Value = "complete")]
    Complete
} 