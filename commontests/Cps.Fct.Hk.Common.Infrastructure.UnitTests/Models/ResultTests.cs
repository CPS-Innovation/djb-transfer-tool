namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Models;
using ValidationProblemDetails = Cps.Fct.Hk.Common.Contracts.Models.ValidationProblemDetails;

public class ResultTests
{
    [Fact]
    public void Constructor_ReturnsResultWithoutStatus()
    {
        // Arrange & Act
        var result = new Result();

        // Assert
        result.Status.ShouldBe((HttpStatusCode)0);
    }

    [Fact]
    public void Constructor_ReturnsGenericResultWithoutStatus()
    {
        // Arrange & Act
        var result = new Result<object>();

        // Assert
        result.Status.ShouldBe((HttpStatusCode)0);
    }

    [Fact]
    public void Ok_ReturnsResultWithOkStatusCode()
    {
        // Arrange & Act
        var result = Result.Ok("some-value");

        // Assert
        result.Status.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public void Ok_ReturnsResultWithValueSet()
    {
        // Arrange
        var someValue = new object();

        // Act
        var result = Result.Ok(someValue);

        // Assert
        result.Value.ShouldBe(someValue);
    }

    [Fact]
    public void Created_ReturnsResultWithCreatedStatusCode()
    {
        // Arrange & Act
        var result = Result.Created("some-value");

        // Assert
        result.Status.ShouldBe(HttpStatusCode.Created);
    }

    [Fact]
    public void Created_ReturnsResultWithValueSet()
    {
        // Arrange
        var someValue = new object();

        // Act
        var result = Result.Created(someValue);

        // Assert
        result.Value.ShouldBe(someValue);
    }

    [Fact]
    public void Created_ReturnsResultWithCustomStateSet()
    {
        // Arrange
        var someValue = new object();

        // Act
        var result = Result.Created(someValue, new Uri("https://some-uri.com"));

        // Assert
        result.CustomState.ShouldBeEquivalentTo(new Uri("https://some-uri.com"));
    }

    [Fact]
    public void Updated_ReturnsResultWithOkStatusCode()
    {
        // Arrange & Act
        var result = Result.Updated("some-value");

        // Assert
        result.Status.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public void Updated_ReturnsResultWithValueSet()
    {
        // Arrange
        var someValue = new object();

        // Act
        var result = Result.Updated(someValue, new Uri("https://some-uri.com"));

        // Assert
        result.CustomState.ShouldBeEquivalentTo(new Uri("https://some-uri.com"));
    }

    [Fact]
    public void NotFound_ReturnsResultWithNotFoundStatusCode()
    {
        // Arrange & Act
        var result = Result.NotFound<object>("some-custom-state");

        // Assert
        result.Status.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public void NotFound_ReturnsResultWithCustomStateSet()
    {
        // Arrange
        var customState = new object();

        // Act
        var result = Result.NotFound<object>(customState);

        // Assert
        result.CustomState.ShouldBe(customState);
    }

    [Fact]
    public void BadRequest_ReturnsResultWithBadRequestStatusCode()
    {
        // Arrange & Act
        var result = Result.BadRequest<object>("some-custom-state");

        // Assert
        result.Status.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public void BadRequest_ReturnsResultWithCustomStateSet()
    {
        // Arrange
        var customState = new ValidationProblemDetails();

        // Act
        var result = Result.BadRequest<object>(customState);

        // Assert
        result.CustomState.ShouldBe(customState);
    }

    [Fact]
    public void InternalServerError_ReturnsResultWithInternalServerErrorStatusCode()
    {
        // Arrange & Act
        var result = Result.InternalServerError<object>("some-custom-state");

        // Assert
        result.Status.ShouldBe(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public void InternalServerError_ReturnsResultWithCustomStateSet()
    {
        // Arrange
        var customState = new object();

        // Act
        var result = Result.InternalServerError<object>(customState);

        // Assert
        result.CustomState.ShouldBe(customState);
    }

    [Fact]
    public void StatusCode_ReturnsResultWithStatusCode()
    {
        // Arrange & Act
        var result = Result.StatusCode<object>(
            HttpStatusCode.Conflict, "some-custom-state");

        // Assert
        result.Status.ShouldBe(HttpStatusCode.Conflict);
    }

    [Fact]
    public void StatusCode_ReturnsResultWithCustomStateSet()
    {
        // Arrange
        var customState = new object();

        // Act
        var result = Result.StatusCode<object>(HttpStatusCode.Forbidden, customState);

        // Assert
        result.CustomState.ShouldBe(customState);
    }

    [Theory]
    [InlineData(HttpStatusCode.OK, true)]
    [InlineData(HttpStatusCode.Created, true)]
    [InlineData(HttpStatusCode.BadGateway, false)]
    [InlineData(HttpStatusCode.Processing, false)]
    public void IsSuccess_ReturnsValueBasedOnResultStatusCode(
        HttpStatusCode statusCode, bool expectedResult)
    {
        // Arrange & Act
        var result = Result.StatusCode<object>(statusCode).IsSuccess();

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Fact]
    public void NoContent_ReturnsResultWithNoContentStatusCode()
    {
        // Arrange & Act
        var result = Result.NoContent<object>();

        // Assert
        result.Status.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public void Unauthorized_ReturnsResultWithUnauthorizedCode()
    {
        // Arrange & Act
        var result = Result.Unauthorized<object>();

        // Assert
        result.Status.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public void Unauthorized_ReturnsResultWithCustomStateSet()
    {
        // Arrange
        var customState = new object();

        // Act
        var result = Result.Unauthorized<object>(customState);

        // Assert
        result.CustomState.ShouldBe(customState);
    }

    [Fact]
    public void ValidationErrors_Unauthorized_ShouldBeNull()
    {
        // Arrange & Act
        var result = Result.Unauthorized<object>();

        // Assert
        result.ValidationErrors.ShouldBeNull();
    }

    [Fact]
    public void ValidationErrors_BadRequest_ShouldBeSet()
    {
        // Arrange & Act
        var result = Result.BadRequest<object>("Test title");

        // Assert
        result.ValidationErrors!.Title.ShouldBe("Test title");
    }
}
