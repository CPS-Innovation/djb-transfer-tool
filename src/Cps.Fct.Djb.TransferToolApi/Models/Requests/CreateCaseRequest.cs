// <copyright file="CreateCaseRequest.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Models.Requests;

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

/// <summary>
/// Represents the request data for creating a case in case center.
/// </summary>
public record CreateCaseRequest
{
    /// <summary>
    /// Gets CmsCaseId.
    /// </summary>
    [Required]
    [JsonProperty("cmsCaseId")]
    public int CmsCaseId { get; init; } = default!;

    /// <summary>
    /// Gets CmsUsername.
    /// </summary>
    [Required]
    [JsonProperty("cmsUsername")]
    public string CmsUsername { get; init; } = string.Empty;
}
