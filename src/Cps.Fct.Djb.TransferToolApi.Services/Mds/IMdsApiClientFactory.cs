// <copyright file="IMdsApiClientFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Cps.MasterDataService.Infrastructure.ApiClient;

/// <summary>
/// Contract for IMdsApiClientFactory.
/// </summary>
public interface IMdsApiClientFactory
{
    /// <summary>
    /// Create HTTP client.
    /// </summary>
    /// <param name="cookieHeader">cookieHeader.</param>
    /// <returns>MDS Client.</returns>
    /// <exception cref="ArgumentNullException">ArgumentNullException.</exception>
    /// <exception cref="InvalidOperationException">InvalidOperationException.</exception>
    IMdsApiClient Create(string cookieHeader);
}
