// <copyright file="BundleSummaryResponse.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.Models.Response.CaseCenter;

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents the payload for bundle summary
/// </summary>
public class BundleSummaryResponse
{
    [Required]
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;
}
