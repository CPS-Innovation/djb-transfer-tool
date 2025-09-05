// <copyright file="ErrorResponse.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.Models.Response.CaseCenter;

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents the ErrorResponse
/// </summary>
public class ErrorResponse
{
    [Required]
    [JsonProperty("errorCode")]
    public string ErrorCode { get; set; } = string.Empty;

    [Required]
    [JsonProperty("errorMessage")]
    public string ErrorMessage { get; set; } = string.Empty;
}
