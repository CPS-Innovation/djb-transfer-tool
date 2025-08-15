// <copyright file="CreateCaseDto.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Case;

using System.ComponentModel.DataAnnotations;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Auth;
using Newtonsoft.Json;

/// <summary>
/// Represents the request payload needed to create a case.
/// </summary>
public class CreateCaseDto : AuthenticatedUserDto
{
    /// <summary>
    /// Gets or sets the CMS URN for the case.
    /// </summary>
    [Required]
    [JsonProperty("cmsUrn")]
    public string CmsUrn { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the creator of the case.
    /// </summary>
    [Required]
    [JsonProperty("caseCreator")]
    public string CaseCreator { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the organisation ID associated with the case.
    /// </summary>
    [Required]
    [JsonProperty("organisationId")]
    public string OrganisationId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the title of the case.
    /// </summary>
    [Required]
    [JsonProperty("casetitle")]
    public string CaseTitle { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the template ID for the case.
    /// </summary>
    [Required]
    [JsonProperty("templateId")]
    public string TemplateId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of the case.
    /// </summary>
    [Required]
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier for the case in the CMS system.
    /// </summary>
    [Required]
    [JsonProperty("cmsUniqueId")]
    public string CmsUniqueId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the department ID associated with the case.
    /// </summary>
    [JsonProperty("departmentId")]
    public string DepartmentId { get; set; } = string.Empty;
}
