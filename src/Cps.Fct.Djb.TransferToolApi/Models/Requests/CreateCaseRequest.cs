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
    /// Gets CmsUrn.
    /// </summary>
    [Required]
    [JsonProperty("cmsUrn")]
    public string CmsUrn { get; init; } = string.Empty;

    /// <summary>
    /// Gets CaseCreator.
    /// </summary>
    [Required]
    [JsonProperty("caseCreator")]
    public string CaseCreator { get; init; } = string.Empty;

    /// <summary>
    /// Gets CaseTitle.
    /// </summary>
    [Required]
    [JsonProperty("casetitle")]
    public string CaseTitle { get; init; } = string.Empty;

    /// <summary>
    /// Gets CmsUniqueId.
    /// </summary>
    [Required]
    [JsonProperty("cmsUniqueId")]
    public string CmsUniqueId { get; init; } = string.Empty;

    /// <summary>
    /// Gets DepartmentId.
    /// </summary>
    [JsonProperty("departmentId")]
    public string DepartmentId { get; init; } = string.Empty;
}
