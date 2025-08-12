namespace Cps.Fct.Hk.Common.Contracts.HealthChecks;

/// <summary>
/// Specific health check details.
/// </summary>
public class HealthCheckResponseEntry
{
    /// <summary>
    /// The name of the service.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Description of the service.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Time that the health check took to execute.
    /// </summary>
    public string? DurationSeconds { get; set; }

    /// <summary>
    /// Status of the service.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Exception details if raised.
    /// </summary>
    public string? Exception { get; set; }

    /// <summary>
    /// Further untyped data.
    /// </summary>
    public IReadOnlyDictionary<string, object>? Data { get; set; }
}
