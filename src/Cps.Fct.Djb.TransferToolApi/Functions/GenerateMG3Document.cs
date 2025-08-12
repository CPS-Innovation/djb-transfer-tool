// <copyright file="GenerateMG3Document.cs" company="PlaceholderCompany">
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

/// <summary>
/// Represents a function that is to generate MG3 document.,
/// intended to be accessed via a link from the Casework app.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="GenerateMG3Document"/> class.
/// </remarks>
/// <param name="logger">The logger instance used to log information and errors.</param>
public class GenerateMG3Document(ILogger<GenerateMG3Document> logger)
{
    private readonly ILogger<GenerateMG3Document> logger = logger;

    /// <summary>
    /// The Azure Function that processes an HTTP request for the 'init' route.
    /// </summary>
    /// <param name="req">The HTTP request.</param>
    /// <param name="pcdRequestId">The PCD Request Id.</param>
    /// <returns>An <see cref="IActionResult"/> representing the response of the function.</returns>
    [OpenApiOperation(operationId: "MG3", tags: ["Init"], Description = "Represents a function that generates MG3 document.")]
    [OpenApiParameter("pcdRequestId", In = ParameterLocation.Path, Type = typeof(int), Description = "The case id of the Request.", Required = true)]
    [OpenApiRequestBody("application/json", typeof(InitResult), Description = "Returns on success response.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK)]
    [Function("Init")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "generate-document/mg3/{pcdRequestId}")] HttpRequest req,
        string pcdRequestId)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            this.logger.LogInformation($"{LoggingConstants.HskUiLogPrefix} Init function processed a request.");
            req.HttpContext.Response.Cookies.Delete("HSK");

            // Retrieve query parameters
            string? cc = req.Query["cc"];
            InitResult result = new InitResult();

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
                    this.logger.LogInformation($"{LoggingConstants.HskUiLogPrefix} Milestone: PcdRequestId [{pcdRequestId}] Init function completed in [{stopwatch.Elapsed}]");

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
            this.logger.LogError($"{LoggingConstants.HskUiLogPrefix} Generate MG3 function encountered an error: {ex.Message}");
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
