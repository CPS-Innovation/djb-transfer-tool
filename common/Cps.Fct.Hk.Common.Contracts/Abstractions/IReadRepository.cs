using Cps.Fct.Hk.Common.Contracts.Exceptions;

namespace Cps.Fct.Hk.Common.Contracts.Abstractions;

/// <summary>
/// Repository for managing read operations on a data store.
/// </summary>
/// <typeparam name="TDataType">The data type stored.</typeparam>
public interface IReadRepository<TDataType>
{
    /// <summary>
    /// Gets the item associated with the Id.
    /// </summary>
    /// <param name="instanceId">The unique identifier of the instance.</param>
    /// <returns>The basket.</returns>
    /// <exception cref="NotFoundException">Thrown when no item found matching the identifier.</exception>
    Task<TDataType> GetAsync(string instanceId);
}
