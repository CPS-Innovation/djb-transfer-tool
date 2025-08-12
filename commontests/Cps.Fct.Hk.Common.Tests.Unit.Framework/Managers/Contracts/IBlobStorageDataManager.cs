// <copyright file="IBlobStorageDataManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.Tests.Unit.Framework.Managers.Contracts;

public interface IBlobStorageDataManager
{
    bool BlobExists(string containerName, string partialBlobName);

    int GetBlobCount(string containerName);

    Task CreateBlob(string container, string blobName, string blobContent);
}
