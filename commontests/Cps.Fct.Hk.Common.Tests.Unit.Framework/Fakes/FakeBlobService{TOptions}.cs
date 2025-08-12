// <copyright file="FakeBlobService{TOptions}.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.Tests.Unit.Framework.Fakes;
using Cps.Fct.Hk.Common.Contracts.BlobStorage;

public class FakeBlobService<TOptions> : IAzureBlobService<TOptions>
    where TOptions : class, IAzureBlobStorageOptions
{
    private readonly ILogger<FakeBlobService<TOptions>> logger;

    private readonly Dictionary<string, Dictionary<string, string>> store = new();

    public FakeBlobService(
        ILogger<FakeBlobService<TOptions>> logger,
        IOptions<TOptions> options)
    {
        this.logger = logger;
        Options = options.Value;
    }

    public TOptions Options { get; }

    public Task EnsureContainerExistsAsync(string container)
    {
        store.TryAdd(container, new());

        return Task.CompletedTask;
    }

    // Shouldn't be in this class
    public List<string> GetContainerBlobs(string containerName)
    {
        var blobs = GetContainerBlobNames(containerName);
        return blobs.ToList();
    }

    /// <inheritdoc/>
    public Task UploadAsync(string container, string blobName, string blobContent)
    {
        this.logger.LogInformation("Beginning uploading {BlobName} to {Container}", blobName, container);

        this.store.TryAdd(container, new());
        var containerStore = this.store[container];

        if (!containerStore.TryAdd(blobName, blobContent))
        {
            throw new InvalidOperationException($"{blobName} already exists in {container}");
        }

        logger.LogInformation("Completed uploading {BlobName} to {Container}", blobName, container);

        return Task.CompletedTask;
    }

    public async Task UploadAsJsonAsync<T>(string container, string blobName, T blobObject)
         where T : class
    {
        await this.UploadAsync(container, blobName, JsonSerializer.Serialize(blobObject)).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public Task DeleteAsync(string container, string blobName)
    {
        logger.LogInformation("Beginning deleting {BlobName} from {Container}", blobName, container);

        if (!store.TryGetValue(container, out var containerStore))
        {
            throw new InvalidOperationException($"Invalid {container}");
        }

        if (!containerStore.ContainsKey(blobName))
        {
            throw new FileNotFoundException($"Invalid {blobName}");
        }

        containerStore.Remove(blobName);

        logger.LogInformation("Completed deleting {BlobName} from {Container}", blobName, container);

        return Task.CompletedTask;
    }

    public Task<string> GetBlobAsync(string container, string blobName)
    {
        logger.LogInformation("Beginning get {BlobName} from {Container}", blobName, container);

        if (!store.TryGetValue(container, out var containerStore))
        {
            throw new InvalidOperationException($"Invalid {container}");
        }

        if (!containerStore.TryGetValue(blobName, out var blobNameStore))
        {
            throw new FileNotFoundException($"Invalid {blobName}");
        }

        logger.LogInformation("Found {BlobName} from {Container}", blobNameStore, container);

        return Task.FromResult(blobNameStore);
    }

    private IEnumerable<string> GetContainerBlobNames(string container)
    {
        if (!store.TryGetValue(container, out var containerStore))
        {
            return Enumerable.Empty<string>();
        }

        return containerStore.Keys;
    }
}
