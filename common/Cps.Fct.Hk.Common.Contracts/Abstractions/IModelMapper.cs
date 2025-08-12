namespace Cps.Fct.Hk.Common.Contracts.Abstractions;

/// <summary>
/// Mapper used for mapping from a source object to properties in a new object.
/// </summary>
/// <typeparam name="TSource">The type of the input.</typeparam>
/// <typeparam name="TOutput">The type of the output.</typeparam>
public interface IModelMapper<in TSource, out TOutput>
{
    /// <summary>
    /// Map from the source to a new destination object.
    /// </summary>
    /// <param name="source">The object to pull information from.</param>
    /// <returns>The new object with properties populated from source.</returns>
    TOutput Map(TSource source);
}
