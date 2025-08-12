using Newtonsoft.Json.Linq;
using Cps.Fct.Hk.Common.Contracts.Exceptions;
using RichardSzalay.MockHttp;
using ValidationProblemDetails = Cps.Fct.Hk.Common.Contracts.Models.ValidationProblemDetails;
using Cps.Fct.Hk.Common.Infrastructure.UnitTests.Models;

namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Extensions;

public class HttpClientExtensionsTests : IDisposable
{
    /// <summary>
    /// The _handlerMock setup implements "SendAsync".
    /// HttpMessageHandler's "SendAsync" method will be called under the hood whenever the HttpClient calls GetAsync/PostAsync.
    /// For more information, Please read <see href="https://carlpaton.github.io/2021/01/mocking-httpclient-sendasync/"/>.
    /// </summary>
    private readonly HttpClient _sut;

    private readonly MockHttpMessageHandler _handler;
    private bool _isDisposed;

    public HttpClientExtensionsTests()
    {
        _handler = new MockHttpMessageHandler();
        _sut = new HttpClient(_handler);
    }

    [Fact]
    public async Task HttpClient_GetAsync_ReturnsResponse()
    {
        // Arrange
        const string route = "http://localhost:7386";

        var request = _handler
            .When(HttpMethod.Get, route)
            .Respond(HttpStatusCode.OK, message =>
                message.Content = new StringContent(new JObject(new JProperty("Status", 200)).ToString()));

        _handler.AddRequestExpectation(request);

        // Act
        var result = await _sut.GetAsync<DummyResponse>(route);

        // Assert
        _handler.VerifyNoOutstandingExpectation();
        _handler.GetMatchCount(request).ShouldBe(1);

        result.ShouldNotBeNull();
        result.Status.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public void HttpClient_GetAsync_ThrowsNotFoundException()
    {
        // Arrange
        const string route = "http://localhost:7386";

        var request = _handler
            .When(HttpMethod.Get, route)
            .Respond(HttpStatusCode.NotFound);

        _handler.AddRequestExpectation(request);

        // Act
        var result = _sut.GetAsync<DummyResponse>(route);

        // Assert
        _handler.VerifyNoOutstandingExpectation();
        _handler.GetMatchCount(request).ShouldBe(1);

        result.ShouldThrow<NotFoundException>();
    }

    [Fact]
    public async Task HttpClient_PostAsync_ReturnsResponse()
    {
        // Arrange
        const string route = "http://localhost:7386";

        var request = _handler
            .When(HttpMethod.Post, route)
            .Respond(HttpStatusCode.OK);

        _handler.AddRequestExpectation(request);

        // Act
        await _sut.PostAsync(route);

        // Assert
        _handler.VerifyNoOutstandingExpectation();
        _handler.GetMatchCount(request).ShouldBe(1);
    }

    [Fact]
    public void HttpClient_PostAsync_ThrowsApiException()
    {
        // Arrange
        const string route = "http://localhost:7386";

        var request = _handler
            .When(HttpMethod.Post, route)
            .Respond(HttpStatusCode.BadGateway);

        _handler.AddRequestExpectation(request);

        // Act
        var result = _sut.PostAsync(route);

        // Assert
        _handler.VerifyNoOutstandingExpectation();
        _handler.GetMatchCount(request).ShouldBe(1);

        result.ShouldThrow<ApiException>($"Response for requested URI: {route} did not return successfully. Status Code: {HttpStatusCode.BadGateway}");
    }

    [Fact]
    public async Task HttpClient_PostAsyncWithTRequest_ReturnsResponse()
    {
        // Arrange
        const string route = "http://localhost:7386";

        var request = _handler
            .When(HttpMethod.Post, route)
            .Respond(HttpStatusCode.OK, message =>
                message.Content = new StringContent(new JObject(new JProperty("Status", 200)).ToString()));

        _handler.AddRequestExpectation(request);

        // Act
        var result = await _sut.PostAsync(route, new DummyRequest());

        // Assert
        _handler.VerifyNoOutstandingExpectation();
        _handler.GetMatchCount(request).ShouldBe(1);

        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public void HttpClient_PostAsyncWithTRequest_ThrowsBadRequestException()
    {
        // Arrange
        const string route = "http://localhost:7386";

        var request = _handler
            .When(HttpMethod.Post, route)
            .Respond(HttpStatusCode.BadRequest, message =>
                message.Content = new StringContent(new JObject(new JProperty("errors", new JObject(new JProperty("Error", new JArray("Error"))))).ToString()));

        _handler.AddRequestExpectation(request);

        // Act
        var result = _sut.PostAsync(route, new ValidationProblemDetails());

        // Assert
        _handler.VerifyNoOutstandingExpectation();
        _handler.GetMatchCount(request).ShouldBe(1);

        result.ShouldThrow<BadRequestException>($"Route: {route} returned Bad Request. One or more validation errors occurred.: Error");
    }

    [Fact]
    public async Task HttpClient_PostAsyncWithTRequestTResponse_ReturnsResponse()
    {
        // Arrange
        const string route = "http://localhost:7386";

        var request = _handler
            .When(HttpMethod.Post, route)
            .Respond(HttpStatusCode.OK, message =>
                message.Content = new StringContent(new JObject(new JProperty("Status", 200)).ToString()));

        _handler.AddRequestExpectation(request);

        // Act
        var result = await _sut.PostAsync<DummyRequest, DummyResponse>(route, new DummyRequest());

        // Assert
        _handler.VerifyNoOutstandingExpectation();
        _handler.GetMatchCount(request).ShouldBe(1);

        result.ShouldNotBeNull();
        result.Status.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task HttpClient_PostAsyncWithTRequestTResponse_HasStatusCodeNoContent_ReturnEmptyResponse()
    {
        // Arrange
        const string route = "http://localhost:7386";

        var request = _handler
            .When(HttpMethod.Post, route)
            .Respond(HttpStatusCode.NoContent);

        _handler.AddRequestExpectation(request);

        // Act
        var result = await _sut.PostAsync<DummyRequest, DummyResponse>(route, new DummyRequest());

        // Assert
        _handler.VerifyNoOutstandingExpectation();
        _handler.GetMatchCount(request).ShouldBe(1);

        result.ShouldBeNull();
    }

    [Fact]
    public void HttpClient_PostAsyncWithTRequestTResponse_ThrowsNotFoundException()
    {
        // Arrange
        const string route = "http://localhost:7386";

        var request = _handler
            .When(HttpMethod.Post, route)
            .Respond(HttpStatusCode.Forbidden);

        _handler.AddRequestExpectation(request);

        // Act
        var result = _sut.PostAsync<DummyRequest, DummyResponse>(route, new DummyRequest());

        // Assert
        _handler.VerifyNoOutstandingExpectation();
        _handler.GetMatchCount(request).ShouldBe(1);

        result.ShouldThrow<ApiException>();
    }

    [Fact]
    public async Task HttpClient_PatchAsyncWithTRequest_ReturnsResponse()
    {
        // Arrange
        const string route = "http://localhost:7386";

        var request = _handler
            .When(HttpMethod.Patch, route)
            .Respond(HttpStatusCode.OK, message =>
                message.Content = new StringContent(new JObject(new JProperty("Status", 200)).ToString()));

        _handler.AddRequestExpectation(request);

        // Act
        var result = await _sut.PatchAsync(route, new DummyRequest());

        // Assert
        _handler.VerifyNoOutstandingExpectation();
        _handler.GetMatchCount(request).ShouldBe(1);

        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public void HttpClient_PatchAsyncWithTRequest_ThrowsBadRequestException()
    {
        // Arrange
        const string route = "http://localhost:7386";

        var request = _handler
            .When(HttpMethod.Patch, route)
            .Respond(HttpStatusCode.BadRequest, message =>
                message.Content = new StringContent(new JObject(new JProperty("errors", new JObject(new JProperty("Error", new JArray("Error"))))).ToString()));

        _handler.AddRequestExpectation(request);

        // Act
        var result = _sut.PatchAsync(route, new ValidationProblemDetails());

        // Assert
        _handler.VerifyNoOutstandingExpectation();
        _handler.GetMatchCount(request).ShouldBe(1);

        result.ShouldThrow<BadRequestException>($"Route: {route} returned Bad Request. One or more validation errors occurred.: Error");
    }

    [Fact]
    public void HttpClient_PatchAsyncWithTRequest_ThrowsNotFoundException()
    {
        // Arrange
        const string route = "http://localhost:7386";

        var request = _handler
            .When(HttpMethod.Patch, route)
            .Respond(HttpStatusCode.Forbidden);

        _handler.AddRequestExpectation(request);

        // Act
        var result = _sut.PatchAsync(route, new DummyRequest());

        // Assert
        _handler.VerifyNoOutstandingExpectation();
        _handler.GetMatchCount(request).ShouldBe(1);

        result.ShouldThrow<ApiException>();
    }

    [Fact]
    public async Task HttpClient_PutAsyncWithTRequest_ReturnsResponse()
    {
        // Arrange
        const string route = "http://localhost:7386";

        var request = _handler
            .When(HttpMethod.Put, route)
            .Respond(HttpStatusCode.OK, message =>
                message.Content = new StringContent(new JObject(new JProperty("Status", 200)).ToString()));

        _handler.AddRequestExpectation(request);

        // Act
        var result = await _sut.PutAsync(route, new DummyRequest());

        // Assert
        _handler.VerifyNoOutstandingExpectation();
        _handler.GetMatchCount(request).ShouldBe(1);

        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public void HttpClient_PutAsyncWithTRequest_ThrowsBadRequestException()
    {
        // Arrange
        const string route = "http://localhost:7386";

        var request = _handler
            .When(HttpMethod.Put, route)
            .Respond(HttpStatusCode.BadRequest, message =>
                message.Content = new StringContent(new JObject(new JProperty("errors", new JObject(new JProperty("Error", new JArray("Error"))))).ToString()));

        _handler.AddRequestExpectation(request);

        // Act
        var result = _sut.PutAsync(route, new ValidationProblemDetails());

        // Assert
        _handler.VerifyNoOutstandingExpectation();
        _handler.GetMatchCount(request).ShouldBe(1);

        result.ShouldThrow<BadRequestException>($"Route: {route} returned Bad Request. One or more validation errors occurred.: Error");
    }

    [Fact]
    public void HttpClient_PutAsyncWithTRequest_ThrowsNotFoundException()
    {
        // Arrange
        const string route = "http://localhost:7386";

        var request = _handler
            .When(HttpMethod.Put, route)
            .Respond(HttpStatusCode.Forbidden);

        _handler.AddRequestExpectation(request);

        // Act
        var result = _sut.PutAsync(route, new DummyRequest());

        // Assert
        _handler.VerifyNoOutstandingExpectation();
        _handler.GetMatchCount(request).ShouldBe(1);

        result.ShouldThrow<ApiException>();
    }

    [Fact]
    public async Task HttpClient_PutAsyncWithTRequestTResponse_ReturnsResponse()
    {
        // Arrange
        const string route = "http://localhost:7386";

        var request = _handler
            .When(HttpMethod.Put, route)
            .Respond(HttpStatusCode.OK, message =>
                message.Content = new StringContent(new JObject(new JProperty("Status", 200)).ToString()));

        _handler.AddRequestExpectation(request);

        // Act
        var result = await _sut.PutAsync<DummyRequest, DummyResponse>(route, new DummyRequest());

        // Assert
        _handler.VerifyNoOutstandingExpectation();
        _handler.GetMatchCount(request).ShouldBe(1);

        result.ShouldNotBeNull();
        result.Status.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task HttpClient_PutAsyncWithTRequestTResponse_HasStatusCodeNoContent_ReturnEmptyResponse()
    {
        // Arrange
        const string route = "http://localhost:7386";

        var request = _handler
            .When(HttpMethod.Put, route)
            .Respond(HttpStatusCode.NoContent);

        _handler.AddRequestExpectation(request);

        // Act
        var result = await _sut.PutAsync<DummyRequest, DummyResponse>(route, new DummyRequest());

        // Assert
        _handler.VerifyNoOutstandingExpectation();
        _handler.GetMatchCount(request).ShouldBe(1);

        result.ShouldBeNull();
    }

    [Fact]
    public void HttpClient_PutAsyncWithTRequestTResponse_ThrowsNotFoundException()
    {
        // Arrange
        const string route = "http://localhost:7386";

        var request = _handler
            .When(HttpMethod.Put, route)
            .Respond(HttpStatusCode.Forbidden);

        _handler.AddRequestExpectation(request);

        // Act
        var result = _sut.PutAsync<DummyRequest, DummyResponse>(route, new DummyRequest());

        // Assert
        _handler.VerifyNoOutstandingExpectation();
        _handler.GetMatchCount(request).ShouldBe(1);

        result.ShouldThrow<ApiException>();
    }

    [Fact]
    public async Task HttpClient_CreateEntityAsync_ReturnsLocation()
    {
        // Arrange
        const string route = "http://localhost:7386/api/test";

        var request = _handler
            .When(HttpMethod.Post, route)
            .Respond(_ =>
            {
                var response = new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent(new JObject(new JProperty("Status", 201)).ToString())
                };
                response.Headers.Location = new Uri(route);
                return response;
            });

        _handler.AddRequestExpectation(request);

        // Act
        var result = await _sut.CreateEntityAsync(route, new DummyRequest());

        // Assert
        _handler.VerifyNoOutstandingExpectation();
        _handler.GetMatchCount(request).ShouldBe(1);

        result.ShouldBe("test");
    }

    [Fact]
    public void HttpClient_CreateEntityAsync_ThrowsApiExceptionForUnexpectedStatusCode()
    {
        // Arrange
        const string route = "http://localhost:7386/api/test";

        var request = _handler
            .When(HttpMethod.Post, route)
            .Respond(_ =>
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(new JObject(new JProperty("Status", 200)).ToString())
                };
                response.Headers.Location = new Uri(route);
                return response;
            });

        _handler.AddRequestExpectation(request);

        // Act
        var result = _sut.CreateEntityAsync(route, new DummyRequest());

        // Assert
        _handler.VerifyNoOutstandingExpectation();
        _handler.GetMatchCount(request).ShouldBe(1);

        result.ShouldThrow<ApiException>($"Unexpected Status code received: {HttpStatusCode.OK}");
    }

    [Fact]
    public void HttpClient_CreateEntityAsync_ThrowsApiExceptionForEmptyLocation()
    {
        // Arrange
        const string route = "http://localhost:7386/api/test";

        var request = _handler
            .When(HttpMethod.Post, route)
            .Respond(HttpStatusCode.Created, message =>
                message.Content = new StringContent(new JObject(new JProperty("Status", 200)).ToString()));

        _handler.AddRequestExpectation(request);

        // Act
        var result = _sut.CreateEntityAsync(route, new DummyRequest());

        // Assert
        _handler.VerifyNoOutstandingExpectation();
        _handler.GetMatchCount(request).ShouldBe(1);

        result.ShouldThrow<ApiException>("Location header is missing from the response");
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            _sut.Dispose();
            _handler.Dispose();
        }

        _isDisposed = true;
    }
}
