using System.Globalization;

namespace Cps.Fct.Hk.Common.Contracts;

/// <summary>
/// Provides default settings for Bogus Faker.
/// </summary>
public static class Defaults
{
    /// <summary>
    /// Setting locale to GB.
    /// </summary>
    public const string FakerLocale = "en_GB";

    /// <summary>
    /// Default culture for dealing with globalisation.
    /// </summary>
    public static CultureInfo Culture { get; } = CultureInfo.GetCultureInfo("en-GB");

    /// <summary>
    /// Timestamp format.
    /// </summary>
    public static string TimestampFormat { get; } = "yyyyMMdd_HHmmssfff";

    /// <summary>
    /// Timestamp format without underscores.
    /// </summary>
    public static string TimestampFormatNoUnderscores { get; } = "yyyyMMddHHmmssfff";
}
