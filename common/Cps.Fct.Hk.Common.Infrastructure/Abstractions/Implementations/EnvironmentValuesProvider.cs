using Cps.Fct.Hk.Common.Contracts.Abstractions;

namespace Cps.Fct.Hk.Common.Infrastructure.Abstractions.Implementations;

/// <inheritdoc cref="IEnvironmentValuesProvider"/>
public class EnvironmentValuesProvider : IEnvironmentValuesProvider
{
    /// <inheritdoc/>
    public string? GetEnvironmentVariable(string variable)
    {
        return Environment.GetEnvironmentVariable(variable);
    }
}
