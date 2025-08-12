namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Extensions;

public class EnumerableExtensionsTests
{
    [Fact]
    public void ToCsv_GivenEmptyArray_ReturnsEmptyString()
    {
        // Arrange
        var sut = new List<string[]?>();

        // Act
        var result = sut!.ToCsv(",");

        // Assert
        result.ShouldBeEquivalentTo(string.Empty);
    }

    [Fact]
    public void ToCsv_GivenPopulatedArray_ReturnsCsvString()
    {
        // Arrange
        var sut = new List<string[]?>
        {
            new string[] { "One", "Two", "Three", "Four", "Five" },
            new string[] { "A", "B", "C", "D", "E" },
            new string[] { "I", "II", "III", "IV", "V" }
        };
        var expectedCsv = $"One,Two,Three,Four,Five{Environment.NewLine}" +
                            $"A,B,C,D,E{Environment.NewLine}" +
                            $"I,II,III,IV,V{Environment.NewLine}";

        // Act
        var result = sut!.ToCsv(",");

        // Assert
        result.ShouldBeEquivalentTo(expectedCsv);
    }

    [Fact]
    public void ForEach_GiveArray_ShouldIterate()
    {
        // Arrange
        var sut = new string[3];
        var count = 0;

        // Act
        sut.ForEach(_ => count++);

        // Assert
        count.ShouldBe(3);
    }
}
