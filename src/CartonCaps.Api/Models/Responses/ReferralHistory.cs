using System.Text.Json.Serialization;
using CartonCaps.Api.Models.Enums;

namespace CartonCaps.Api.Models.Responses;

/// <summary>
/// A referral in the user's history.
/// Tracks both the referral details and metadata about how/when it was created.
/// </summary>
public record ReferralHistory(
    string ReferredUserName,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    ReferralStatus Status,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    ShareMethodType ShareMethod,
    DateTimeOffset CreatedAt,
    DateTimeOffset? CompletedAt,
    string? ReferrerSchool  // Track which school benefits from this referral
); 