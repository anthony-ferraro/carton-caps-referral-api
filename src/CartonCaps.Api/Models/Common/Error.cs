namespace CartonCaps.Api.Models;

/// <summary>
/// Represents an error response from the API
/// </summary>
public record Error
{
    /// <summary>
    /// A machine-readable error code
    /// </summary>
    public required string Code { get; init; }
    
    /// <summary>
    /// A human-readable error message
    /// </summary>
    public required string Message { get; init; }
} 