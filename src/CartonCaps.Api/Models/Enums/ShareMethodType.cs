using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace CartonCaps.Api.Models.Enums;

/// <summary>
/// Share method types supported by the API.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ShareMethodType
{
    [EnumMember(Value = "text")]
    TEXT,
    
    [EnumMember(Value = "email")]
    EMAIL,
    
    [EnumMember(Value = "share")]
    SHARE
} 