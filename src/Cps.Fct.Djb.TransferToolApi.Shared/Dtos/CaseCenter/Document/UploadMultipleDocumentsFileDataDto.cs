// <copyright file="UploadMultipleDocumentsFileDataDto.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Shared.Dtos.CaseCenter.Document;

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents the request payload needed to upload documents from a cms bundle to a case.
/// </summary>
public record UploadMultipleDocumentsFileDataDto
{
    /// <summary>
    /// Gets or sets the Cms document id of the file being uploaded.
    /// </summary>
    [Required]
    [JsonProperty("cmsDocumentId")]
    public int CmsDocumentId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the name of the file being uploaded.
    /// </summary>
    [Required]
    [JsonProperty("fileName")]
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the binary content of the file being uploaded.
    /// </summary>
    [Required]
    [JsonProperty("content")]
    public byte[] Content { get; set; } = Array.Empty<byte>();

    /// <summary>
    /// Gets or sets the base 64 encoded content of the file being uploaded.
    /// </summary>
    [Required]
    [JsonProperty("base64Encoded")]
    public string Base64EncodedContent { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the content type (MIME type) of the file being uploaded.
    /// </summary>
    [Required]
    [JsonProperty("contentType")]
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the checksum of the file being uploaded.
    /// </summary>
    [Required]
    [JsonProperty("checksum")]
    public string Checksum { get; set; } = string.Empty;
}
