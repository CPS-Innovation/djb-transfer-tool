using Cps.Fct.Hk.Common.Contracts.Models;

namespace Cps.Fct.Hk.Common.Contracts.Abstractions;

/// <summary>
/// Returns version information about the Assembly.
/// </summary>
public interface IAssemblyInfoProvider
{
    /// <summary>
    /// Assembly Build Number.
    /// </summary>
    public string BuildNumber { get; }

    /// <summary>
    /// Version given within a file system.
    /// </summary>
    public string FileVersion { get; }

    /// <summary>
    /// The Product version of the assembly.
    /// </summary>
    public string InformationalVersion { get; }

    /// <summary>
    /// Version used by the framework at build and runtime.
    /// </summary>
    public string Version { get; }

    /// <summary>
    /// Map values to new <see cref="VersionInformation"/>.
    /// </summary>
    public VersionInformation VersionInfo => new(
        BuildNumber,
        FileVersion,
        InformationalVersion,
        Version);
}
