using Cps.Fct.Hk.Common.Contracts.Configuration;

namespace Cps.Fct.Hk.Common.Contracts.Logging;

/// <summary>
/// CorrelationId configuration.
/// </summary>
public class CorrelationIdOptions : IConfigurationOptions
{
    private const string DefaultHeader = "correlationid";

    /// <summary>
    /// The header field name where the correlation ID will be stored.
    /// </summary>
    public string Header { get; init; } = DefaultHeader;

    /// <summary>
    /// Controls whether the correlation ID is returned in the response headers.
    /// </summary>
    public bool IncludeInResponse { get; init; } = true;

    /// <inheritdoc/>
    public string? SectionName => string.Empty;
}
