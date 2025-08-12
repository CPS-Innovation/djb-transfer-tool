namespace Cps.Fct.Hk.Common.Contracts.Extensions;

using System.Text.RegularExpressions;

/// <summary>Extension for System.IO.Path.</summary>
public static class PathExtensions
{
    private const short _regexTimeout = 5000;

    /// <summary>Join all files paths into a single string.</summary>
    /// <param name="filePaths">Collection of file paths to join.</param>
    /// <returns>Filepaths comma separated.</returns>
    public static string Join(IEnumerable<string> filePaths)
    {
        return string.Join(',', filePaths.Select(Path.GetFileName));
    }

    /// <summary>Determines if file path matches the provided pattern.</summary>
    /// <param name="filePath">Path of file to match against.</param>
    /// <param name="pattern">Regex pattern to match against.</param>
    /// <returns>atch is succesfful or not.</returns>
    public static bool MatchesPattern(string filePath, string pattern)
    {
        var filename = Path.GetFileName(filePath);
        var regex = new Regex(pattern, RegexOptions.None, TimeSpan.FromMilliseconds(_regexTimeout));
        var match = regex.IsMatch(filename);

        return match;
    }
}
