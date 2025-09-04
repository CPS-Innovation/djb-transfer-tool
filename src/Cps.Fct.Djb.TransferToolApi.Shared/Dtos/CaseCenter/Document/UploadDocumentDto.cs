// <copyright file="UploadDocumentDto.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Shared.Dtos.CaseCenter.Document;

using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Auth;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents the request payload needed to add a document to a case.
/// </summary>
public record UploadDocumentDto : AuthenticatedUserDto
{
    [Required]
    [JsonProperty("date")]
    public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow;

    [Required]
    [JsonProperty("filedata")]
    public string FileData { get; set; } = string.Empty;

    [Required]
    [JsonProperty("filesize")]
    public string FileSize { get; set; } = string.Empty;

    [Required]
    [JsonProperty("filename")]
    public string FileName { get; set; } = string.Empty;

    [Required]
    [JsonProperty("fileextension")]
    public string FileExtension { get; set; } = string.Empty;

    [Required]
    [JsonProperty("checkSum")]
    public string CheckSum { get; set; } = string.Empty;

    [Required]
    [JsonProperty("indexAutoIncremented")]
    public bool IndexAutoIncremented { get; set; } = true;
}
