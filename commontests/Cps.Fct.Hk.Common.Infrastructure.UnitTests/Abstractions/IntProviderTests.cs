using Cps.Fct.Hk.Common.Infrastructure.Abstractions.Implementations;

namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Abstractions;

public class IntProviderTests
{
    [Fact]
    public void ParseOrThrow_WithValidString_IsParsed()
    {
        // Arrange
        var sut = new IntProvider();
        const string number = "32";

        // Act
        var result = sut.ParseOrThrow(number);

        // Assert
        result.ShouldBe(32);
    }

    [Fact]
    public void ParseOrThrow_WithInvalidString_ThrowsException()
    {
        // Arrange
        var sut = new IntProvider();
        const string number = "078sadf";

        // Act
        var act = () => sut.ParseOrThrow(number);

        // Assert
        var exception = Assert.Throws<ArgumentException>(() => act());
        exception.Message.ShouldBe("The integer value of 078sadf could not be parsed to an integer");
    }
}
