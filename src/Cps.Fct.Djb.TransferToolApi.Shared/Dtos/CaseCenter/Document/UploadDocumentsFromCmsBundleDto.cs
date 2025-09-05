// <copyright file="AddDocumentToCaseDto.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Shared.Dtos.CaseCenter.Document;

using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Auth;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents the request payload needed to upload documents from a cms bundle to a case.
/// </summary>
public record UploadDocumentsFromCmsBundleDto : AuthenticatedUserDto
{
    /// <summary>
    /// Gets the unique identifier for the case in CMS.
    /// </summary>
    [Required]
    [JsonProperty("cmsCaseId")]
    [JsonIgnore]
    public int CmsCaseId { get; set; } = default!;

    /// <summary>
    /// Gets the unique identifier for the bundle in cms.
    /// </summary>
    [Required]
    [JsonProperty("cmsBundleId")]
    [JsonIgnore]
    public int CmsBundleId { get; set; } = default!;

    /// <summary>
    /// Gets the document uploader.
    /// </summary>
    [Required]
    [JsonProperty("documentUploader")]
    public string DocumentUploader { get; init; } = string.Empty;
}
