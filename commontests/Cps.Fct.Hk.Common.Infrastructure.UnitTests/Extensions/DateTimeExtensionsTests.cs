namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Extensions;

using System.Globalization;
using Cps.Fct.Hk.Common.Infrastructure.Abstractions.Implementations;
using Cps.Fct.Hk.Common.Tests.Unit.Framework.Extensions;

public class DateTimeExtensionsTests
{
    private const string UkTimeFormat = "dd/MM/yyyy HH:mm:ss";

    [Fact]
    public void GetTimestampWithoutUnderscores_Value_HasWithCorrectFormat()
    {
        // Arrange
        var sut = new DateTimeProvider();

        // Act
        var result = sut.Now.GetTimestampWithoutUnderscores();
        var resultDateTime = DateTime.ParseExact(result, "yyyyMMddHHmmssfff", null);
        var dateTimeTwoMinutesAgo = DateTime.UtcNow.AddMinutes(-2);

        // Assert
        resultDateTime.ShouldBeGreaterThan(dateTimeTwoMinutesAgo);
    }

    [Fact]
    public async Task GetTimestampWithoutUnderscores_Value_NewOneProvidedEachTime()
    {
        // Arrange
        var sut = new DateTimeProvider();

        // Act
        var value1 = sut.Now.GetTimestampWithoutUnderscores();
        await Task.Delay(100);
        var value2 = sut.Now.GetTimestampWithoutUnderscores();

        // Assert
        value1.ShouldNotBe(value2);
    }

    [Fact]
    public void GetTimestamp_Value_HasWithCorrectFormat()
    {
        // Arrange
        var sut = new DateTimeProvider();

        // Act
        var result = sut.Now.GetTimestamp();
        var resultDateTime = DateTime.ParseExact(result, "yyyyMMdd_HHmmssfff", null);
        var dateTimeTwoMinutesAgo = DateTime.UtcNow.AddMinutes(-2);

        // Assert
        resultDateTime.ShouldBeGreaterThan(dateTimeTwoMinutesAgo);
    }

    [Fact]
    public async Task GetTimestamp_Value_NewOneProvidedEachTime()
    {
        // Arrange
        var sut = new DateTimeProvider();

        // Act
        var value1 = sut.Now.GetTimestamp();
        await Task.Delay(100);
        var value2 = sut.Now.GetTimestamp();

        // Assert
        value1.ShouldNotBe(value2);
    }

    [Fact]
    public void ToUniversalIso8601_GivenValue_FormatsCorrectly()
    {
        // Arrange
        var dateTime = new DateTime(2023, 2, 3, 4, 5, 6, DateTimeKind.Local);

        // Act
        var result = dateTime.ToUniversalIso8601();

        // Assert
        result.ShouldNotBe("2023-02-03T14:05:06Z");
    }

    [Theory]
    [InlineData("2022-12-13 11:12:10", "13/12/2022 11:12:10", false)]
    [InlineData("adslkjfjaoisdu", "", true)]
    public void ParseAsDateTimeOrThrow_GivenValue_ParsesOrThrows(string dateTime, string expectedDateTime, bool shouldThrow)
    {
        // Act
        var act = () => dateTime.ParseAsDateTimeOrThrow();

        // Assert
        if (!shouldThrow)
        {
            act().ToString(UkTimeFormat, CultureInfo.InvariantCulture).ShouldBe(expectedDateTime);
            return;
        }

        act.ShouldThrowWithMessage<DateTime, ArgumentException>($"The date value of {dateTime} could not be parsed to a datetime");
    }

    [Theory]
    [InlineData("2022-12-13", "yyyy-MM-dd", "13/12/2022 00:00:00", false)]
    [InlineData("adslkjfjaoisdu", "", null, true)]
    public void ParseAsDateTimeOrThrowWithFormat_GivenValue_ParsesOrThrows(string dateTime, string format, string? expectedDateTime, bool shouldThrow)
    {
        // Act
        var act = () => dateTime.ParseAsDateTimeOrThrow(format);

        // Assert
        if (!shouldThrow)
        {
            act().ToString(UkTimeFormat, CultureInfo.InvariantCulture).ShouldBe(expectedDateTime);
            return;
        }

        act.ShouldThrowWithMessage<DateTime, ArgumentException>($"The date value of {dateTime} could not be parsed to a datetime");
    }

    [Theory]
    [InlineData("2022-12-13", "yyyy-MM-dd", "13/12/2022 00:00:00", false)]
    [InlineData(null, "", "", false)]
    [InlineData("", "", "", false)]
    [InlineData("adslkjfjaoisdu", "", null, true)]
    public void ParseAsNullableDateTimeOrThrowWithFormat_GivenValue_ParsesOrThrows(string? dateTime, string format, string? expectedDateTime, bool shouldThrow)
    {
        // Act
        var act = () => dateTime.ParseAsNullableDateTimeOrThrow(format);

        // Assert
        if (!shouldThrow)
        {
            act()?.ToString(UkTimeFormat, CultureInfo.InvariantCulture).ShouldBe(expectedDateTime);
            return;
        }

        act.ShouldThrowWithMessage<DateTime?, ArgumentException>($"The date value of {dateTime} could not be parsed to a datetime");
    }
}
