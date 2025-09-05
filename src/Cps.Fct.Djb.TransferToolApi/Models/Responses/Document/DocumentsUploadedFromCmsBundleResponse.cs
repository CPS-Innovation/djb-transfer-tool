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
    /// Gets DocumentResponses.
    /// </summary>
    [Required]
    [JsonProperty("documentResponses")]
    public IEnumerable<DocumentUploadResponse> DocumentResponses { get; init; } = new List<DocumentUploadResponse>();
}
