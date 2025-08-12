namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Extensions;
using Cps.Fct.Hk.Common.Infrastructure.UnitTests.Extensions.TestData;

public class EnumerationExtensionsTests
{
    [Theory]
    [InlineData(TestEnumeration.BigDog, "This is a Big Dog!")]
    [InlineData(TestEnumeration.MassiveFox, "Massive Fox")]
    [InlineData(TestEnumeration.SmallCat, "SmallCat")]
    public void GetDisplayName_ProvidedValue_ReturnsExpectedValue(TestEnumeration enumeration, string expectedValue)
    {
        // Arrange & Act
        var result = enumeration.GetDisplayName();

        // Assert
        result.ShouldBe(expectedValue);
    }
}
