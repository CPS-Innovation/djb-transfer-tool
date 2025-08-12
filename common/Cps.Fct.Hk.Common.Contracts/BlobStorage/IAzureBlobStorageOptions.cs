namespace Cps.Fct.Hk.Common.Contracts.BlobStorage;
/// <summary>Configuration options when working with <see cref="IAzureBlobService{TOptions}"/>.</summary>
public interface IAzureBlobStorageOptions
{
    /// <summary>Connection String to the storage account.</summary>
    public string ConnectionString { get; }

    /// <summary>Containers to be used.</summary>
    public Dictionary<string, string> Containers { get; init; }

    /// <summary>Settings section.</summary>
    public string SectionName { get; }

    /// <summary>Returns a container value by key.</summary>
    /// <param name="container."></param>
    /// <returns>Container name.</returns>
    public string GetContainer(string container);
}
