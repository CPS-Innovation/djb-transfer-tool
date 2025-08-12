using Cps.Fct.Hk.Common.Contracts.Abstractions;

namespace Cps.Fct.Hk.Common.Contracts.Extensions;

/// <summary>
/// Mapper extension helpers.
/// </summary>
public static class ModelMapperExtensions
{
    /// <summary>
    /// Map from the source to an existing destination object.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <param name="mapper">The mapper that is being used.</param>
    /// <param name="source">The object to pull information from.</param>
    /// <param name="postMap">The action to perform on the output after mapping before it is returned.</param>
    /// <returns>The output with properties populated from source.</returns>
    public static TOutput Map<TSource, TOutput>(
        this IModelMapper<TSource, TOutput> mapper, TSource source, Action<TOutput> postMap)
    {
        var result = mapper.Map(source);
        postMap(result);
        return result;
    }

    /// <summary>
    /// Map multiple objects from the source to an existing destination object.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <param name="mapper">The mapper that is being used.</param>
    /// <param name="source">The list of objects to pull information from.</param>
    /// <param name="postMap">The action to perform on the output after mapping before it is returned.</param>
    /// <returns>The outputs with properties populated from source.</returns>
    public static TOutput[] MapArray<TSource, TOutput>(
        this IModelMapper<TSource, TOutput> mapper, IEnumerable<TSource> source, Action<TOutput> postMap)
    {
        return source
            .Select(x => mapper.Map(x, postMap))
            .ToArray();
    }
}
