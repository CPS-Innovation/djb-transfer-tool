using System.Globalization;

namespace Cps.Fct.Hk.Common.Contracts.Extensions;

/// <summary>Extensions for dealing with DateTime objects.</summary>
public static class DateTimeExtensions
{
    /// <summary>Get DateTime formatted as <see cref="Defaults.TimestampFormat"/>.</summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string GetTimestamp(this DateTime dateTime)
    {
        return dateTime.ToString(Defaults.TimestampFormat, CultureInfo.InvariantCulture);
    }

    /// <summary>Get DateTime formatted as <see cref="Defaults.TimestampFormatNoUnderscores"/>.</summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string GetTimestampWithoutUnderscores(this DateTime dateTime)
    {
        return dateTime.ToString(Defaults.TimestampFormatNoUnderscores, CultureInfo.InvariantCulture);
    }

    /// <summary>Converts a date to ISO 8601 string format.</summary>
    /// <param name="dateTime">Date to format.</param>
    /// <returns>Formatted date as string.</returns>
    public static string ToUniversalIso8601(this DateTime dateTime)
    {
        return dateTime.ToUniversalTime().ToString("u").Replace(" ", "T");
    }

    /// <summary>Parses a string to a date if it's not null.</summary>
    /// <param name="value">datetime string to parse.</param>
    /// <param name="format">Format of date in value.</param>
    /// <returns>Parsed datetime value or null.</returns>
    public static DateTime? ParseAsNullableDateTimeOrThrow(this string? value, string format)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return ParseDate(value, format);
    }

    /// <summary>Parses a string to a date and if it can't then it throws an exception.</summary>
    /// <param name="value"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static DateTime ParseAsDateTimeOrThrow(this string value, string format)
    {
        return ParseDate(value, format);
    }

    /// <summary>Parses a string to a date and if it can't then it throws an exception.</summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DateTime ParseAsDateTimeOrThrow(this string value)
    {
        return ParseDate(value, null);
    }

    /// <summary>
    /// Converts a DateTime to a long string representation including the date suffix.
    /// </summary>
    /// <param name="dateTime">The DateTime to convert.</param>
    /// <param name="includeYear">Whether or not to include the year in the string representation.</param>
    /// <returns>A long string representation of the DateTime including the date suffix.</returns>
    public static string ToLongDateStringWithDateSuffix(this DateTime dateTime, bool includeYear = true)
    {
        var date = dateTime.Day;
        var dateSuffix = date switch
        {
            1 or 21 or 31 => "st",
            2 or 22 => "nd",
            3 or 23 => "rd",
            _ => "th"
        };

        var dayOfWeek = dateTime.DayOfWeek.ToString();
        var month = dateTime.ToString("MMMM", CultureInfo.InvariantCulture);

        var longString = $"{dayOfWeek} {date}{dateSuffix} {month}";

        if (includeYear)
        {
            longString += $" {dateTime.Year}";
        }

        return longString;
    }

    private static DateTime ParseDate(string? value, string? format)
    {
        if (format != null && DateTime.TryParseExact(value, format, Defaults.Culture, DateTimeStyles.None, out var dateTimeFormat))
        {
            return dateTimeFormat;
        }

        if (format == null && DateTime.TryParse(value, Defaults.Culture, DateTimeStyles.None, out var dateTime))
        {
            return dateTime;
        }

        throw new ArgumentException($"The date value of {value} could not be parsed to a datetime");
    }
}
