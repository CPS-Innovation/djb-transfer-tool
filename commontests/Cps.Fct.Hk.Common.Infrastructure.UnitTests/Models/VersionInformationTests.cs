namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Models;

public class VersionInformationTests
{
    [Fact]
    public void DefaultVersion_HasCorrectValue()
    {
        // Arrange & Act & Assert
        VersionInformation.DefaultVersion.ShouldBe("1.0.0");
    }

    [Fact]
    public void Ctor_SetsAllProperties()
    {
        // Arrange & Act
        var sut = new VersionInformation(
            buildNumber: "0.1.2",
            fileVersion: "2.3.4",
            informationalVersion: "3.4.5",
            version: "1.2.3");

        // Assert
        sut.BuildNumber.ShouldBe("0.1.2");
        sut.FileVersion.ShouldBe("2.3.4");
        sut.InformationalVersion.ShouldBe("3.4.5");
        sut.Version.ShouldBe("1.2.3");
    }

    [Fact]
    public void Default_ConstructsWithExpectedValues()
    {
        // Arrange & Act
        var sut = VersionInformation.Default();

        // Assert
        sut.BuildNumber.ShouldBe("1.0.0");
        sut.FileVersion.ShouldBe("1.0.0");
        sut.InformationalVersion.ShouldBe("1.0.0");
        sut.Version.ShouldBe("1.0.0");
    }

    [Fact]
    public void WithVersion_SetsAllValuesToProvidedVersion()
    {
        // Arrange
        const string version = "1.1.1";

        // Act
        var sut = VersionInformation.WithVersion(version);

        // Assert
        sut.BuildNumber.ShouldBe(version);
        sut.FileVersion.ShouldBe(version);
        sut.InformationalVersion.ShouldBe(version);
        sut.Version.ShouldBe(version);
    }
}
