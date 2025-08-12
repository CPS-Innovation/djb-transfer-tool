using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Cps.Fct.Hk.Common.Contracts.Abstractions;

namespace Cps.Fct.Hk.Common.Infrastructure.Abstractions.Implementations;

/// <inheritdoc cref="IAssemblyProvider"/>
[ExcludeFromCodeCoverage]
public class AssemblyProvider : IAssemblyProvider
{
    /// <inheritdoc/>
    public Assembly GetEntryAssembly()
    {
        // Returns the process executable. Keep in mind that this may not be your executable.
        var assembly = Assembly.GetEntryAssembly();

        return
            assembly ??
            throw new InvalidOperationException("Could not retrieve Entry Assembly");
    }
}
