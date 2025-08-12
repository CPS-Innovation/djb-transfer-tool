using Cps.Fct.Hk.Common.Infrastructure.UnitTests.Models;

namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Extensions;

public class TypeExtensionsTests
{
    [Fact]
    public void IsSimple_TypeIsSimple_ReturnsTrue()
    {
        // Arrange
        var type = typeof(string);

        // Act
        var result = type.IsSimple();

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void IsSimple_TypeIsComplex_ReturnsFalse()
    {
        // Arrange
        var type = typeof(DummyRequest);

        // Act
        var result = type.IsSimple();

        // Assert
        result.ShouldBeFalse();
    }
}
