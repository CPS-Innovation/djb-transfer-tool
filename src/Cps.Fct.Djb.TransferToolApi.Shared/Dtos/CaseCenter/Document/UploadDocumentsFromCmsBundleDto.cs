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
    public string CmsCaseId { get; set; } = string.Empty;

    /// <summary>
    /// Gets the unique identifier for the case in Case Center.
    /// </summary>
    [Required]
    [JsonProperty("caseid")]
    public string CaseId { get; set; } = string.Empty;

    /// <summary>
    /// Gets the document uploader.
    /// </summary>
    [Required]
    [JsonProperty("documentUploader")]
    public string DocumentUploader { get; init; } = string.Empty;
}
