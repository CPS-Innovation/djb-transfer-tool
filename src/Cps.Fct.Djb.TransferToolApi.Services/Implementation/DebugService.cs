// <copyright file="DebugService.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Services.Implementation;

using System.Globalization;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Cps.Fct.Djb.TransferTool.Shared.Constants;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Factories.Interfaces;
using Cps.Fct.Djb.TransferToolApi.Services.Interfaces;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Mds;
using Cps.MasterDataService.Infrastructure.ApiClient;
using Microsoft;
using Microsoft.Extensions.Logging;

/// <summary>
/// Debug service.
/// </summary>
public class DebugService : IDebugService
{
    private readonly ILogger<DebugService> logger;
    private readonly ICaseCenterApiClientFactory caseCenterApiClientFactory;
    private readonly IMdsApiClientFactory mdsApiClientFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="DebugService"/> class.
    /// </summary>
    /// <param name="logger">ILogger DebugService.</param>
    /// <param name="caseCenterApiClientFactory">ICaseCenterApiClientFactory.</param>
    /// <param name="mdsApiClientFactory">IMdsApiClientFactory.</param>
    public DebugService(
        ILogger<DebugService> logger,
        ICaseCenterApiClientFactory caseCenterApiClientFactory,
        IMdsApiClientFactory mdsApiClientFactory)
    {
        this.logger = logger;
        this.caseCenterApiClientFactory = caseCenterApiClientFactory ?? throw new ArgumentNullException(nameof(caseCenterApiClientFactory));
        this.mdsApiClientFactory = mdsApiClientFactory;
    }

    /// <summary>
    /// Get case center case id.
    /// </summary>
    /// <param name="caseCenterSourceSystemId">source system case id.</param>
    /// <returns>Returns the case center case id for via the source system case id.</returns>
    public async Task<HttpReturnResultDto<string>> GetCaseCenterCaseIdAsync(string caseCenterSourceSystemId)
    {
        try
        {
            Requires.NotNullOrWhiteSpace(caseCenterSourceSystemId);

            // get the admin auth token from case center
            var caseCenterApiClient = this.caseCenterApiClientFactory.Create(string.Empty);

            var getAdminAuthTokenResponse = await caseCenterApiClient.GetAuthTokenAsync().ConfigureAwait(false);

            if (!getAdminAuthTokenResponse.IsSuccess)
            {
                return getAdminAuthTokenResponse;
            }

            var caseCenterAuthToken = getAdminAuthTokenResponse.Data;

            // get case center case id
            var getCaseCenterCaseIdResponse = await caseCenterApiClient.GetCaseIdAsync(caseCenterAuthToken, caseCenterSourceSystemId).ConfigureAwait(false);

            if (!getCaseCenterCaseIdResponse.IsSuccess)
            {
                return getCaseCenterCaseIdResponse;
            }

            var caseCenterCaseId = getCaseCenterCaseIdResponse.Data;

            return HttpReturnResultDto<string>.Success(HttpStatusCode.Created, caseCenterCaseId);
        }
        catch (Exception ex)
        {
            var message = $"Service encountered an error: {ex.Message}";
            this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(DebugService)}.{nameof(this.GetCaseCenterCaseIdAsync)}: Milestone - {message}");
            return HttpReturnResultDto<string>.Fail(HttpStatusCode.InternalServerError, message);
        }
    }

    /// <summary>
    /// Get cms case summary.
    /// </summary>
    /// <param name="cmsCaseId">cms case id.</param>
    /// <param name="cmsClassicAuthCookies">cmsClassicAuthCookies.</param>
    /// <param name="cmsModernAuthToken">cmsModernAuthToken.</param>
    /// <returns>Returns the cms case summary.</returns>
    public async Task<HttpReturnResultDto<CaseSummary>> GetCmsCaseSymmaryAsync(string cmsCaseId, string cmsClassicAuthCookies, string cmsModernAuthToken)
    {
        try
        {
            Requires.NotNullOrWhiteSpace(cmsCaseId);

            // get the case from MDS
            var cookie = new MdsCookie(cmsClassicAuthCookies, cmsModernAuthToken);
            var client = this.mdsApiClientFactory.Create(JsonSerializer.Serialize(cookie));
            var caseSummary = await client.GetCaseSummaryAsync(int.Parse(cmsCaseId, CultureInfo.InvariantCulture)).ConfigureAwait(false);

            if (caseSummary is null)
            {
                var message = $"No case found in MDS for CMS Case ID {cmsCaseId}";
                this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(DebugService)}.{nameof(this.GetCmsCaseSymmaryAsync)}: Milestone - {message}");
                return HttpReturnResultDto<CaseSummary>.Fail(HttpStatusCode.NotFound, message);
            }

            return HttpReturnResultDto<CaseSummary>.Success(HttpStatusCode.Created, caseSummary);
        }
        catch (Exception ex)
        {
            var message = $"Service encountered an error: {ex.Message}";
            this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(DebugService)}.{nameof(this.GetCaseCenterCaseIdAsync)}: Milestone - {message}");
            return HttpReturnResultDto<CaseSummary>.Fail(HttpStatusCode.InternalServerError, message);
        }
    }
}
