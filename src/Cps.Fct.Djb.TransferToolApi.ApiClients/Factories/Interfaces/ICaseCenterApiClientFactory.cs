// <copyright file="ICaseCenterApiClientFactory.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.Factories.Interfaces;

using Cps.Fct.Djb.TransferToolApi.ApiClients.Clients.Interfaces;

/// <summary>
/// Contract for ICaseCenterApiClientFactory.
/// </summary>
public interface ICaseCenterApiClientFactory
{
    /// <summary>
    /// Create HTTP client.
    /// </summary>
    /// <param name="cookieHeader">cookieHeader.</param>
    /// <returns>MDS Client.</returns>
    /// <exception cref="ArgumentNullException">ArgumentNullException.</exception>
    /// <exception cref="InvalidOperationException">InvalidOperationException.</exception>
    ICaseCenterApiClient Create(string cookieHeader);
}
