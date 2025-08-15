// <copyright file="ICaseCenterApiClient.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.Clients.Interfaces;

using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Case;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;

/// <summary>
/// Contract for ICaseCenterApiClient.
/// </summary>
public interface ICaseCenterApiClient
{
    /// <summary>
    /// Creates a Case Center case based on the payload data.
    /// Returns HttpReturnResultDto with a case center case id if successful.
    ///   - id is non-null if success
    ///   - id is null if failure (then statusCode is the server's code or 500 if exception)
    /// </summary>
    /// <param name="createCaseDto">The DTO containing the case payload data.</param>
    /// <returns>
    /// A HttpResponseMessage
    /// </returns>
    Task<HttpReturnResultDto<string>> CreateCaseAsync(CreateCaseDto createCaseDto);
}
