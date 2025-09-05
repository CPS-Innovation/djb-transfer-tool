// <copyright file="MultipleDocumentsUploadedFileDataDto.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Shared.Dtos.CaseCenter.Document;


using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents MultipleDocumentsUploadedFileDataDto.
/// </summary>
public record MultipleDocumentsUploadedFileDataDto
{
    [Required]
    [JsonProperty("uploadStatus")]
    public bool UploadStatus { get; set; } = default!;

    [Required]
    [JsonProperty("errorCode")]
    public string ErrorCode { get; set; } = string.Empty;

    [Required]
    [JsonProperty("errorMessage")]
    public string ErrorMessage { get; set; } = string.Empty;

    [Required]
    [JsonProperty("filename")]
    public string Filename { get; set; } = string.Empty;

    [Required]
    [JsonProperty("caseCenterDocumentId")]
    public string CaseCenterDocumentId { get; set; } = string.Empty;

    [Required]
    [JsonProperty("cmsDocumentId")]
    public int CmsDocumentId { get; set; } = default!;
}
