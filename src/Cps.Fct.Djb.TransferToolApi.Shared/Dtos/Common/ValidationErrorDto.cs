// <copyright file="ValidationErrorDto.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;

using System.Text.Json.Serialization;

/// <summary>
/// Represents a single validation error, mapping a property to its error message.
/// </summary>
public record ValidationErrorDto
{
    /// <summary>
    /// Gets the name of the property that has the validation error.
    /// </summary>
    [JsonPropertyName("propertyName")]
    public string PropertyName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the error message associated with the property.
    /// </summary>
    [JsonPropertyName("errorMessage")]
    public string ErrorMessage { get; init; } = string.Empty;
}
