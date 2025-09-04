// <copyright file="CreateCaseDto.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Shared.Dtos.CaseCenter.Case;

using System.ComponentModel.DataAnnotations;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Auth;
using Newtonsoft.Json;

/// <summary>
/// Represents the request data for creating a case in case center.
/// </summary>
public record CreateCaseDto : AuthenticatedUserDto
{
    /// <summary>
    /// Gets CmsCaseId.
    /// </summary>
    [Required]
    [JsonProperty("cmsCaseId")]
    public int CmsCaseId { get; init; } = default!;

    /// <summary>
    /// Gets CaseCreator.
    /// </summary>
    [Required]
    [JsonProperty("caseCreator")]
    public string CaseCreator { get; init; } = string.Empty;

    /// <summary>
    /// Gets caseUrn.
    /// </summary>
    [Required]
    [JsonProperty("caseUrn")]
    public string CaseUrn { get; init; } = string.Empty;

    /// <summary>
    /// Gets CaseTitle.
    /// </summary>
    [Required]
    [JsonProperty("caseTitle")]
    public string CaseTitle { get; init; } = string.Empty;

    /// <summary>
    /// Gets AreaCrownCourtCode.
    /// </summary>
    [Required]
    [JsonProperty("areaCrownCourtCode")]
    public string AreaCrownCourtCode { get; init; } = string.Empty;
}
