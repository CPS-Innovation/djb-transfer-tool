namespace Cps.Fct.Hk.Common.Contracts.BlobStorage;

/// <summary>
/// Interface allowing to interact with blobs in an Azure Blob Storage.
/// </summary>
public interface IAzureBlobService
{
    /// <summary>
    /// Ensures a container exists, if not it creates it.
    /// </summary>
    /// <param name="container"></param>
    /// <returns></returns>
    Task EnsureContainerExistsAsync(string container);

    /// <summary>
    /// Uploads a blob to the specified container.
    /// </summary>
    /// <param name="container"></param>
    /// <param name="blobName"></param>
    /// <param name="blobContent"></param>
    /// <returns></returns>
    Task UploadAsync(string container, string blobName, string blobContent);

    /// <summary>
    /// Deletes a blob from the specified container.
    /// </summary>
    /// <param name="container"></param>
    /// <param name="blobName"></param>
    /// <returns></returns>
    Task DeleteAsync(string container, string blobName);

    /// <summary>
    /// Retrieves a blob from specified container.
    /// </summary>
    /// <param name="container"></param>
    /// <param name="blobName"></param>
    /// <returns></returns>
    Task<string> GetBlobAsync(string container, string blobName);
}
