using Cps.Fct.Hk.Common.Infrastructure.Abstractions.Implementations;

namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Abstractions;

public class GuidProviderTests
{
    [Fact]
    public void NewGuid_Value_HasValueWithCorrectFormat()
    {
        // Arrange
        var sut = new GuidProvider();

        // Act
        var result = sut.NewGuid;

        // Assert
        result.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public void NewGuid_Value_NewOneProvidedEachTime()
    {
        // Arrange
        var sut = new GuidProvider();

        // Act
        var value1 = sut.NewGuid;
        var value2 = sut.NewGuid;

        // Assert
        value1.ShouldNotBe(value2);
    }
}
