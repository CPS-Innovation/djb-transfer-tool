namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Extensions;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("", new string[0])]
    [InlineData(" ; ", new string[0])]
    [InlineData("tenant-id-1", new[] { "tenant-id-1" })]
    [InlineData("tenant-id-1;tenant-id-2", new[] { "tenant-id-1", "tenant-id-2" })]
    [InlineData("tenant-id-1;;tenant-id-2", new[] { "tenant-id-1", "tenant-id-2" })]
    [InlineData("tenant-id-1;;tenant-id-2;", new[] { "tenant-id-1", "tenant-id-2" })]
    public void SplitValues_SplitsAsExpected(string value, string[] expected)
    {
        // Arrange & Act
        var result = value.SplitValues().ToList();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(expected.Length);
        result.Intersect(expected).Count().ShouldBe(expected.Length);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("A", "a")]
    [InlineData("ab", "ab")]
    [InlineData("MyProperty", "myProperty")]
    [InlineData("My", "my")]
    public void ToCamelCase_FormatsAsExpected(string value, string expected)
    {
        // Arrange & Act
        var result = value.ToCamelCase();

        // Assert
        result.ShouldBe(expected);
    }
}
