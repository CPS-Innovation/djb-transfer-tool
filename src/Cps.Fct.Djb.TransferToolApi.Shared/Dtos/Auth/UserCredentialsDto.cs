// <copyright file="UserCredentialsDto.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Auth;

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

/// <summary>
/// Represents the user login credentials.
/// </summary>
public record UserCredentialsDto
{
    /// <summary>
    /// Gets the username for authentication.
    /// </summary>
    [Required]
    [JsonProperty("userName")]
    public string Username { get; init; } = string.Empty;

    /// <summary>
    /// Gets the password for authentication.
    /// </summary>
    [Required]
    [JsonProperty("password")]
    public string Password { get; init; } = string.Empty;
}
