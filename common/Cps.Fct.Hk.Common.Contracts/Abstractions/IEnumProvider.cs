namespace Cps.Fct.Hk.Common.Contracts.Abstractions;

/// <summary>
/// Retrieve Enum information.
/// </summary>
public interface IEnumProvider
{
    /// <summary>
    /// Parses a string to an enum and if it can't then it throws an exception.
    /// </summary>
    /// <typeparam name="T">Enum type.</typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    T ParseOrThrow<T>(string value)
        where T : struct, Enum;
}
