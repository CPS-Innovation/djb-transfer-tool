// <copyright file="UploadDocumentsFromCmsBundle.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Functions.CaseCenter.Document;

using System.Diagnostics;
using System.Net;
using AutoMapper;
using Cps.Fct.Djb.TransferTool.Shared.Constants;
using Cps.Fct.Djb.TransferToolApi.Functions;
using Cps.Fct.Djb.TransferToolApi.Functions.CaseCenter.Document.Examples;
using Cps.Fct.Djb.TransferToolApi.Models.Requests.Document;
using Cps.Fct.Djb.TransferToolApi.Models.Responses.Document;
using Cps.Fct.Djb.TransferToolApi.Services.Interfaces;
using Cps.Fct.Djb.TransferToolApi.Services.Interfaces.Document;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

/// <summary>
/// Represents a function that uploads documents from Cms bundle a case in Case Center.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="UploadDocumentsFromCmsBundle"/> class.
/// </remarks>
/// <param name="logger">The logger instance used to log information and errors.</param>
/// <param name="autoMapper">IValidationService.</param>
/// <param name="validationService">IMapper.</param>
/// <param name="uploadDocumentsFromCmsBundleService">IUploadDocumentsFromCmsBundleService.</param>
public class UploadDocumentsFromCmsBundle(ILogger<UploadDocumentsFromCmsBundle> logger,
    IValidationService validationService,
    IMapper autoMapper,
    IUploadDocumentsFromCmsBundleService uploadDocumentsFromCmsBundleService)
    : BaseHttpFunction()
{
    private readonly ILogger<UploadDocumentsFromCmsBundle> logger = logger;
    private readonly IMapper autoMapper = autoMapper;
    private readonly IValidationService validationService = validationService;
    private readonly IUploadDocumentsFromCmsBundleService uploadDocumentsFromCmsBundleService = uploadDocumentsFromCmsBundleService;

    /// <summary>
    /// Uploads documents from a cms bundle to a case in Case Center.
    /// </summary>
    /// <param name="request">The HTTP request containing the payload for uploading documents from cms bundle to a case center case.</param>
    /// <returns>
    /// An <see cref="HttpResponseData"/> containing:
    /// - 200 OK: Documents uploaded successfully along with a list of Case Center document ids.
    /// - 400 Bad Request: Missing or invalid request data.
    /// - 403 Unauthorized: Invalid credentials.
    /// - 500 Internal Server Error: Case Center API unavailable or unexpected issue.
    /// </returns>
    [Function(nameof(UploadDocumentsFromCmsBundle))]
    [OpenApiOperation(operationId: "UploadDocumentsFromCmsBundle", tags: ["Document"], Description = "Uploads documents from cms bundle to a case center case using the Case Center API.")]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header, Description = "The Azure Function API Key.")]
    [OpenApiSecurity("cms_auth_values", SecuritySchemeType.ApiKey, Name = "Cms-Auth-Values", In = OpenApiSecurityLocationType.Header, Description = "The CMS Auth Values. This can be retrieved via the Authenticate API Endpoint.")]
    [OpenApiRequestBody("application/json", typeof(UploadDocumentsFromCmsBundleRequest), Required = true, Description = "Payload for uploading the documents to case center.", Example = typeof(UploadDocumentsFromCmsBundleRequestExample))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(DocumentsUploadedFromCmsBundleResponse), Description = "Successfully uploaded documents response.", Example = typeof(DocumentsUploadedFromCmsBundleResponseExample))]
    [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest, Description = "Missing or invalid request data.")]
    [OpenApiResponseWithoutBody(HttpStatusCode.Forbidden, Description = "Invalid username or password.")]
    [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Error uploading documents to case.")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "case-center/case/documents")] HttpRequestData request)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            this.logger.LogInformation($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{this.GetFunctionQualifiedName(typeof(UploadDocumentsFromCmsBundle))}.{nameof(this.Run)}: Milestone - Function about to process a request.");

            if (!this.HasCmsAuthValuesHeader(request))
            {
                var unauthorizedResponse = request.CreateResponse(HttpStatusCode.Unauthorized);
                await unauthorizedResponse.WriteStringAsync("Invalid or missing auth headers.").ConfigureAwait(false);
                return unauthorizedResponse;
            }

            throw new NotImplementedException();

            //var cmsAuthValues = this.GetCmsAuthValues(request);

            //var requestBodyPayload = await this.TryReadRequestBodyAsync<CreateCaseRequest>(request).ConfigureAwait(false);

            //if (requestBodyPayload == null)
            //{
            //    var badRequestResponse = request.CreateResponse(HttpStatusCode.BadRequest);
            //    await badRequestResponse.WriteStringAsync("Invalid or missing request body.").ConfigureAwait(false);
            //    return badRequestResponse;
            //}

            //var validateModelResult = await this.validationService.ValidateModelAsync(requestBodyPayload).ConfigureAwait(false);

            //if (!validateModelResult.IsSuccess)
            //{
            //    return await this.BuildBadRequestResponseFromValidationAsync(request, validateModelResult.Data).ConfigureAwait(false);
            //}

            //var createCaseDto = this.autoMapper.Map<CreateCaseDto>((requestBodyPayload, cmsAuthValues));
            //var createCaseResult = await this.createCaseCenterCaseService.CreateCaseAsync(createCaseDto).ConfigureAwait(false);

            //if (!createCaseResult.IsSuccess)
            //{
            //    return request.CreateResponse(createCaseResult.StatusCode);
            //}

            //var responseData = request.CreateResponse(HttpStatusCode.OK);
            //await responseData.WriteAsJsonAsync(createCaseResult.Data).ConfigureAwait(false);
            //return responseData;
        }
        catch (Exception ex)
        {
            this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{this.GetFunctionQualifiedName(typeof(UploadDocumentsFromCmsBundle))}.{nameof(this.Run)}: Milestone - Function encountered an error: {ex.Message}");
            return request.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}
