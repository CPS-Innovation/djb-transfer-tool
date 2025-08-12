namespace Cps.Fct.Hk.Common.Contracts.UnitTests.Exceptions;

public class UnauthorizedExceptionTests
{
    [Fact]
    public void UnauthorizedException_Construct_NoParameters()
    {
        // Act
        var result = new UnauthorizedAccessException();

        // Assert
        result.ShouldNotBeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("Exception details.")]
    public void NotFoundException_Construct_Message(string message)
    {
        // Act
        var result = new UnauthorizedAccessException(message);

        // Assert
        result.Message.ShouldBe(message);
    }
}
