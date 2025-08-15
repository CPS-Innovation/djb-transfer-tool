using System.Text.Json.Serialization;

namespace Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;

/// <summary>
/// Represents a single validation error, mapping a property to its error message.
/// </summary>
public record ValidationErrorDto
{
    [JsonPropertyName("propertyName")]
    public string PropertyName { get; init; } = string.Empty;

    [JsonPropertyName("errorMessage")]
    public string ErrorMessage { get; init; } = string.Empty;
}
