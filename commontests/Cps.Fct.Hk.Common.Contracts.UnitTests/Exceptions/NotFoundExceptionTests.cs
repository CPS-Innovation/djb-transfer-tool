using Cps.Fct.Hk.Common.Contracts.Exceptions;

namespace Cps.Fct.Hk.Common.Contracts.UnitTests.Exceptions;

public class NotFoundExceptionTests
{
    [Fact]
    public void NotFoundException_Construct_NoParameters()
    {
        // Act
        var result = new NotFoundException();

        // Assert
        result.ShouldNotBeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("Exception details.")]
    public void NotFoundException_Construct_MessageOnly(string message)
    {
        // Act
        var result = new NotFoundException(message);

        // Assert
        result.Message.ShouldBe(message);
    }

    [Fact]
    public void NotFoundException_Construct_InnerException()
    {
        // Arrange
        const string testMessage = "Test Message";
        var innerException = new InvalidTimeZoneException("Inner exception details");

        // Act
        var result = new NotFoundException(testMessage, innerException);

        // Assert
        result.Message.ShouldBe(testMessage);
        result.InnerException.ShouldBeSameAs(innerException);
    }

    [Fact]
    public void NotFoundException_Construct_NameAndKey()
    {
        // Arrange
        const int key = 26;

        // Act
        var result = new NotFoundException("IdValue", key);

        // Assert
        result.Message.ShouldBe("IdValue (26) was not found.");
    }
}
