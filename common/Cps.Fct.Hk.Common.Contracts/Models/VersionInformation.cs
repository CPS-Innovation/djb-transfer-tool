namespace Cps.Fct.Hk.Common.Contracts.Models;

using Cps.Fct.Hk.Common.Contracts;

/// <summary>
/// Version information of an assembly.
/// </summary>
public class VersionInformation : ValueObject
{
    /// <summary>
    /// Default version.
    /// </summary>
    public const string DefaultVersion = "1.0.0";

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionInformation"/> class.
    /// </summary>
    /// <param name="buildNumber"></param>
    /// <param name="fileVersion"></param>
    /// <param name="informationalVersion"></param>
    /// <param name="version"></param>
    public VersionInformation(
        string buildNumber,
        string fileVersion,
        string informationalVersion,
        string version)
    {
        BuildNumber = buildNumber;
        FileVersion = fileVersion;
        InformationalVersion = informationalVersion;
        Version = version;
    }

    /// <summary>
    /// Retrieved from the BUILD_NUMBER environment variable.
    /// </summary>
    public string BuildNumber { get; }

    /// <summary>
    /// File Version.
    /// </summary>
    public string FileVersion { get; }

    /// <summary>
    /// Informational Version / Product Version.
    /// </summary>
    public string InformationalVersion { get; }

    /// <summary>
    /// Assembly Version.
    /// </summary>
    public string Version { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionInformation"/> class with the provided version.
    /// </summary>
    /// <param name="version"></param>
    /// <returns></returns>
    public static VersionInformation WithVersion(string version)
    {
        return new VersionInformation(version, version, version, version);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionInformation"/> class with default version.
    /// </summary>
    /// <returns>An instance of the <see cref="VersionInformation"/> class.</returns>
    public static VersionInformation Default()
    {
        return new VersionInformation(DefaultVersion, DefaultVersion, DefaultVersion, DefaultVersion);
    }

    /// <summary>
    /// Method to return properties for value comparing operations.
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return BuildNumber;
        yield return FileVersion;
        yield return InformationalVersion;
        yield return Version;
    }
}
