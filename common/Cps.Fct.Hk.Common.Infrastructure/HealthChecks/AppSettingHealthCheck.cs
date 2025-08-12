using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Cps.Fct.Hk.Common.Infrastructure.HealthChecks;

/// <summary>
/// A health check that checks if an app setting is present.
/// </summary>
public class AppSettingHealthCheck : IHealthCheck
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppSettingHealthCheck"/> class.
    /// </summary>
    /// <param name="configuration">IConfiguration instance to load app setting.</param>
    public AppSettingHealthCheck(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <inheritdoc/>
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        HealthCheckResult result;
        var name = context.Registration.Name;
        var value = _configuration[name];

        if (string.IsNullOrWhiteSpace(value))
        {
            var errorMessage = $"Error occurred validating app setting {name} as it is null or empty";
            result = HealthCheckResult.Unhealthy(errorMessage);
        }
        else
        {
            var successMessage = $"Successfully validated app setting {name}";
            result = HealthCheckResult.Healthy(successMessage);
        }

        return Task.FromResult(result);
    }
}
