// <copyright file="IDebugService.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Services.Implementation.Interfaces;

using System.Threading.Tasks;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;
using Cps.MasterDataService.Infrastructure.ApiClient;

/// <summary>
/// Debug service.
/// </summary>
public interface IDebugService
{
    /// <summary>
    /// Get case center case id.
    /// </summary>
    /// <param name="caseCenterSourceSystemId">source system case id.</param>
    /// <returns>Returns the case center case id for via the source system case id.</returns>
    public Task<HttpReturnResultDto<string>> GetCaseCenterCaseIdAsync(string caseCenterSourceSystemId);

    /// <summary>
    /// Get cms case summary.
    /// </summary>
    /// <param name="cmsCaseId">cms case id.</param>
    /// <param name="cmsClassicAuthCookies">cmsClassicAuthCookies.</param>
    /// <param name="cmsModernAuthToken">cmsModernAuthToken.</param>
    /// <returns>Returns the cms case summary.</returns>
    public Task<HttpReturnResultDto<CaseSummary>> GetCmsCaseSymmaryAsync(string cmsCaseId, string cmsClassicAuthCookies, string cmsModernAuthToken);
}
