using Cps.Fct.Hk.Common.Infrastructure.Abstractions.Implementations;

namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Abstractions;

public class EnumProviderTests
{
    private enum Days
    {
        Monday,
        Tuesday
    }

    [Fact]
    public void ParseOrThrow_WithValidString_IsParsed()
    {
        // Arrange
        var sut = new EnumProvider();
        const string day = "Tuesday";

        // Act
        var result = sut.ParseOrThrow<Days>(day);

        // Assert
        result.ShouldBe(Days.Tuesday);
    }

    [Fact]
    public void ParseOrThrow_WithInvalidString_ThrowsException()
    {
        // Arrange
        var sut = new EnumProvider();
        const string day = "lknjzxlciuv";

        // Act
        var act = () => sut.ParseOrThrow<Days>(day);

        // Assert
        var exception = Assert.Throws<ArgumentException>(() => act());
        exception.Message.ShouldBe("The enum value of lknjzxlciuv could not be parsed to an enum");
    }
}
