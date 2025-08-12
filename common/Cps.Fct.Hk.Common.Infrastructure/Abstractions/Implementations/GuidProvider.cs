using Cps.Fct.Hk.Common.Contracts.Abstractions;

namespace Cps.Fct.Hk.Common.Infrastructure.Abstractions.Implementations;

/// <inheritdoc cref="IGuidProvider"/>
public class GuidProvider : IGuidProvider
{
    /// <inheritdoc/>
    public Guid NewGuid => Guid.NewGuid();
}
