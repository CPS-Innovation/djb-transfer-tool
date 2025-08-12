using Cps.Fct.Hk.Common.Infrastructure.Abstractions.Implementations;

namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Abstractions;

public class AssemblyInfoProviderTests
{
    private static string DefaultVersion { get; } = "1.0.0";

    private static VersionInformation DefaultVersionInfo { get; } = VersionInformation.Default();

    [Fact]
    public void Ctor_NoValuesPresent_ReturnsDefault()
    {
        // Arrange & Act
        var sut = SetupSut();

        // Assert
        sut.BuildNumber.ShouldBe(DefaultVersion);
        sut.FileVersion.ShouldBe(DefaultVersion);
        sut.InformationalVersion.ShouldBe(DefaultVersion);
        sut.Version.ShouldBe(DefaultVersion);
        sut.VersionInfo.ShouldBe(DefaultVersionInfo);
    }

    [Fact]
    public void Ctor_ValuesPresent_Returns()
    {
        // Arrange & Act
        var versionInfo = new VersionInformation(
            buildNumber: "0.1.2",
            fileVersion: "2.3.4",
            informationalVersion: "3.4.5",
            version: "1.2.3");

        var sut = SetupSut((assembly, envProvider) =>
        {
            envProvider.Setup(x => x.GetEnvironmentVariable("BUILD_NUMBER")).Returns(versionInfo.BuildNumber);
            assembly.Object.GetName().Version = new Version(versionInfo.Version);
            assembly
                .Setup(x => x.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false))
                .Returns(new object[] { new AssemblyFileVersionAttribute(versionInfo.FileVersion) });
            assembly
                .Setup(x => x.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false))
                .Returns(new object[] { new AssemblyInformationalVersionAttribute(versionInfo.InformationalVersion) });
        });

        // Assert
        sut.BuildNumber.ShouldBe(versionInfo.BuildNumber);
        sut.FileVersion.ShouldBe(versionInfo.FileVersion);
        sut.InformationalVersion.ShouldBe(sut.InformationalVersion);
        sut.Version.ShouldBe(versionInfo.Version);
        sut.VersionInfo.ShouldBe(versionInfo);
    }

    private static Mock<Assembly> SetupMockAssembly(string name)
    {
        var assembly = new Mock<Assembly>();
        var assemblyName = new AssemblyName(name);
        assembly.Setup(x => x.FullName).Returns(assemblyName.FullName);
        assembly.Setup(x => x.GetName()).Returns(assemblyName);

        return assembly;
    }

    private static IAssemblyInfoProvider SetupSut(Action<Mock<Assembly>, Mock<IEnvironmentValuesProvider>>? assemblyAction = null)
    {
        var assemblyMock = SetupMockAssembly("CPS.TestAssembly");

        var envProviderMock = new Mock<IEnvironmentValuesProvider>();
        var assemblyProviderMock = new Mock<IAssemblyProvider>();
        assemblyProviderMock.Setup(x => x.GetEntryAssembly()).Returns(assemblyMock.Object);

        var assemblyProvider = new AssemblyInfoProvider(envProviderMock.Object, assemblyProviderMock.Object);

        assemblyAction?.Invoke(assemblyMock, envProviderMock);

        return assemblyProvider;
    }
}
