// <copyright file="UploadDocumentsFromCmsBundleRequest.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Models.Requests.Document;

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

/// <summary>
/// Represents the request data for uploading documents from cms bundle to a case in case center.
/// </summary>
public record UploadDocumentsFromCmsBundleRequest
{
    /// <summary>
    /// Gets CmsCaseId.
    /// </summary>
    [Required]
    [JsonProperty("cmsCaseId")]
    public int CmsCaseId { get; init; } = default!;

    /// <summary>
    /// Gets CmsBundleId.
    /// </summary>
    [Required]
    [JsonProperty("cmsBundleId")]
    public int CmsBundleId { get; init; } = default!;

    /// <summary>
    /// Gets CmsUsername.
    /// </summary>
    [Required]
    [JsonProperty("cmsUsername")]
    public string CmsUsername { get; init; } = string.Empty;
}
