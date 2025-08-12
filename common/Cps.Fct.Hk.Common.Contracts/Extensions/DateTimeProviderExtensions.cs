using System.Globalization;
using Cps.Fct.Hk.Common.Contracts.Abstractions;

namespace Cps.Fct.Hk.Common.Contracts.Extensions;

/// <summary>
/// Extensions for dealing with DateTime objects.
/// </summary>
public static class DateTimeProviderExtensions
{
    /// <summary>
    /// Get DateTime.UtcNow formatted as yyyyMMdd_HHmmss.
    /// </summary>
    /// <param name="dateTimeProvider"></param>
    /// <returns></returns>
    public static string GetUtcNowTimestamp(this IDateTimeProvider dateTimeProvider)
    {
        return dateTimeProvider.Now.ToString(Defaults.TimestampFormat, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Parses a string to a date and if it can't then it throws an exception.
    /// </summary>
    /// <param name="dateTimeProvider"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Thrown when the value is not a valid date.</exception>
    public static DateTime ParseOrThrow(this IDateTimeProvider dateTimeProvider, string value)
    {
        _ = dateTimeProvider;
        var dateParseSuccess = DateTime.TryParse(value, out var dateTime);
        if (!dateParseSuccess)
        {
            throw new ArgumentException($"The date value of {value} could not be parsed to a datetime");
        }

        return dateTime;
    }
}
