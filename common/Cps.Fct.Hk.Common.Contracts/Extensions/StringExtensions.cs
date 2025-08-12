namespace Cps.Fct.Hk.Common.Contracts.Extensions;

/// <summary>
/// Provides extension methods for strings.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Splits strings with ; as the separator.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static IEnumerable<string> SplitValues(this string value)
    {
        const string separator = ";";

        return value
            .Split(separator, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToList();
    }

    /// <summary>Camel cases string.</summary>
    /// <param name="value"></param>
    /// <returns>Came cased string.</returns>
    public static string ToCamelCase(this string value)
    {
        if (string.IsNullOrEmpty(value) || value.Length < 2)
        {
#pragma warning disable CA1308 // Normalize strings to uppercase
            return value.ToLowerInvariant();
#pragma warning restore CA1308 // Normalize strings to uppercase
        }

        return char.ToLowerInvariant(value[0]) + value[1..];
    }
}
