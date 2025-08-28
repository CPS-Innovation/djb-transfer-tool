// <copyright file="ICreateCaseCenterCaseService.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Services.Interfaces;

using System.Threading.Tasks;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;

/// <summary>
/// Create case center case service interface.
/// </summary>
public interface ICreateCaseCenterCaseService
{
    /// <summary>
    /// Create a case in Case Center.
    /// </summary>
    /// <param name="cmsCaseId">The cms unique case id.</param>
    /// <param name="cmsUsername">The username of the person creating the case.</param>
    /// <returns>Returns the case center case idfor the created case.</returns>
    public Task<HttpReturnResultDto<string>> CreateCaseAsync(int cmsCaseId, string cmsUsername);
}
