namespace Cps.Fct.Hk.Common.Contracts.BlobStorage;

using System.Diagnostics.CodeAnalysis;

/// <summary>Interface allowing to interact with blobs in an Azure Blob Storage.</summary>
/// <typeparam name="TOptions">Options for configuring the blob storage.</typeparam>
public interface IAzureBlobService<out TOptions> : IAzureBlobService
    where TOptions : class, IAzureBlobStorageOptions
{
    /// <summary>Hack for DI registrations. Should find a better way like typed registrations a factory approach, etc.</summary>
    [ExcludeFromCodeCoverage]
    Type OptionsType => typeof(TOptions);

    /// <summary>Exposes the service options.</summary>
    TOptions Options { get; }
}
