namespace Cps.Fct.Hk.Common.Contracts.Abstractions;

/// <summary>
/// Repository for managing write operations on a data store.
/// </summary>
/// <typeparam name="TDataType">The data type stored.</typeparam>
public interface IWriteRepository<in TDataType>
{
    /// <summary>
    /// Stores a new item.
    /// </summary>
    /// <param name="instance">The item to store.</param>
    /// <returns>True if the item was created successfully.</returns>
    public Task<bool> CreateAsync(TDataType instance);

    /// <summary>
    /// Updates an existing item.
    /// </summary>
    /// <param name="instance">The updated item.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task UpdateAsync(TDataType instance);
}
