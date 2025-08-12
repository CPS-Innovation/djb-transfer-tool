// <copyright file="MdsCookie.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Services.Dto;

using System.Globalization;

/// <summary>
/// MDS Cookie.
/// </summary>
public class MdsCookie
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MdsCookie"/> class.
    /// </summary>
    /// <param name="cookies">Cookie.</param>
    /// <param name="token">token.</param>
    public MdsCookie(string cookies, string token)
    {
        this.Cookies = cookies;
        this.Token = token;
        this.ExpiryTime = DateTime.UtcNow.AddHours(1).ToString("o", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets or sets cookie.
    /// </summary>
    public string Cookies { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets token.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets expiry time.
    /// </summary>
    public string ExpiryTime { get; set; } = string.Empty;
}
