// <copyright file="UploadDocumentResponse.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.Models.Response.CaseCenter;

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents the UploadDocumentResponse
/// </summary>
public class UploadDocumentResponse
{
    [Required]
    [JsonProperty("uploadStatus")]
    public bool UploadStatus { get; set; } = default!;

    [Required]
    [JsonProperty("uploadError")]
    public ErrorResponse UploadError { get; set; } = default!;

    [Required]
    [JsonProperty("filename")]
    public string Filename { get; set; } = string.Empty;

    [Required]
    [JsonProperty("documentId")]
    public string DocumentId { get; set; } = string.Empty;
}
