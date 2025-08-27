// <copyright file="CreateCaseCenterCaseService.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Services;

using System.Net;
using System.Threading.Tasks;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Factories.Interfaces;
using Cps.Fct.Djb.TransferToolApi.Services.Interfaces;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Auth;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;
using Microsoft.Extensions.Logging;
using Microsoft;
using Azure.Core;
using Cps.Fct.Djb.TransferTool.Shared.Constants;

/// <summary>
/// Create case center case service.
/// </summary>
public class CreateCaseCenterCaseService : ICreateCaseCenterCaseService
{
    private readonly ILogger<CreateCaseCenterCaseService> logger;
    private readonly ICaseCenterApiClientFactory caseCenterApiClientFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCaseCenterCaseService"/> class.
    /// </summary>
    /// <param name="logger">ILogger CreateCaseCenterCaseService</param>
    /// <param name="caseCenterApiClientFactory">ICaseCenterApiClientFactory.</param>
    public CreateCaseCenterCaseService(
        ILogger<CreateCaseCenterCaseService> logger,
        ICaseCenterApiClientFactory caseCenterApiClientFactory)
    {
        this.logger = logger;
        this.caseCenterApiClientFactory = caseCenterApiClientFactory ?? throw new ArgumentNullException(nameof(caseCenterApiClientFactory));
    }

    /// <summary>
    /// Create a case in Case Center.
    /// </summary>
    /// <returns>Returns the case center case idfor the created case.</returns>
    public async Task<HttpReturnResultDto<string>> CreateCaseAsync()
    {
        try
        {
            // get the admin auth token from case center
            var caseCenterApiClient = this.caseCenterApiClientFactory.Create(string.Empty);

            var getAdminAuthTokenResponse = await caseCenterApiClient.GetAuthTokenAsync().ConfigureAwait(false);

            if (!getAdminAuthTokenResponse.IsSuccess)
            {
                return getAdminAuthTokenResponse;
            }

            var caseCenterAuthToken = getAdminAuthTokenResponse.Data;




            return HttpReturnResultDto<string>.Success(HttpStatusCode.Created, caseCenterAuthToken);

            //// first get the template
            //var getTemplateResponse = await this.caseCenterApiClient.GetTemplateCaseAsync(new AuthenticatedUserDto() { CaseCenterAuthToken = caseCenterAuthToken });

            //if (!getTemplateResponse.IsSuccess)
            //{
            //    return getTemplateResponse;
            //}

            //var templateId = getTemplateResponse.Data;

            //// now create the case
            //createCaseDto.CaseCenterAuthToken = caseCenterAuthToken;
            //createCaseDto.TemplateId = templateId;
            //createCaseDto.OrganisationId = caseCenterOptions.OrganisationId;
            //createCaseDto.Type = caseCenterOptions.OrganisationType;

            //var createCaseResponse = await this.caseCenterApiClient.CreateCaseAsync(createCaseDto);

            //if (!createCaseResponse.IsSuccess)
            //{
            //    return createCaseResponse;
            //}

            //var caseCenterCaseId = createCaseResponse.Data;

            //// now get all the bundles for the case
            //var getBundlesForCaseResponse = await this.caseCenterApiClient.GetCaseBundlesAsync(new GetCaseBundleDto()
            //{
            //    CaseCenterAuthToken = caseCenterAuthToken,
            //    CaseCenterCaseId = caseCenterCaseId
            //});

            //if (!getBundlesForCaseResponse.IsSuccess)
            //{
            //    return HttpReturnResultDto<string>.Fail(HttpStatusCode.InternalServerError, getBundlesForCaseResponse.Message);
            //}

            //var bundleIds = getBundlesForCaseResponse.Data;

            //// now add the user to the case
            //var addUserToCaseResponse = await this.caseCenterApiClient.AddUserToCaseAsync(new AddUserToCaseDto()
            //{
            //    BundleIds = bundleIds,
            //    CaseCenterAuthToken = caseCenterAuthToken,
            //    CaseCenterCaseId = caseCenterCaseId,
            //    Username = createCaseDto.CaseCreator
            //});

            //if (!addUserToCaseResponse.IsSuccess)
            //{
            //    return HttpReturnResultDto<string>.Fail(HttpStatusCode.InternalServerError, addUserToCaseResponse.Message);
            //}

            // return HttpReturnResultDto<string>.Success(HttpStatusCode.Created, caseCenterCaseId);
        }
        catch (Exception ex)
        {
            var message = $"Service encountered an error: {ex.Message}";
            this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(CreateCaseCenterCaseService)}.{nameof(this.CreateCaseAsync)}: Milestone - {message}");
            return HttpReturnResultDto<string>.Fail(HttpStatusCode.InternalServerError, message);
        }
    }
}
