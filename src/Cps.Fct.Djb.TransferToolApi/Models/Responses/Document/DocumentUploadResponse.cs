// <copyright file="DocumentUploadResponse.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Models.Responses.Document;

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

/// <summary>
/// Represents the response containing the newly uplaoded document ids to the case center case.
/// </summary>
public record DocumentUploadResponse
{
    /// <summary>
    /// Gets a value indicating whether IsUploaded.
    /// </summary>
    [Required]
    [JsonProperty("isUploaded")]
    public bool IsUploaded { get; init; } = default!;

    /// <summary>
    /// Gets ErrorMessage.
    /// </summary>
    [Required]
    [JsonProperty("errorMessage")]
    public string ErrorMessage { get; init; } = default!;

    /// <summary>
    /// Gets CmsDocumentId.
    /// </summary>
    [Required]
    [JsonProperty("cmsDocumentId")]
    public int CmsDocumentId { get; init; } = default!;

    /// <summary>
    /// Gets CaseCenterDocumentId.
    /// </summary>
    [Required]
    [JsonProperty("caseCenterDocumentId")]
    public string CaseCenterDocumentId { get; init; } = default!;
}
