namespace Cps.Fct.Hk.Common.Contracts.Abstractions;

/// <summary>
/// Retrieve Environment Values.
/// </summary>
public interface IEnvironmentValuesProvider
{
    /// <inheritdoc cref="Environment.GetEnvironmentVariable(string)"/>
    string? GetEnvironmentVariable(string variable);
}
