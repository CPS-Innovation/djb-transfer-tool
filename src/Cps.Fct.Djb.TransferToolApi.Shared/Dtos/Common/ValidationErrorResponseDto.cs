using System.Text.Json.Serialization;

namespace Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;

/// <summary>
/// Represents a structured response containing a list of validation errors.
/// </summary>
public record ValidationErrorResponseDto
{
    [JsonPropertyName("message")]
    public string Message { get; init; } = "Validation errors occurred.";

    [JsonPropertyName("errors")]
    public IReadOnlyList<ValidationErrorDto> Errors { get; init; }
        = Array.Empty<ValidationErrorDto>();
}
