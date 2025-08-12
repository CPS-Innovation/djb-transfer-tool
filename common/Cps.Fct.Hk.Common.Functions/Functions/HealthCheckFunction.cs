namespace Cps.Fct.Hk.Common.Functions.Functions;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cps.Fct.Hk.Common.Functions.Extensions;
using Cps.Fct.Hk.Common.Infrastructure.Extensions;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

/// <summary>
/// A http function that checks the health status of the function app.
/// </summary>
public class HealthCheckFunction
{
    private readonly HealthCheckService _healthCheckService;

    /// <summary>
    /// Initializes a new instance of the <see cref="HealthCheckFunction"/> class.
    /// </summary>
    /// <param name="healthCheckService">Health check service instance.</param>
    public HealthCheckFunction(HealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }

    /// <summary>
    /// Runs the http health check function.
    /// </summary>
    /// <param name="req"></param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [Function(nameof(HealthCheckFunction))]
    public async Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health")] HttpRequestData req)
    {
        var healthReport = await _healthCheckService.CheckHealthAsync().ConfigureAwait(false);

        if (healthReport.Status == HealthStatus.Healthy)
        {
            return req.CreateResponse(HttpStatusCode.OK, "text/plain; charset=utf-8", "Healthy");
        }

        var healthCheckResponse = healthReport.ToResponse();

        var json = JsonSerializer.Serialize(
            healthCheckResponse,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

        return req.CreateResponse(HttpStatusCode.InternalServerError, "application/json", json);
    }
}
