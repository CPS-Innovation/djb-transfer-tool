namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Extensions;

public class AssembliesExtensionsTests
{
    [Fact]
    public void AllowedAssemblyStartNames_Elements_Whitelist()
    {
        // Arrange & Act
        var startNames = AssembliesExtensions.AllowedAssemblyStartNames.ToArray();

        // Assert
        startNames.ShouldHaveSingleItem();
        startNames[0].ShouldBe("CPS");
    }

    [Fact]
    public void GetPetsAssemblies_EntryAssemblyMatches_GetsReturned()
    {
        // Arrange
        var entryAssembly = SetupMockAssembly("CPS.EntryAssembly");

        // Act
        var assemblies = entryAssembly.Object.GetCPSAssemblies();

        // Assert
        assemblies.ShouldHaveSingleItem();
        assemblies[0].FullName.ShouldBe("CPS.EntryAssembly");
    }

    [Fact]
    public void GetPetsAssemblies_EntryAssemblyDoesNotMatch_DoesNotGetReturned()
    {
        // Arrange
        var entryAssembly = SetupMockAssembly("Random.EntryAssembly");

        // Act
        var assemblies = entryAssembly.Object.GetCPSAssemblies();

        // Assert
        assemblies.ShouldBeEmpty();
    }

    [Fact]
    public void GetPetsAssemblies_FindsAllowedAssembliesNested_Returns()
    {
        // Arrange
        var entryAssembly = SetupMockAssembly("CPS.EntryAssembly");

        var rbMajor1Assembly = SetupMockAssembly("CPS.MajorAssembly1");
        var rbMajor2Assembly = SetupMockAssembly("CPS.MajorAssembly2");
        var nonPahMajor1Assembly = SetupMockAssembly("Non.MajorAssembly1");
        var rbMinor1Assembly = SetupMockAssembly("CPS.MinorAssembly1");
        var rbMinor2Assembly = SetupMockAssembly("CPS.MinorAssembly2");
        var nonPahMinor1Assembly = SetupMockAssembly("Non.MinorAssembly1");
        var allMockedAssemblies = new[]
        {
            entryAssembly,
            rbMajor1Assembly,
            rbMajor2Assembly,
            nonPahMajor1Assembly,
            rbMinor1Assembly,
            rbMinor2Assembly,
            nonPahMinor1Assembly
        };

        AssembliesExtensions.AssemblyLoad = assemblyName
            => allMockedAssemblies.First(x => x.Object.GetName() == assemblyName).Object;

        rbMajor1Assembly.Setup(x => x.GetReferencedAssemblies()).Returns(new[]
        {
            rbMajor1Assembly.Object.GetName(),
            nonPahMajor1Assembly.Object.GetName()
        });

        rbMajor2Assembly.Setup(x => x.GetReferencedAssemblies()).Returns(new[]
        {
            rbMinor1Assembly.Object.GetName(),
            rbMinor2Assembly.Object.GetName(),
            nonPahMinor1Assembly.Object.GetName()
        });

        entryAssembly.Setup(x => x.GetReferencedAssemblies()).Returns(new[]
        {
            rbMajor1Assembly.Object.GetName(),
            nonPahMajor1Assembly.Object.GetName(),
            rbMajor2Assembly.Object.GetName()
        });

        // Act
        var assemblies = entryAssembly.Object.GetCPSAssemblies();

        // Assert
        assemblies = assemblies.OrderBy(x => x.FullName).ToArray();
        assemblies.Length.ShouldBe(5);
        assemblies[0].FullName.ShouldBe("CPS.EntryAssembly");
        assemblies[1].FullName.ShouldBe("CPS.MajorAssembly1");
        assemblies[2].FullName.ShouldBe("CPS.MajorAssembly2");
        assemblies[3].FullName.ShouldBe("CPS.MinorAssembly1");
        assemblies[4].FullName.ShouldBe("CPS.MinorAssembly2");
    }

    private static Mock<Assembly> SetupMockAssembly(string name)
    {
        var assembly = new Mock<Assembly>();
        var assemblyName = new AssemblyName(name);
        assembly.Setup(x => x.FullName).Returns(assemblyName.FullName);
        assembly.Setup(x => x.GetName()).Returns(assemblyName);

        return assembly;
    }
}
