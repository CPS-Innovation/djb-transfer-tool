// <copyright file="AuthenticatedUserDto.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Auth;

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

/// <summary>
/// Represents the authenticated user token.
/// </summary>
public class AuthenticatedUserDto
{
    /// <summary>
    /// Gets or sets the authentication token  user.
    /// </summary>
    [Required]
    [JsonProperty("authToken")]
    [JsonIgnore]
    public string AuthToken { get; set; } = string.Empty;
}
