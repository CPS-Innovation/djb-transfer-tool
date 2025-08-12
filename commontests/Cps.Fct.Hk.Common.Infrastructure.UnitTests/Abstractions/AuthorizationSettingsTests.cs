using Cps.Fct.Hk.Common.Infrastructure.Abstractions.Implementations;

namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Abstractions;

public class AuthorizationSettingsTests
{
    [Fact]
    public void Authorize_ShouldBeTrue()
    {
        // Arrange & Act
        var sut = new AuthorizationSettings();

        // Assert
        sut.Authorize.ShouldBeTrue();
    }
}
