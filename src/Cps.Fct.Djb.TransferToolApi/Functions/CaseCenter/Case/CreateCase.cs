// <copyright file="CreateCase.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Functions.CaseCenter.Case;

using System.Diagnostics;
using System.Net;
using AutoMapper;
using Cps.Fct.Djb.TransferTool.Shared.Constants;
using Cps.Fct.Djb.TransferToolApi.Functions;
using Cps.Fct.Djb.TransferToolApi.Functions.CaseCenter.Case.Examples;
using Cps.Fct.Djb.TransferToolApi.Models.Requests.Case;
using Cps.Fct.Djb.TransferToolApi.Models.Responses.Case;
using Cps.Fct.Djb.TransferToolApi.Services.Interfaces;
using Cps.Fct.Djb.TransferToolApi.Services.Interfaces.Case;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.CaseCenter.Case;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

/// <summary>
/// Represents a function that creates a case in Case Center.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Case.CreateCase"/> class.
/// </remarks>
/// <param name="logger">The logger instance used to log information and errors.</param>
/// <param name="autoMapper">IValidationService.</param>
/// <param name="validationService">IMapper.</param>
/// <param name="createCaseCenterCaseService">ICreateCaseCenterCaseService.</param>
public class CreateCase(ILogger<CreateCase> logger,
    IValidationService validationService,
    IMapper autoMapper,
    ICreateCaseService createCaseCenterCaseService)
    : BaseHttpFunction()
{
    private readonly ILogger<CreateCase> logger = logger;
    private readonly IMapper autoMapper = autoMapper;
    private readonly IValidationService validationService = validationService;
    private readonly ICreateCaseService createCaseCenterCaseService = createCaseCenterCaseService;

    /// <summary>
    /// Creates a case in Case Center.
    /// </summary>
    /// <param name="request">The HTTP request containing the payload for creating a case.</param>
    /// <returns>
    /// An <see cref="HttpResponseData"/> containing:
    /// - 200 OK: Case created successfully along with the Case Center case id.
    /// - 400 Bad Request: Missing or invalid request data.
    /// - 403 Unauthorized: Invalid credentials.
    /// - 500 Internal Server Error: Case Center API unavailable or unexpected issue.
    /// </returns>
    [Function(nameof(Case.CreateCase))]
    [OpenApiOperation(operationId: "CreateCaseCenterCase", tags: ["Case"], Description = "Creates a case in Case Center using the Case Center API.")]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header, Description = "The Azure Function API Key.")]
    [OpenApiSecurity("cms_auth_values", SecuritySchemeType.ApiKey, Name = "Cms-Auth-Values", In = OpenApiSecurityLocationType.Header, Description = "The CMS Auth Values. This can be retrieved via the Authenticate API Endpoint.")]
    [OpenApiRequestBody("application/json", typeof(CreateCaseRequest), Required = true, Description = "Payload for creating the case.", Example = typeof(CreateCaseRequestExample))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(CaseCreatedResponse), Description = "Successfully created a case.", Example = typeof(CaseCreatedResponseExample))]
    [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest, Description = "Missing or invalid request data.")]
    [OpenApiResponseWithoutBody(HttpStatusCode.Forbidden, Description = "Invalid username or password.")]
    [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Error creating case.")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "case-center/case")] HttpRequestData request)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            this.logger.LogInformation($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{this.GetFunctionQualifiedName(typeof(CreateCase))}.{nameof(this.Run)}: Milestone - Function about to process a request.");

            if (!this.HasCmsAuthValuesHeader(request))
            {
                var unauthorizedResponse = request.CreateResponse(HttpStatusCode.Unauthorized);
                await unauthorizedResponse.WriteStringAsync("Invalid or missing auth headers.").ConfigureAwait(false);
                return unauthorizedResponse;
            }

            var cmsAuthValues = this.GetCmsAuthValues(request);

            var requestBodyPayload = await this.TryReadRequestBodyAsync<CreateCaseRequest>(request).ConfigureAwait(false);

            if (requestBodyPayload == null)
            {
                var badRequestResponse = request.CreateResponse(HttpStatusCode.BadRequest);
                await badRequestResponse.WriteStringAsync("Invalid or missing request body.").ConfigureAwait(false);
                return badRequestResponse;
            }

            var validateModelResult = await this.validationService.ValidateModelAsync(requestBodyPayload).ConfigureAwait(false);

            if (!validateModelResult.IsSuccess)
            {
                return await this.BuildBadRequestResponseFromValidationAsync(request, validateModelResult.Data).ConfigureAwait(false);
            }

            var createCaseDto = this.autoMapper.Map<CreateCaseDto>((requestBodyPayload, cmsAuthValues));
            var createCaseResult = await this.createCaseCenterCaseService.CreateCaseAsync(createCaseDto).ConfigureAwait(false);

            if (!createCaseResult.IsSuccess)
            {
                return request.CreateResponse(createCaseResult.StatusCode);
            }

            var responseData = request.CreateResponse(HttpStatusCode.OK);
            await responseData.WriteAsJsonAsync(createCaseResult.Data).ConfigureAwait(false);
            return responseData;
        }
        catch (Exception ex)
        {
            this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{this.GetFunctionQualifiedName(typeof(CreateCase))}.{nameof(this.Run)}: Milestone - Function encountered an error: {ex.Message}");
            return request.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}
