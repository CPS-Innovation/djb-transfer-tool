using System.Reflection;
using Cps.Fct.Hk.Common.Contracts.Abstractions;
using Cps.Fct.Hk.Common.Contracts.Models;

namespace Cps.Fct.Hk.Common.Infrastructure.Abstractions.Implementations;

/// <inheritdoc cref="IAssemblyInfoProvider"/>
public class AssemblyInfoProvider : IAssemblyInfoProvider
{
    private readonly Assembly _assembly;
    private readonly IEnvironmentValuesProvider _environmentValuesProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyInfoProvider"/> class.
    /// </summary>
    /// <param name="environmentValuesProvider"></param>
    /// <param name="assemblyProvider"></param>
    public AssemblyInfoProvider(
        IEnvironmentValuesProvider environmentValuesProvider,
        IAssemblyProvider assemblyProvider)
    {
        _environmentValuesProvider = environmentValuesProvider;
        _assembly = assemblyProvider.GetEntryAssembly();
    }

    /// <inheritdoc/>
    public string BuildNumber => ValueOrDefaultVersion(_environmentValuesProvider.GetEnvironmentVariable("BUILD_NUMBER"));

    /// <inheritdoc />
    public string FileVersion => ValueOrDefaultVersion(CustomAttributes<AssemblyFileVersionAttribute>()?.Version);

    /// <inheritdoc />
    public string InformationalVersion => ValueOrDefaultVersion(CustomAttributes<AssemblyInformationalVersionAttribute>()?.InformationalVersion);

    /// <inheritdoc />
    public string Version
    {
        get
        {
            var assemblyVersion = _assembly.GetName().Version;
            if (assemblyVersion == null)
            {
                return VersionInformation.DefaultVersion;
            }

            return $"{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}";
        }
    }

    private static string ValueOrDefaultVersion(string? value)
    {
        return value ?? VersionInformation.DefaultVersion;
    }

    private T? CustomAttributes<T>()
        where T : Attribute
    {
        var customAttributes = _assembly.GetCustomAttributes(typeof(T), false);
        if (customAttributes.Length > 0)
        {
            return (T)customAttributes[0];
        }

        return null;
    }
}
