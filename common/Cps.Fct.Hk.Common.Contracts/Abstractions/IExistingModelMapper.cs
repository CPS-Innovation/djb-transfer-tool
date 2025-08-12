namespace Cps.Fct.Hk.Common.Contracts.Abstractions;

/// <summary>
/// Mapper used for mapping from a source object to properties in an existing object.
/// </summary>
/// <typeparam name="TSource">The type of the input.</typeparam>
/// <typeparam name="TOutput">The type of the output.</typeparam>
public interface IExistingModelMapper<in TSource, TOutput>
{
    /// <summary>
    /// Map from the source to an existing destination object.
    /// </summary>
    /// <param name="source">The object to pull information from.</param>
    /// <param name="destination">The object to add information to.</param>
    /// <returns>The destination with properties populated from source.</returns>
    TOutput Map(TSource source, TOutput destination);
}
