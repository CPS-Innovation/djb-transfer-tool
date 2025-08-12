namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Abstractions;
using Cps.Fct.Hk.Common.Infrastructure.Abstractions.Implementations;

public class EnvironmentValuesProviderTests
{
    private readonly IEnvironmentValuesProvider _provider;

    public EnvironmentValuesProviderTests()
    {
        _provider = new EnvironmentValuesProvider();
    }

    [Fact]
    public void EnvironmentValuesProvider_NotSet()
    {
        // Act
        var result = _provider.GetEnvironmentVariable("TestUnknown");

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public void EnvironmentValuesProvider_Set()
    {
        // Arrange
        const string key = "TestSet";
        const string value = "value1";
        Environment.SetEnvironmentVariable(key, value);

        // Act
        var result = _provider.GetEnvironmentVariable(key);

        // Assert
        result.ShouldBe(value);
    }
}
