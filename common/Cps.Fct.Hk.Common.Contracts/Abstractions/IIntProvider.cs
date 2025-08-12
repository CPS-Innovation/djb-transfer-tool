namespace Cps.Fct.Hk.Common.Contracts.Abstractions;

/// <summary>
/// Retrieve Integer information.
/// </summary>
public interface IIntProvider
{
    /// <summary>
    /// Parses a string to an integer and if it can't then it throws an exception.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    int ParseOrThrow(string value);
}
