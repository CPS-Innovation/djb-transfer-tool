using Cps.Fct.Hk.Common.Infrastructure.Abstractions.Implementations;

namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Abstractions;

public class DateTimeProviderTests
{
    [Fact]
    public void Now_WhenCalled_ReturnsUtcNow()
    {
        // Arrange
        var sut = new DateTimeProvider();

        // Act
        var utcNow = DateTime.UtcNow;
        var result = sut.Now;

        // Assert
        (utcNow.AddSeconds(-2) < result && result < utcNow.AddSeconds(2)).ShouldBeTrue();
    }
}
