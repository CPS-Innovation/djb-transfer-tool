// <copyright file="CreateCaseCenterCaseService.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Services.Implementation;

using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Cps.Fct.Djb.TransferTool.Shared.Constants;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Factories.Interfaces;
using Cps.Fct.Djb.TransferToolApi.Services.Implementation.Interfaces;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.CaseCenter;
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
    /// <param name="inputCreateCaseDto">The payload for the case to be created.</param>
    /// <returns>Returns the case center case idfor the created case.</returns>
    public async Task<HttpReturnResultDto<string>> CreateCaseAsync(CreateCaseDto inputCreateCaseDto)
    {
        try
        {
            Requires.NotNull(inputCreateCaseDto);
            Requires.NotNull(inputCreateCaseDto.CmsCaseId);
            Requires.NotNull(inputCreateCaseDto.CaseCreator);
            Requires.NotNull(inputCreateCaseDto.CmsModernAuthToken);
            Requires.NotNull(inputCreateCaseDto.CmsClassicAuthCookies);

            // get the case from MDS
            var cookie = new MdsCookie(inputCreateCaseDto.CmsClassicAuthCookies, inputCreateCaseDto.CmsModernAuthToken);
            var client = this.mdsApiClientFactory.Create(JsonSerializer.Serialize(cookie));
            var caseSummary = await client.GetCaseSummaryAsync(inputCreateCaseDto.CmsCaseId).ConfigureAwait(false);

            if (caseSummary is null)
            {
                var message = $"No case found in MDS for CMS Case ID {inputCreateCaseDto.CmsCaseId}";
                this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(CreateCaseCenterCaseService)}.{nameof(this.CreateCaseAsync)}: Milestone - {message}");
                return HttpReturnResultDto<string>.Fail(HttpStatusCode.NotFound, message);
            }

            #pragma warning disable SA1101 // Prefix local calls with this
            var caseToCreateDto = inputCreateCaseDto with
            {
                AreaCrownCourtCode = caseSummary.NextHearingVenueCode ?? string.Empty,
                CaseUrn = caseSummary.Urn ?? string.Empty,
                CaseTitle = caseSummary.LeadDefendantFirstNames + " " + caseSummary.LeadDefendantSurname?.ToUpper(),
            };
            #pragma warning restore SA1101

            // get the admin auth token from case center
            var caseCenterApiClient = this.caseCenterApiClientFactory.Create(string.Empty);

            var getAdminAuthTokenResponse = await caseCenterApiClient.GetAuthTokenAsync().ConfigureAwait(false);

            if (!getAdminAuthTokenResponse.IsSuccess)
            {
                return getAdminAuthTokenResponse;
            }

            var caseCenterAuthToken = getAdminAuthTokenResponse.Data;

            // now create the case
            var createCaseCenterCaseResponse = await caseCenterApiClient.CreateCaseAsync(caseCenterAuthToken, caseToCreateDto).ConfigureAwait(false);

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
