namespace Cps.Fct.Hk.Common.Contracts.Extensions;

/// <summary>EnumerableExtensions.</summary>
public static class EnumerableExtensions
{
    /// <summary>Converts string list to csv.</summary>
    /// <param name="csvLines">CSV lines to convert.</param>
    /// <param name="separator">Separator to use.</param>
    /// <returns>Csv string.</returns>
    public static string ToCsv(this IEnumerable<string?[]> csvLines, string separator)
    {
        if (!csvLines.Any())
        {
            return string.Empty;
        }

        var lineEnding = Environment.NewLine;
        var csvSeparatedLines = csvLines.Select(l => string.Join(separator, l));
        var csvContent = $"{string.Join(lineEnding, csvSeparatedLines)}{lineEnding}";
        return csvContent;
    }

    /// <summary>Iterates through an iEnumerable.</summary>
    /// <typeparam name="T">Type of items in iEnumerable.</typeparam>
    /// <param name="items"></param>
    /// <param name="action"></param>
    public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
    {
        foreach (var item in items)
        {
            action(item);
        }
    }
}
