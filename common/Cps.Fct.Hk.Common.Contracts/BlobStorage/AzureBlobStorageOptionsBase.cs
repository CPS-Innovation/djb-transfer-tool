using System.ComponentModel.DataAnnotations;

namespace Cps.Fct.Hk.Common.Contracts.BlobStorage;

/// <summary>Base class configuration options when working with <see cref="IAzureBlobService{TOptions}"/>.</summary>
public class AzureBlobStorageOptionsBase : IAzureBlobStorageOptions
{
    /// <inheritdoc/>
    [Required]
    public string ConnectionString { get; init; } = string.Empty;

    /// <inheritdoc/>
    public Dictionary<string, string> Containers { get; init; } = new();

    /// <inheritdoc/>
    public virtual string SectionName { get; } = string.Empty;

    /// <summary>Success container name.</summary>
    public string SuccessContainer => GetContainer("Container");   

    /// <inheritdoc/>
    public string GetContainer(string container)
    {
        return Containers.TryGetValue(container, out var containerValue) ? containerValue : string.Empty;
    }
}
