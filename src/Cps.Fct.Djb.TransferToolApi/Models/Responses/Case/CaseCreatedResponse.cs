// <copyright file="CaseCreatedResponse.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Models.Responses.Case;

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

/// <summary>
/// Represents the response containing the newly created cases case center id.
/// </summary>
public record CaseCreatedResponse
{
    /// <summary>
    /// Gets CaseCenterCaseId.
    /// </summary>
    [Required]
    [JsonProperty("caseCenterCaseId")]
    public string CaseCenterCaseId { get; init; } = string.Empty;
}
