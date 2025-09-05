// <copyright file="BundleSummaryDto.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.Models.Response.CaseCenter;

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents the payload for bundle section summary
/// </summary>
public class BundleSectionSummaryResponse
{
    [Required]
    [JsonProperty("sectionid")]
    public string SectionId { get; set; } = string.Empty;

    [Required]
    [JsonProperty("sectionName")]
    public string SectionName { get; set; } = string.Empty;
}
