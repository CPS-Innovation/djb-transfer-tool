// <copyright file="BlobStorageDataManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.Tests.Unit.Framework.Managers;

using Cps.Fct.Hk.Common.Contracts.BlobStorage;
using Cps.Fct.Hk.Common.Tests.Unit.Framework.Fakes;
using Cps.Fct.Hk.Common.Tests.Unit.Framework.Managers.Contracts;

public class BlobStorageDataManager<TOptions> : IBlobStorageDataManager
    where TOptions : class, IAzureBlobStorageOptions
{
    private readonly FakeBlobService<TOptions> blobStorage;

    public BlobStorageDataManager(IAzureBlobService<TOptions> blobStorage)
    {
        this.blobStorage = blobStorage as FakeBlobService<TOptions>
            ?? throw new ArgumentException(
                $"Not of type FakeBlobService{typeof(TOptions)}", nameof(blobStorage));
    }

    public async Task CreateBlob(string container, string blobName, string blobContent)
    {
        await blobStorage.UploadAsync(container, blobName, blobContent).ConfigureAwait(false);
    }

    public bool BlobExists(string containerName, string partialBlobName)
    {
        var blobs = blobStorage.GetContainerBlobs(containerName);
        var matchingBlobs = blobs.Where(b => b.Contains(partialBlobName));

        var fileCount = matchingBlobs.Count();

        if (fileCount > 1)
        {
            throw new InvalidOperationException($"Blobs matching {containerName}/{partialBlobName} were found more than once");
        }

        return fileCount == 1;
    }

    public int GetBlobCount(string containerName)
    {
        var blobs = blobStorage.GetContainerBlobs(containerName);
        return blobs.Count;
    }
}
