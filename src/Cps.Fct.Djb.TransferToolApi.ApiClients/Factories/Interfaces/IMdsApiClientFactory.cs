// <copyright file="IMdsApiClientFactory.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.Factories.Interfaces;

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
