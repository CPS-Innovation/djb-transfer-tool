using Cps.Fct.Hk.Common.Contracts.Clients;
using Cps.Fct.Hk.Common.Contracts.Exceptions;
using RichardSzalay.MockHttp;
using Cps.Fct.Hk.Common.Tests.Unit.Framework.Clients;
using Cps.Fct.Hk.Common.Tests.Unit.Framework.Extensions;

namespace Cps.Fct.Hk.Common.Contracts.UnitTests.Clients;

public class ExternalJsonClientTests
{
    private static TestClient ClientWithRequest(
        MockHttpMessageHandler handler,
        Mock<ILogger<ExternalJsonClient<TestErrorResponse>>> logger)
    {
        var httpClient = handler.ToHttpClient();
        httpClient.BaseAddress = new Uri("http://localhost");

        return new TestClient(httpClient, logger.Object);
    }

    [Fact]
    public async Task GetAsync_WhenSuccessful_ReturnsResponse()
    {
        // Arrange
        const string response = "abc";
        var logger = new Mock<ILogger<ExternalJsonClient<TestErrorResponse>>>();

        using var handler = new MockHttpMessageHandler();
        var request = handler
            .When(HttpMethod.Get, "http://localhost/test")
            .RespondWithJsonObject(response);

        handler.AddRequestExpectation(request);

        var sut = ClientWithRequest(handler, logger);

        // Act
        var result = await sut.TestGetAsync("test");

        // Assert
        result.ShouldBe(response);
        logger.VerifyLogged("Tests GET: http://localhost/testSuffix", LogLevel.Information);
        handler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetAsync_WhenNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var logger = new Mock<ILogger<ExternalJsonClient<TestErrorResponse>>>();

        using var handler = new MockHttpMessageHandler();
        var request = handler
            .When(HttpMethod.Get, "http://localhost/test")
            .Respond(HttpStatusCode.NotFound);

        handler.AddRequestExpectation(request);

        var sut = ClientWithRequest(handler, logger);

        // Act
        var act = async () => await sut.TestGetAsync("test").ConfigureAwait(false);

        // Assert
        var result = await act.ShouldThrowAsync<NotFoundException>();
        result.Message.ShouldBe("Route: testSuffix returned Not Found");
        logger.VerifyLogged("Tests GET: http://localhost/testSuffix", LogLevel.Information);
        handler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetAsync_WhenUnauthorized_ThrowsUnauthorizedException()
    {
        // Arrange
        var logger = new Mock<ILogger<ExternalJsonClient<TestErrorResponse>>>();
        var response = new TestErrorResponse { ErrorDetails = "Test error" };

        using var handler = new MockHttpMessageHandler();
        var request = handler
            .When(HttpMethod.Get, "http://localhost/test")
            .RespondWithJsonObject(response, HttpStatusCode.Unauthorized);

        handler.AddRequestExpectation(request);

        var sut = ClientWithRequest(handler, logger);

        // Act
        var act = async () => await sut.TestGetAsync("test").ConfigureAwait(false);

        // Assert
        var result = await act.ShouldThrowAsync<UnauthorizedException>();
        result.Message.ShouldBe("Tests - Route: testSuffix returned error with code Unauthorized. Test error");
        logger.VerifyLogged("Tests GET: http://localhost/testSuffix", LogLevel.Information);
        logger.VerifyLogged("Error when calling API: Tests - Route: testSuffix returned error with code Unauthorized. Test error", LogLevel.Error);
        handler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetAsync_WhenBadRequest_ThrowsBadRequestException()
    {
        // Arrange
        var logger = new Mock<ILogger<ExternalJsonClient<TestErrorResponse>>>();
        var response = new TestErrorResponse { ErrorDetails = "Test error" };

        using var handler = new MockHttpMessageHandler();
        var request = handler
            .When(HttpMethod.Get, "http://localhost/test")
            .RespondWithJsonObject(response, HttpStatusCode.BadRequest);

        handler.AddRequestExpectation(request);

        var sut = ClientWithRequest(handler, logger);

        // Act
        var act = async () => await sut.TestGetAsync("test").ConfigureAwait(true);

        // Assert
        var result = await act.ShouldThrowAsync<BadRequestException>();
        result.Message.ShouldBe("Tests - Route: testSuffix returned error with code BadRequest. Test error");
        logger.VerifyLogged("Tests GET: http://localhost/testSuffix", LogLevel.Information);
        logger.VerifyLogged("Error when calling API: Tests - Route: testSuffix returned error with code BadRequest. Test error", LogLevel.Error);
        handler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetAsync_WhenOtherUnsuccessfulCode_ThrowsApiException()
    {
        // Arrange
        var logger = new Mock<ILogger<ExternalJsonClient<TestErrorResponse>>>();
        var response = new TestErrorResponse { ErrorDetails = "Test error" };

        using var handler = new MockHttpMessageHandler();
        var request = handler
            .When(HttpMethod.Get, "http://localhost/test")
            .RespondWithJsonObject(response, HttpStatusCode.InternalServerError);

        handler.AddRequestExpectation(request);

        var sut = ClientWithRequest(handler, logger);

        // Act
        var act = async () => await sut.TestGetAsync("test").ConfigureAwait(false);

        // Assert
        var result = await act.ShouldThrowAsync<ApiException>();
        result.Message.ShouldBe("Tests - Route: testSuffix returned error with code InternalServerError. Test error");
        logger.VerifyLogged("Tests GET: http://localhost/testSuffix", LogLevel.Information);
        logger.VerifyLogged("Error when calling API: Tests - Route: testSuffix returned error with code InternalServerError. Test error", LogLevel.Error);
        handler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task PostAsync_WhenSuccessful_ReturnsResponse()
    {
        // Arrange
        var requestBody = new TestErrorResponse { ErrorDetails = "test request" };

        const string response = "abc";
        var logger = new Mock<ILogger<ExternalJsonClient<TestErrorResponse>>>();

        using var handler = new MockHttpMessageHandler();

        var sut = ClientWithRequest(handler, logger);

        var requestJson = JsonSerializer.Serialize(requestBody, sut.TestSerializerOptions);
        var request = handler
            .When(HttpMethod.Post, "http://localhost/test")
            .WithContent(requestJson)
            .RespondWithJsonObject(response);

        handler.AddRequestExpectation(request);

        // Act
        var result = await sut.TestPostAsync("test", requestBody);

        // Assert
        result.ShouldBe(response);
        logger.VerifyLogged("Tests POST: http://localhost/testSuffix", LogLevel.Information);
        handler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task SendAsync_WhenSuccessful_ReturnsResponse()
    {
        // Arrange
        const string response = "abc";
        var logger = new Mock<ILogger<ExternalJsonClient<TestErrorResponse>>>();

        using var handler = new MockHttpMessageHandler();

        var sut = ClientWithRequest(handler, logger);

        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, "sendTest");

        requestMessage.Content = new FormUrlEncodedContent(
            new KeyValuePair<string, string>[]
            {
                new("key", "value")
            });

        var request = handler
            .When(HttpMethod.Post, "http://localhost/sendTest")
            .WithContent("key=value")
            .RespondWithJsonObject(response);

        handler.AddRequestExpectation(request);

        // Act
        var result = await sut.TestSendAsync(requestMessage);

        // Assert
        result.ShouldBe(response);
        logger.VerifyLogged("Tests SEND(POST): http://localhost/sendTestSuffix", LogLevel.Information);
        handler.VerifyNoOutstandingExpectation();
    }
}
