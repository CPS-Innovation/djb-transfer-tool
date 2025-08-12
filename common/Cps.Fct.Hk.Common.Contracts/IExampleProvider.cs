namespace Cps.Fct.Hk.Common.Contracts;

/// <summary>
/// Classes that can provide a fully populated example of themselves.
/// </summary>
/// <typeparam name="T">The type of class that is generated.</typeparam>
public interface IExampleProvider<out T>
{
    /// <summary>
    /// Generate a fully populated object with valid example data.
    /// </summary>
    /// <returns></returns>
    T Generate();
}
