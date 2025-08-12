// <copyright file="Init.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Functions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using System.Net;
using Cps.Fct.Hk.Common.Contracts.Logging;
using Cps.Fct.Djb.TransferToolApi.Services.Dto;
using Cps.Fct.Djb.TransferToolApi.Services.Contracts;

/// <summary>
/// Represents a function that is the entry point for the Housekeeping application,
/// intended to be accessed via a link from the Casework app.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Init"/> class.
/// </remarks>
/// <param name="logger">The logger instance used to log information and errors.</param>
/// <param name="initService">The service used to process the request and generate the result.</param>
public class Init(ILogger<Init> logger, IInitService initService)
{
    private readonly ILogger<Init> logger = logger;
    private readonly IInitService initService = initService;

    /// <summary>
    /// The Azure Function that processes an HTTP request for the 'init' route.
    /// </summary>
    /// <param name="req">The HTTP request.</param>
    /// <param name="case_id">The case ID passed as a route parameter.</param>
    /// <returns>An <see cref="IActionResult"/> representing the response of the function.</returns>
    [OpenApiOperation(operationId: "Init", tags: ["Init"], Description = "Represents a function that is the entry point for the Housekeeping application.")]
    [OpenApiParameter("case_id", In = ParameterLocation.Path, Type = typeof(int), Description = "The case id of the Request.", Required = true)]
    [OpenApiRequestBody("application/json", typeof(InitResult), Description = "Returns on success response.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK)]
    [Function("Init")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "init/{case_id}")] HttpRequest req, string case_id)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            this.logger.LogInformation($"{LoggingConstants.HskUiLogPrefix} Init function processed a request.");
            req.HttpContext.Response.Cookies.Delete("HSK");

            if (!int.TryParse(case_id, out int caseId))
            {
                return new BadRequestObjectResult($"{LoggingConstants.HskUiLogPrefix} Invalid case_id format. It should be an integer.");
            }

            // Retrieve query parameters
            string? cc = req.Query["cc"];

            // Call the service and simulate async operation
            InitResult result = await Task.Run(() => this.initService.ProcessRequest(req, caseId, cc)).ConfigureAwait(true);

            // Handle the result and return appropriate response
            switch (result.Status)
            {
                case InitResultStatus.BadRequest:
                    return new BadRequestObjectResult(result.Message);

                case InitResultStatus.Redirect:
                    if (string.IsNullOrEmpty(result.RedirectUrl))
                    {
                        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                    }

                    // Check if we need to set the HSK session cookie
                    if (result.ShouldSetCookie && result.CaseId != null)
                    {
                        bool success = this.TryAddHskCookie(req.HttpContext.Response.Cookies, result.CaseId, result.Cc, result.Ct);
                        if (!success)
                        {
                            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                        }
                    }

                    // Return a 302 Found (Redirect) response
                    this.logger.LogInformation($"{LoggingConstants.HskUiLogPrefix} Milestone: caseId [{caseId}] Init function completed in [{stopwatch.Elapsed}]");

                    return new RedirectResult(result.RedirectUrl, permanent: false);

                case InitResultStatus.ServerError:
                    return new ObjectResult(result.Message)
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                    };

                default:
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        catch (Exception ex)
        {
            this.logger.LogError($"{LoggingConstants.HskUiLogPrefix} Init function encountered an error: {ex.Message}");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Attempts to add the HSK cookie to the response.
    /// </summary>
    /// <param name="cookies">The response cookies collection.</param>
    /// <param name="caseId">The case ID to include in the cookie.</param>
    /// <param name="cc">The _cc value to include in the cookie.</param>
    /// <param name="ct">The _ct value to include in the cookie.</param>
    /// <returns>True if the cookie was successfully added; otherwise, false.</returns>
    public virtual bool TryAddHskCookie(IResponseCookies cookies, string caseId, string? cc, string? ct)
    {
        try
        {
            // URL-encode the cc and ct values to handle special characters like semicolons
            string encodedCc = cc != null ? Uri.EscapeDataString(cc) : string.Empty;
            string encodedCt = ct != null ? Uri.EscapeDataString(ct) : string.Empty;

            // Create the cookie value by concatenating case_id, _cc, and _ct
            string cookieValue = $"{caseId}:{cc}:{ct}";

            // Create the cookie options
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                IsEssential = true,
                Path = "/",
            };

            // Add the cookie to the response
            cookies.Append("HSK", cookieValue, cookieOptions);
            return true;
        }
        catch (Exception ex)
        {
            this.logger.LogError($"{LoggingConstants.HskUiLogPrefix} Error adding HSK cookie: {ex.Message}");
            return false;
        }
    }
}
