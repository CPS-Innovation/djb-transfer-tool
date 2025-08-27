// <copyright file="CreateCaseCenterCase.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Functions;

using System.Diagnostics;
using System.Net;
using Cps.Fct.Djb.TransferTool.FunctionApp.Functions.CaseCenter.Case.Examples;
using Cps.Fct.Djb.TransferTool.Shared.Constants;
using Cps.Fct.Djb.TransferToolApi.Functions.Examples;
using Cps.Fct.Djb.TransferToolApi.Models.Requests;
using Cps.Fct.Djb.TransferToolApi.Models.Responses;
using Cps.Fct.Djb.TransferToolApi.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

/// <summary>
/// Represents a function that creates a case in Case Center.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CreateCaseCenterCase"/> class.
/// </remarks>
/// <param name="logger">The logger instance used to log information and errors.</param>
/// <param name="createCaseCenterCaseService">ICreateCaseCenterCaseService</param>
public class CreateCaseCenterCase(ILogger<CreateCaseCenterCase> logger,
    ICreateCaseCenterCaseService createCaseCenterCaseService)
{
    private readonly ILogger<CreateCaseCenterCase> logger = logger;
    private readonly ICreateCaseCenterCaseService createCaseCenterCaseService = createCaseCenterCaseService;

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
    [Function(nameof(CreateCaseCenterCase))]
    [OpenApiOperation(operationId: "CreateCaseCenterCase", tags: ["Case"], Description = "Creates a case in Case Center using the Case Center API.")]
    [OpenApiRequestBody("application/json", typeof(CreateCaseRequest), Required = true, Description = "Payload for creating the case.", Example = typeof(CreateCaseCenterCaseRequestExample))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(CaseCreatedResponse), Description = "Successfully created a case.", Example = typeof(CreateCaseCenterCaseResponseExample))]
    [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest, Description = "Missing or invalid request data.")]
    [OpenApiResponseWithoutBody(HttpStatusCode.Forbidden, Description = "Invalid username or password.")]
    [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Error creating case.")]
    public async Task<HttpResponseData> CreateCase(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "case-center/case")] HttpRequestData request)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            this.logger.LogInformation($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(CreateCaseCenterCase)}.{nameof(this.CreateCase)}: Milestone - Function about to process a request.");

            var response = await this.createCaseCenterCaseService.CreateCaseAsync().ConfigureAwait(false);

            var responseData = request.CreateResponse(HttpStatusCode.OK);
            return responseData;
        }
        catch (Exception ex)
        {
            this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(CreateCaseCenterCase)}.{nameof(this.CreateCase)}: Milestone - Function encountered an error: {ex.Message}");
            return request.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}
