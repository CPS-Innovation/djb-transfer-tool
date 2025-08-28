// <copyright file="CreateCaseCenterCaseService.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Services;

using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Cps.Fct.Djb.TransferTool.Shared.Constants;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Factories.Interfaces;
using Cps.Fct.Djb.TransferToolApi.Services.Interfaces;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Case;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Mds;
using Microsoft;
using Microsoft.Extensions.Logging;

/// <summary>
/// Create case center case service.
/// </summary>
public class CreateCaseCenterCaseService : ICreateCaseCenterCaseService
{
    private readonly ILogger<CreateCaseCenterCaseService> logger;
    private readonly ICaseCenterApiClientFactory caseCenterApiClientFactory;
    private readonly IMdsApiClientFactory mdsApiClientFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCaseCenterCaseService"/> class.
    /// </summary>
    /// <param name="logger">ILogger CreateCaseCenterCaseService.</param>
    /// <param name="caseCenterApiClientFactory">ICaseCenterApiClientFactory.</param>
    /// <param name="mdsApiClientFactory">IMdsApiClientFactory.</param>
    public CreateCaseCenterCaseService(
        ILogger<CreateCaseCenterCaseService> logger,
        ICaseCenterApiClientFactory caseCenterApiClientFactory,
        IMdsApiClientFactory mdsApiClientFactory)
    {
        this.logger = logger;
        this.caseCenterApiClientFactory = caseCenterApiClientFactory ?? throw new ArgumentNullException(nameof(caseCenterApiClientFactory));
        this.mdsApiClientFactory = mdsApiClientFactory;
    }

    /// <summary>
    /// Create a case in Case Center.
    /// </summary>
    /// <param name="cmsCaseId">The cms unique case id.</param>
    /// <param name="cmsUsername">The username of the person creating the case.</param>
    /// <returns>Returns the case center case idfor the created case.</returns>
    public async Task<HttpReturnResultDto<string>> CreateCaseAsync(int cmsCaseId, string cmsUsername)
    {
        try
        {
            Requires.NotNull(cmsCaseId);
            Requires.NotNull(cmsUsername);

            // FXS: Populate these for development/debugging only - in production these will come from the user's session
            var cmsCookies = string.Empty;
            var cmsModernToken = string.Empty;

            // get the case from MDS
            var cookie = new MdsCookie(cmsCookies, cmsModernToken);
            var client = this.mdsApiClientFactory.Create(JsonSerializer.Serialize(cookie));
            var caseSummary = await client.GetCaseSummaryAsync(cmsCaseId).ConfigureAwait(false);

            if (caseSummary is null)
            {
                var message = $"No case found in MDS for CMS Case ID {cmsCaseId}";
                this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(CreateCaseCenterCaseService)}.{nameof(this.CreateCaseAsync)}: Milestone - {message}");
                return HttpReturnResultDto<string>.Fail(HttpStatusCode.NotFound, message);
            }

            var caseToCreate = new CreateCaseDto()
            {
                CmsCaseId = cmsCaseId,
                CaseCreator = cmsUsername,
                AreaCrownCourtCode = caseSummary?.NextHearingVenueCode ?? string.Empty,
                CaseUrn = caseSummary?.Urn ?? string.Empty,
                CaseTitle = caseSummary?.LeadDefendantFirstNames + " " + caseSummary?.LeadDefendantSurname?.ToUpper(),
            };

            // get the admin auth token from case center
            var caseCenterApiClient = this.caseCenterApiClientFactory.Create(string.Empty);

            var getAdminAuthTokenResponse = await caseCenterApiClient.GetAuthTokenAsync().ConfigureAwait(false);

            if (!getAdminAuthTokenResponse.IsSuccess)
            {
                return getAdminAuthTokenResponse;
            }

            var caseCenterAuthToken = getAdminAuthTokenResponse.Data;

            // now create the case
            var createCaseCenterCaseResponse = await caseCenterApiClient.CreateCaseAsync(caseCenterAuthToken, caseToCreate).ConfigureAwait(false);

            if (!createCaseCenterCaseResponse.IsSuccess)
            {
                return createCaseCenterCaseResponse;
            }

            var caseCenterCaseId = createCaseCenterCaseResponse.Data;

            return HttpReturnResultDto<string>.Success(HttpStatusCode.Created, caseCenterCaseId);
        }
        catch (Exception ex)
        {
            var message = $"Service encountered an error: {ex.Message}";
            this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(CreateCaseCenterCaseService)}.{nameof(this.CreateCaseAsync)}: Milestone - {message}");
            return HttpReturnResultDto<string>.Fail(HttpStatusCode.InternalServerError, message);
        }
    }
}
