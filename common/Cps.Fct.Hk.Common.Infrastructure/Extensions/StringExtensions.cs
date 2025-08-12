using System.Globalization;

namespace Cps.Fct.Hk.Common.Infrastructure.Extensions;

/// <summary>
/// StringExtensions.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// ConvertToPascalCase.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string ConvertToPascalCase(this string input)
    {
        var info = CultureInfo.CurrentCulture.TextInfo;
        return info.ToTitleCase(input);
    }
}
