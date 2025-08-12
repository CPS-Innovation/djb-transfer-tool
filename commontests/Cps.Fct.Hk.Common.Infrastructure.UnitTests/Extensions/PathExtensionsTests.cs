namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Extensions;

/// <summary>Tests for System.IO.Path extensions.</summary>
public class PathExtensionsTests
{
    [Fact]
    public void Join_GivenList_ShouldReturnCorrectString()
    {
        // Arrange
        var filePaths = new[] { "/path1/file1.csv", "/path2/file2.csv" };

        // Act
        var sut = PathExtensions.Join(filePaths);

        // Assert
        sut.ShouldBe("file1.csv,file2.csv");
    }

    [Theory]
    [InlineData("/Outgoing/Test_file_20230217.csv", "^Test_file_[0-9]{8}.csv$", true)]
    [InlineData("Test_file_20230217.csv", "^Test_file_[0-9]{8}.csv$", true)]
    [InlineData("/Outgoing/Test_file_20230217.csv", "Test_file_[0-9]{8}.csv", true)]
    [InlineData("Test_file_2023021.csv", "Test_file_[0-9]{8}.csv", false)]
    [InlineData("invalidname.csv", "Test_file_[0-9]{8}.csv", false)]
    [InlineData("/Outgoing/Test_file_20230217.xml", "Test_file_[0-9]{8}.csv", false)]
    public void MatchesPattern_GivenFilePathAndPattern_ReturnsCorrectMatch(string filePath, string pattern, bool matches)
    {
        // Act
        var sut = PathExtensions.MatchesPattern(filePath, pattern);

        // Assert
        sut.ShouldBe(matches);
    }
}
