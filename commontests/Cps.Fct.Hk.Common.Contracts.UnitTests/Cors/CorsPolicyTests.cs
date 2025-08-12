using Cps.Fct.Hk.Common.Contracts.Cors;

namespace Cps.Fct.Hk.Common.Contracts.UnitTests.Cors;

public class CorsPolicyTests
{
    [Fact]
    public void AllowedOrigins_DefaultValue_IsExpectedValue()
    {
        // Arrange
        const string expected = "*";

        var sut = new CorsPolicy();

        // Act
        var actual = sut.AllowedOrigins;

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public void AllowedOrigins_SetValue_IsExpectedValue()
    {
        // Arrange
        const string expected = "test";

        var sut = new CorsPolicy
        {
            AllowedOrigins = "test"
        };

        // Act
        var actual = sut.AllowedOrigins;

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public void ExposedHeaders_DefaultValue_IsExpectedValue()
    {
        // Arrange
        const string expected = "location";

        var sut = new CorsPolicy();

        // Act
        var actual = sut.ExposedHeaders;

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public void ExposedHeaders_SetValue_IsExpectedValue()
    {
        // Arrange
        const string expected = "test";

        var sut = new CorsPolicy
        {
            ExposedHeaders = "test"
        };

        // Act
        var actual = sut.ExposedHeaders;

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public void GetOrigins_RetrievesCorsPolicyValues_ReturnsArray()
    {
        // Arrange
        string[] expected = { "test", "test2" };

        var sut = new CorsPolicy { AllowedOrigins = "test;test2" };

        // Act
        var actual = sut.GetOrigins();

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public void GetHeaders_RetrievesCorsPolicyValues_ReturnsArray()
    {
        // Arrange
        string[] expected = { "test", "location" };

        var sut = new CorsPolicy
        {
            ExposedHeaders = "test;location"
        };

        // Act
        var actual = sut.GetHeaders();

        // Assert
        actual.ShouldBe(expected);
    }
}
