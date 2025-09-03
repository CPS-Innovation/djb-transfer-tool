// <copyright file="DebugFunctions.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Functions;

using System.Net;
using Cps.Fct.Djb.TransferTool.FunctionApp.Functions.CaseCenter.Case.Examples;
using Cps.Fct.Djb.TransferTool.Shared.Constants;
using Cps.Fct.Djb.TransferToolApi.Models.Responses;
using Cps.Fct.Djb.TransferToolApi.Services.Implementation.Interfaces;
using Cps.MasterDataService.Infrastructure.ApiClient;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

/// <summary>
/// Debug functions.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DebugFunctions"/> class.
/// </remarks>
/// <param name="logger">The logger instance used to log information and errors.</param>
/// <param name="debugService">IDebugService.</param>
public class DebugFunctions(ILogger<DebugFunctions> logger,
    IDebugService debugService)
    : BaseHttpFunction()
{
    private readonly ILogger<DebugFunctions> logger = logger;
    private readonly IDebugService debugService = debugService;

    /// <summary>
    /// DebugCallAnonymous.
    /// </summary>
    /// <param name="request">The HTTP request containing the payload for call.</param>
    /// <returns>
    /// An <see cref="HttpResponseData"/> containing:
    /// - 200 OK.
    /// </returns>
    [Function($"{nameof(DebugFunctions)}CallAnonymous")]
    public async Task<HttpResponseData> CallAnonymous(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "debug/anonymous")] HttpRequestData request)
    {
        try
        {
            var responseData = request.CreateResponse(HttpStatusCode.OK);
            return responseData;
        }
        catch (Exception ex)
        {
            this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(DebugFunctions)}CallAnonymous: Milestone - Function encountered an error: {ex.Message}");
            return request.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }

    /// <summary>
    /// CallFunctionKey.
    /// </summary>
    /// <param name="request">The HTTP request containing the payload for call.</param>
    /// <returns>
    /// An <see cref="HttpResponseData"/> containing:
    /// - 200 OK.
    /// </returns>
    [Function($"{nameof(DebugFunctions)}CallFunctionKey")]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header, Description = "The Azure Function API Key.")]
    public async Task<HttpResponseData> CallFunctionKey(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "debug/functionkey")] HttpRequestData request)
    {
        try
        {
            var responseData = request.CreateResponse(HttpStatusCode.OK);
            return responseData;
        }
        catch (Exception ex)
        {
            this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(DebugFunctions)}CallFunctionKey: Milestone - Function encountered an error: {ex.Message}");
            return request.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }

    /// <summary>
    /// CallAnonymousCaseCenter.
    /// </summary>
    /// <param name="request">The HTTP request containing the payload for call.</param>
    /// <param name="sourceSystemCaseId">The source system case id.</param>
    /// <returns>
    /// An <see cref="HttpResponseData"/> containing:
    /// - 200 OK.
    /// </returns>
    [Function($"{nameof(DebugFunctions)}CallAnonymousCaseCenter")]
    [OpenApiParameter("sourceSystemCaseId", In = ParameterLocation.Path, Type = typeof(string), Description = "The source system case id", Required = true)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(string), Description = "Case center case id.")]
    public async Task<HttpResponseData> CallAnonymousCaseCenter(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "debug/anonymous/casecenter/{sourceSystemCaseId}")] HttpRequestData request,
        string sourceSystemCaseId)
    {
        try
        {
            var getCaseCenterCaseIdResponse = await this.debugService.GetCaseCenterCaseIdAsync(sourceSystemCaseId).ConfigureAwait(false);

            if (!getCaseCenterCaseIdResponse.IsSuccess)
            {
                return request.CreateResponse(getCaseCenterCaseIdResponse.StatusCode);
            }

            var responseData = request.CreateResponse(HttpStatusCode.OK);
            await responseData.WriteAsJsonAsync(getCaseCenterCaseIdResponse.Data).ConfigureAwait(false);
            return responseData;
        }
        catch (Exception ex)
        {
            this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(DebugFunctions)}CallAnonymousCaseCenter: Milestone - Function encountered an error: {ex.Message}");
            return request.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }

    /// <summary>
    /// CallFunctionKeyCaseCenter.
    /// </summary>
    /// <param name="request">The HTTP request containing the payload for call.</param>
    /// <param name="sourceSystemCaseId">The source system case id.</param>
    /// <returns>
    /// An <see cref="HttpResponseData"/> containing:
    /// - 200 OK.
    /// </returns>
    [Function($"{nameof(DebugFunctions)}CallFunctionKeyCaseCenter")]
    [OpenApiParameter("sourceSystemCaseId", In = ParameterLocation.Path, Type = typeof(string), Description = "The source system case id", Required = true)]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header, Description = "The Azure function API Key.")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(string), Description = "Case center case id.")]
    public async Task<HttpResponseData> CallFunctionKeyCaseCenter(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "debug/functionkey/casecenter/{sourceSystemCaseId}")] HttpRequestData request,
        string sourceSystemCaseId)
    {
        try
        {
            var getCaseCenterCaseIdResponse = await this.debugService.GetCaseCenterCaseIdAsync(sourceSystemCaseId).ConfigureAwait(false);

            if (!getCaseCenterCaseIdResponse.IsSuccess)
            {
                return request.CreateResponse(getCaseCenterCaseIdResponse.StatusCode);
            }

            var responseData = request.CreateResponse(HttpStatusCode.OK);
            await responseData.WriteAsJsonAsync(getCaseCenterCaseIdResponse.Data).ConfigureAwait(false);
            return responseData;
        }
        catch (Exception ex)
        {
            this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(DebugFunctions)}CallFunctionKeyCaseCenter: Milestone - Function encountered an error: {ex.Message}");
            return request.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }

    /// <summary>
    /// CallAnonymousCms.
    /// </summary>
    /// <param name="request">The HTTP request containing the payload for call.</param>
    /// <param name="cmsCaseId">The cms case id.</param>
    /// <returns>
    /// An <see cref="HttpResponseData"/> containing:
    /// - 200 OK.
    /// </returns>
    [Function($"{nameof(DebugFunctions)}CallAnonymousCms")]
    [OpenApiParameter("cmsCaseId", In = ParameterLocation.Path, Type = typeof(string), Description = "The source system case id", Required = true)]
    [OpenApiSecurity("cms_auth_values", SecuritySchemeType.ApiKey, Name = "Cms-Auth-Values", In = OpenApiSecurityLocationType.Header, Description = "The CMS Auth Values. This can be retrieved via the Authenticate API Endpoint.")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(CaseSummary), Description = "Cms Case Summary.")]
    public async Task<HttpResponseData> CallAnonymousCms(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "debug/anonymous/cms/{cmsCaseId}")] HttpRequestData request,
        string cmsCaseId)
    {
        try
        {
            if (!this.HasCmsAuthValuesHeader(request))
            {
                var unauthorizedResponse = request.CreateResponse(HttpStatusCode.Unauthorized);
                await unauthorizedResponse.WriteStringAsync("Invalid or missing auth headers.").ConfigureAwait(false);
                return unauthorizedResponse;
            }

            var cmsAuthValues = this.GetCmsAuthValues(request);

            if (cmsAuthValues is null)
            {
                var unauthorizedResponse = request.CreateResponse(HttpStatusCode.Unauthorized);
                await unauthorizedResponse.WriteStringAsync("Invalid or missing auth headers.").ConfigureAwait(false);
                return unauthorizedResponse;
            }

            var getCmsCaseSummaryResponse = await this.debugService.GetCmsCaseSymmaryAsync(cmsCaseId, cmsAuthValues.CmsCookies, cmsAuthValues.CmsModernToken).ConfigureAwait(false);

            if (!getCmsCaseSummaryResponse.IsSuccess)
            {
                return request.CreateResponse(getCmsCaseSummaryResponse.StatusCode);
            }

            var responseData = request.CreateResponse(HttpStatusCode.OK);
            await responseData.WriteAsJsonAsync(getCmsCaseSummaryResponse.Data).ConfigureAwait(false);
            return responseData;
        }
        catch (Exception ex)
        {
            this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(DebugFunctions)}CallAnonymousCms: Milestone - Function encountered an error: {ex.Message}");
            return request.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }

    /// <summary>
    /// CallFunctionKeyCms.
    /// </summary>
    /// <param name="request">The HTTP request containing the payload for call.</param>
    /// <param name="cmsCaseId">The source system case id.</param>
    /// <returns>
    /// An <see cref="HttpResponseData"/> containing:
    /// - 200 OK.
    /// </returns>
    [Function($"{nameof(DebugFunctions)}CallFunctionKeyCms")]
    [OpenApiParameter("cmsCaseId", In = ParameterLocation.Path, Type = typeof(string), Description = "The cms case id", Required = true)]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header, Description = "The Azure function API Key.")]
    [OpenApiSecurity("cms_auth_values", SecuritySchemeType.ApiKey, Name = "Cms-Auth-Values", In = OpenApiSecurityLocationType.Header, Description = "The CMS Auth Values. This can be retrieved via the Authenticate API Endpoint.")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(CaseSummary), Description = "Cms case summary.")]
    public async Task<HttpResponseData> CallFunctionKeyCms(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "debug/functionkey/cms/{cmsCaseId}")] HttpRequestData request,
        string cmsCaseId)
    {
        try
        {
            if (!this.HasCmsAuthValuesHeader(request))
            {
                var unauthorizedResponse = request.CreateResponse(HttpStatusCode.Unauthorized);
                await unauthorizedResponse.WriteStringAsync("Invalid or missing auth headers.").ConfigureAwait(false);
                return unauthorizedResponse;
            }

            var cmsAuthValues = this.GetCmsAuthValues(request);

            if (cmsAuthValues is null)
            {
                var unauthorizedResponse = request.CreateResponse(HttpStatusCode.Unauthorized);
                await unauthorizedResponse.WriteStringAsync("Invalid or missing auth headers.").ConfigureAwait(false);
                return unauthorizedResponse;
            }

            var getCmsCaseSummaryResponse = await this.debugService.GetCmsCaseSymmaryAsync(cmsCaseId, cmsAuthValues.CmsCookies, cmsAuthValues.CmsModernToken).ConfigureAwait(false);

            if (!getCmsCaseSummaryResponse.IsSuccess)
            {
                return request.CreateResponse(getCmsCaseSummaryResponse.StatusCode);
            }

            var responseData = request.CreateResponse(HttpStatusCode.OK);
            await responseData.WriteAsJsonAsync(getCmsCaseSummaryResponse.Data).ConfigureAwait(false);
            return responseData;
        }
        catch (Exception ex)
        {
            this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(DebugFunctions)}CallFunctionKeyCms: Milestone - Function encountered an error: {ex.Message}");
            return request.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}
