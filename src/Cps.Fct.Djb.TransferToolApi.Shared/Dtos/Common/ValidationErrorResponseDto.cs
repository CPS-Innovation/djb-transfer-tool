// <copyright file="ValidationErrorResponseDto.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;

using System.Text.Json.Serialization;

/// <summary>
/// Represents a structured response containing a list of validation errors.
/// </summary>
public record ValidationErrorResponseDto
{
    /// <summary>
    /// Gets the general message indicating that validation errors occurred.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; init; } = "Validation errors occurred.";

    /// <summary>
    /// Gets the list of validation errors.
    /// </summary>
    [JsonPropertyName("errors")]
    public IReadOnlyList<ValidationErrorDto> Errors { get; init; }
        = Array.Empty<ValidationErrorDto>();
}
