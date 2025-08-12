using System.Diagnostics.CodeAnalysis;
using Cps.Fct.Hk.Common.Contracts.BlobStorage;

namespace Cps.Fct.Hk.Common.Infrastructure.Extensions;

/// <summary>Extensions to help service provider.</summary>
[ExcludeFromCodeCoverage]
public static class ServiceProviderExtensions
{
    /// <summary>Create containers for blob storage service.</summary>
    /// <param name="serviceProvider"></param>
    /// <param name="blobStorageService">Service to create container for.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task<IServiceProvider> CreateContainersAsync(
        this IServiceProvider serviceProvider,
        IAzureBlobService<IAzureBlobStorageOptions> blobStorageService)
    {
        foreach (var container in blobStorageService.Options.Containers.Values)
        {
            await blobStorageService.EnsureContainerExistsAsync(container).ConfigureAwait(false);
        }

        return serviceProvider;
    }
}
