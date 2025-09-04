// <copyright file="DocumentsUploadedFromCmsBundleResponse.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Models.Responses.Document;

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

/// <summary>
/// Represents the response containing the newly uplaoded document ids to the case center case.
/// </summary>
public record DocumentsUploadedFromCmsBundleResponse
{
    /// <summary>
    /// Gets UploadedDocumentCaseCenterIds.
    /// </summary>
    [Required]
    [JsonProperty("uploadedDocumentCaseCenterIds")]
    public IEnumerable<string> UploadedDocumentCaseCenterIds { get; init; } = new List<string>();
}
