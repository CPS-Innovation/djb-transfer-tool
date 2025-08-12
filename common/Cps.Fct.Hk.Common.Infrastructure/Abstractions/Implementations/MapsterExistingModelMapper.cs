using Cps.Fct.Hk.Common.Contracts.Abstractions;
using Mapster;

namespace Cps.Fct.Hk.Common.Infrastructure.Abstractions.Implementations;

/// <inheritdoc cref="IExistingModelMapper{TSource,TOutput}"/>
/// <typeparam name="TSource">Source object.</typeparam>
/// <typeparam name="TOutput">Destination object.</typeparam>
public class MapsterExistingModelMapper<TSource, TOutput> : IExistingModelMapper<TSource, TOutput>
{
    /// <inheritdoc/>
    public TOutput Map(TSource source, TOutput destination)
    {
        return source.Adapt(destination);
    }
}
