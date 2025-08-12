namespace Cps.Fct.Hk.Common.Infrastructure.Abstractions;

using System.Reflection;

/// <summary>
/// Provides methods for retrieving entry assembly.
/// </summary>
public static class StaticAssemblyProvider
{
    /// <summary>
    /// Function to retrieve the entry assembly.
    /// </summary>
    public static Func<Assembly> GetEntryAssemblyFunc { get; set; } = Assembly.GetExecutingAssembly;
}
