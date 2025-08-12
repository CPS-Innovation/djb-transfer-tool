namespace Cps.Fct.Hk.Common.Contracts.Cors;

/// <summary>
/// Cors Configuration.
/// </summary>
public class CorsPolicy
{
    /// <summary>
    /// Allowed Origins.
    /// </summary>
    public string AllowedOrigins { get; set; } = "*";

    /// <summary>
    /// Exposed Headers.
    /// </summary>
    public string ExposedHeaders { get; set; } = "location";

    /// <summary>
    /// Retrieve Allowed Origins.
    /// </summary>
    /// <returns>An array of allowed origins.</returns>
    public string[] GetOrigins()
    {
        return AllowedOrigins.Split(";");
    }

    /// <summary>
    /// Retrieve Exposed Headers.
    /// </summary>
    /// <returns>An array of exposed headers.</returns>
    public string[] GetHeaders()
    {
        return ExposedHeaders.Split(";");
    }
}
