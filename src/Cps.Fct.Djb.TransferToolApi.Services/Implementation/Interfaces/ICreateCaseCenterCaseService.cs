// <copyright file="ICreateCaseCenterCaseService.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Services.Implementation.Interfaces;

using System.Threading.Tasks;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.CaseCenter;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;

/// <summary>
/// Create case center case service interface.
/// </summary>
public interface ICreateCaseCenterCaseService
{
    /// <summary>
    /// Create a case in Case Center.
    /// </summary>
    /// <param name="inputCreateCaseDto">The payload for the case to be created.</param>
    /// <returns>Returns the case center case idfor the created case.</returns>
    public Task<HttpReturnResultDto<string>> CreateCaseAsync(CreateCaseDto inputCreateCaseDto);
}
