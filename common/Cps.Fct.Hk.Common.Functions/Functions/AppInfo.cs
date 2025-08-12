// <copyright file="AppInfo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.Functions.Functions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using System.Net;
using Microsoft.OpenApi.Models;
using Cps.Fct.Hk.Common.Contracts.Logging;
using Cps.Fct.Hk.Common.Contracts.Abstractions;
using System.Text.Json.Serialization;
using System.Text.Json;
using Cps.Fct.Hk.Common.Functions.Extensions;

/// <summary>
/// Represents a function that retrieves application information.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="AppInfo"/> class.
/// </remarks>
/// <param name="config">The configuration settings.</param>
/// <param name="logger">The logger instance.</param>
/// <param name="timeProvider">The time provider for retrieving current time.</param>
public class AppInfo(IConfiguration config, ILogger<AppInfo> logger, IDateTimeProvider timeProvider)
{
    private readonly IConfiguration config = config;
    private readonly ILogger<AppInfo> logger = logger;
    private readonly IDateTimeProvider timeProvider = timeProvider;

    /// <summary>
    /// Runs the HTTP-triggered function.
    /// </summary>
    /// <param name="req">The HTTP request.</param>
    /// <returns>The HTTP response.</returns>
    [OpenApiOperation(operationId: "AppInfo", tags: ["App"], Description = "Represents a function that retrieves application information.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK)]
    [Function("AppInfo")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "info")] HttpRequestData req)
    {
        this.logger.LogInformation($"{LoggingConstants.HskUiLogPrefix} App info function processed a request.");

        // Get the current UTC time
        DateTime utcNow = this.timeProvider.Now;
        var ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
        DateTime ukTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, ukTimeZone);

        // Retrieve values from configuration
        string? name = this.config["Application:AppName"] ?? "Unknown";
        string? version = this.config["Application:AppVersion"] ?? "Unknown";
        string? description = this.config["Application:AppDescription"] ?? "Unknown";

        var status = new
        {
            name,
            version,
            description,
            timestamp = ukTime,
        };

        await Task.CompletedTask.ConfigureAwait(false);

        var response = new OkObjectResult(status);

        var json = JsonSerializer.Serialize(
            response,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

        return req.CreateResponse(HttpStatusCode.InternalServerError, "application/json", json);
    }
}
