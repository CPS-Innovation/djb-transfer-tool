// <copyright file="AuthenticatedUserDto.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Auth;

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

/// <summary>
/// Represents the authenticated user token.
/// </summary>
public record AuthenticatedUserDto
{
    /// <summary>
    /// Gets or sets the authentication token for case center user.
    /// </summary>
    [Required]
    [JsonProperty("caseCenterAuthToken")]
    [JsonIgnore]
    public string CaseCenterAuthToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Cms classic auth cookies.
    /// </summary>
    [Required]
    [JsonProperty("cmsClassicAuthCookies")]
    [JsonIgnore]
    public string CmsClassicAuthCookies { get; set; } = string.Empty!;

    /// <summary>
    /// Gets or sets the Cms modern auth token.
    /// </summary>
    [Required]
    [JsonProperty("cmsModernAuthToken")]
    [JsonIgnore]
    public string CmsModernAuthToken { get; set; } = string.Empty;
}
