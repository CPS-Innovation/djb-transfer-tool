using Cps.Fct.Hk.Common.Contracts.Abstractions;
using Mapster;

namespace Cps.Fct.Hk.Common.Infrastructure.Abstractions.Implementations;

/// <inheritdoc cref="IModelMapper{TSource,TOutput}"/>
/// <typeparam name="TSource">Source object.</typeparam>
/// <typeparam name="TOutput">Destination object.</typeparam>
public class MapsterModelMapper<TSource, TOutput> : IModelMapper<TSource, TOutput>
    where TOutput : new()
{
    /// <inheritdoc/>
    public TOutput Map(TSource source)
    {
        return source == null ? new TOutput() : source.Adapt<TOutput>();
    }
}
