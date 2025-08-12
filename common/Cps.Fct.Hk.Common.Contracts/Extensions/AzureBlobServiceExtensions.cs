using System.Text.Json;
using Cps.Fct.Hk.Common.Contracts.BlobStorage;
using Microsoft.Extensions.Logging;

namespace Cps.Fct.Hk.Common.Contracts.Extensions;

/// <summary>Provides extensions for the Azure Blob Service interface.</summary>
public static class AzureBlobServiceExtensions
{
    /// <summary>Convert a blob to json before uploading.</summary>
    /// <typeparam name="T">Type of object to serialise.</typeparam>
    /// <param name="azureBlobService"></param>
    /// <param name="container"></param>
    /// <param name="blobName"></param>
    /// <param name="blobObject"></param>
    /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task UploadAsJsonAsync<T>(this IAzureBlobService azureBlobService, string container, string blobName, T blobObject)
    {
        await azureBlobService.UploadAsync(container, blobName, JsonSerializer.Serialize(blobObject)).ConfigureAwait(false);
    }

    /// <summary>
    /// Uploads blob content to storage container.
    /// </summary>
    /// <typeparam name="T">Type of logger to log against.</typeparam>
    /// <param name="azureBlobService"></param>
    /// <param name="fileName"></param>
    /// <param name="fileContent"></param>
    /// <param name="containerType"></param>
    /// <param name="logger"></param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    public static async Task UploadBlobAsync<T>(
        this IAzureBlobService<IAzureBlobStorageOptions> azureBlobService,
        string fileName,
        string fileContent,
        string containerType,
        ILogger<T> logger)
    {
        var container = azureBlobService.Options.GetContainer(containerType.ToString());
        logger.LogInformation($"Sending {fileName} to blob storage service {container} container.");
        await azureBlobService.UploadAsync(container, fileName, fileContent).ConfigureAwait(false);
    }
}
