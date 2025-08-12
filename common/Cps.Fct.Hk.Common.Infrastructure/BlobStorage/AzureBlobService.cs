using System.Diagnostics.CodeAnalysis;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Cps.Fct.Hk.Common.Contracts.BlobStorage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cps.Fct.Hk.Common.Infrastructure.BlobStorage;

/// <summary>
/// Generic service that allows the interaction with an Azure Blob Storage account.
/// </summary>
/// <typeparam name="TOptions">Configuration options.</typeparam>
[ExcludeFromCodeCoverage]
public class AzureBlobService<TOptions> : IAzureBlobService<TOptions>
    where TOptions : class, IAzureBlobStorageOptions
{
    private readonly ILogger<AzureBlobService<TOptions>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureBlobService{TOptions}"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public AzureBlobService(
        ILogger<AzureBlobService<TOptions>> logger,
        IOptions<TOptions> options)
    {
        _logger = logger;
        Options = options.Value;
    }

    /// <inheritdoc/>
    public TOptions Options { get; }

    /// <inheritdoc/>
    public async Task EnsureContainerExistsAsync(string container)
    {
        _logger.LogInformation("Beginning check {Container} exists", container);

        var blobServiceClient = CreateBlobServiceClient();
        _ = await CreateBlobContainerClientAsync(container, blobServiceClient).ConfigureAwait(false);

        _logger.LogInformation("Completed check {Container} exists", container);
    }

    /// <inheritdoc/>
    public async Task UploadAsync(string container, string blobName, string blobContent)
    {
        _logger.LogInformation("Beginning uploading {BlobName} to {Container}", blobName, container);

        var blobServiceClient = CreateBlobServiceClient();
        var containerClient = await CreateBlobContainerClientAsync(container, blobServiceClient).ConfigureAwait(false);
        await UploadAsBase64Async(blobName, blobContent, containerClient).ConfigureAwait(false);

        _logger.LogInformation("Completed uploading {BlobName} to {Container}", blobName, container);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(string container, string blobName)
    {
        _logger.LogInformation("Beginning deleting {BlobName} from {Container}", blobName, container);

        var blobServiceClient = CreateBlobServiceClient();
        var containerClient = await CreateBlobContainerClientAsync(container, blobServiceClient).ConfigureAwait(false);
        await containerClient.DeleteBlobAsync(blobName).ConfigureAwait(false);

        _logger.LogInformation("Completed deleting {BlobName} from {Container}", blobName, container);
    }

    /// <summary>
    /// Reads a blob file from Azure Blob Container.
    /// </summary>
    /// <param name="container"></param>
    /// <param name="blobName"></param>
    /// <exception cref="Exception">Thrown error reading blob file.</exception>
    /// <returns>blob file content.</returns>
    public async Task<string> GetBlobAsync(string container, string blobName)
    {
        _logger.LogInformation("Beginning reading {BlobName} from {Container}", blobName, container);

        var blobServiceClient = CreateBlobServiceClient();
        try
        {
            var containerClient = await CreateBlobContainerClientAsync(container, blobServiceClient).ConfigureAwait(false);
            return await GetBlobAsync(blobName, containerClient).ConfigureAwait(false);
        }
        catch
        {
            _logger.LogError($"Error Retrieving file {blobName} from {container}");
            throw;
        }
    }

    private static async Task<BlobContainerClient> CreateBlobContainerClientAsync(
        string container, BlobServiceClient blobServiceClient)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(container);
        await containerClient.CreateIfNotExistsAsync().ConfigureAwait(false);
        return containerClient;
    }

    private static async Task UploadAsBase64Async(
        string blobName, string blobContent, BlobContainerClient containerClient)
    {
        var fileContentBytes = Convert.FromBase64String(blobContent);
        using var memoryStream = new MemoryStream(fileContentBytes);
        await containerClient.UploadBlobAsync(blobName, memoryStream).ConfigureAwait(false);
    }

    private static async Task<string> GetBlobAsync(string blobName, BlobContainerClient containerClient)
    {
        var blobClient = containerClient.GetBlobClient(blobName);
        BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync().ConfigureAwait(false);
        return downloadResult.Content.ToString();
    }

    private BlobServiceClient CreateBlobServiceClient()
    {
        return new BlobServiceClient(Options.ConnectionString);
    }
}
