// <copyright file="ICaseCenterApiClient.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.Clients.Interfaces;

using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;

/// <summary>
/// Contract for ICaseCenterApiClient.
/// </summary>
public interface ICaseCenterApiClient
{
    /// <summary>
    /// Gets an authentication token from the Case Center API using the provided user credentials.
    /// </summary>
    /// <returns>A HttpReturnResultDto with the authentication token as a payload.</returns>
    public Task<HttpReturnResultDto<string>> GetAuthTokenAsync();
}
