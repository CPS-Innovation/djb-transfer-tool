namespace Cps.Fct.Hk.Common.Contracts.Abstractions;

using System.Reflection;

/// <summary>
/// Retrieve assemblies.
/// </summary>
public interface IAssemblyProvider
{
    /// <summary>
    /// Gets the process executable in the default application domain. 
    /// </summary>
    /// <returns></returns>
    Assembly GetEntryAssembly();
}
