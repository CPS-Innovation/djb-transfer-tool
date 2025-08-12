using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cps.Fct.Hk.Common.Contracts.Exceptions;
using Microsoft.Extensions.Logging;

namespace Cps.Fct.Hk.Common.Contracts.Clients;

/// <summary>
/// Client with common methods to communicate with external APIs and log errors.
/// </summary>
/// <typeparam name="TErrorResponse">The response sent if an error occurs.</typeparam>
public abstract class ExternalJsonClient<TErrorResponse>
    where TErrorResponse : IExternalErrorResponse
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExternalJsonClient<TErrorResponse>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalJsonClient{TErrorResponse}"/> class.
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="logger"></param>
    protected ExternalJsonClient(
        HttpClient httpClient,
        ILogger<ExternalJsonClient<TErrorResponse>> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// Format to serialize Json for request/responses.
    /// </summary>
    protected JsonSerializerOptions SerializerOptions { get; } = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    /// <summary>
    /// The name of the API, which is used for logging.
    /// </summary>
    protected abstract string ApiName { get; }

    /// <summary>
    /// Get the route to log, with any credentials masked/removed.
    /// </summary>
    /// <param name="route"></param>
    /// <returns></returns>
    protected virtual string RouteForLogging(string? route)
    {
        return route ?? string.Empty;
    }

    /// <summary>
    /// Get with deserialized Json response.
    /// </summary>
    /// <param name="route"></param>
    /// <typeparam name="TResponse">Response body model.</typeparam>
    /// <returns></returns>
    protected async Task<TResponse?> GetAsync<TResponse>(string route)
    {
        _logger.LogInformation($"{ApiName} GET: {_httpClient.BaseAddress}{RouteForLogging(route)}");
        var httpResponse = await _httpClient.GetAsync(route).ConfigureAwait(false);

        return await HandleResponseAsync<TResponse>(httpResponse, route).ConfigureAwait(false);
    }

    /// <summary>
    /// Send a POST request and return deserialised response.
    /// </summary>
    /// <typeparam name="TRequest">The type of request to send in body.</typeparam>
    /// <typeparam name="TResponse">The type of response expected.</typeparam>
    /// <param name="route"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    protected async Task<TResponse?> PostAsync<TRequest, TResponse>(string route, TRequest? request)
    {
        _logger.LogInformation($"{ApiName} POST: {_httpClient.BaseAddress}{RouteForLogging(route)}");

        var response = await this._httpClient.PostAsJsonAsync(route, request).ConfigureAwait(false);

        return await HandleResponseAsync<TResponse>(response, route).ConfigureAwait(false);
    }

    /// <summary>
    /// Send a POST request and return deserialised response.
    /// </summary>
    /// <typeparam name="TResponse">The type of response expected.</typeparam>
    /// <param name="requestMessage"></param>
    /// <returns></returns>
    protected async Task<TResponse?> SendAsync<TResponse>(HttpRequestMessage requestMessage)
    {
        var relativeUri = RouteForLogging(requestMessage.RequestUri?.ToString());
        _logger.LogInformation($"{ApiName} SEND({requestMessage.Method}): {_httpClient.BaseAddress}{relativeUri}");

        var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);

        return await HandleResponseAsync<TResponse>(response, relativeUri).ConfigureAwait(false);
    }

    /// <summary>
    /// Create correct error response when API call was not successful.
    /// </summary>
    /// <param name="route"></param>
    /// <param name="httpResponse"></param>
    /// <returns></returns>
    protected virtual async Task<Exception> HandleUnsuccessfulResponseAsync(string route, HttpResponseMessage httpResponse)
    {
#pragma warning disable IDE0072
        return httpResponse.StatusCode switch
#pragma warning restore IDE0072
        {
            HttpStatusCode.Unauthorized => await CreateUnauthorizedExceptionAsync(route, httpResponse).ConfigureAwait(false),
            HttpStatusCode.NotFound => new NotFoundException($"Route: {RouteForLogging(route)} returned Not Found"),
            HttpStatusCode.BadRequest => await CreateBadRequestExceptionAsync(route, httpResponse).ConfigureAwait(false),
            _ => await CreateApiExceptionAsync(route, httpResponse).ConfigureAwait(false)
        };
    }

    private async Task<TResponse?> HandleResponseAsync<TResponse>(HttpResponseMessage httpResponse, string route)
    {
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw await HandleUnsuccessfulResponseAsync(route, httpResponse).ConfigureAwait(false);
        }

        if (httpResponse.StatusCode == HttpStatusCode.NoContent)
        {
            return default;
        }

        return await httpResponse.Content.ReadFromJsonAsync<TResponse>(SerializerOptions).ConfigureAwait(false);
    }

    private async Task<string> GetAndLogErrorAsync(string route, HttpResponseMessage httpResponse)
    {
        var errorDetailsText = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        var errorDetails = string.Empty;

        try
        {
            var problem = JsonSerializer.Deserialize<TErrorResponse>(errorDetailsText, SerializerOptions);

            if (problem != null)
            {
                errorDetails = problem.GetError();
            }

            if (string.IsNullOrEmpty(errorDetails))
            {
                errorDetails = errorDetailsText;
            }
        }
        catch (JsonException)
        {
            errorDetails = errorDetailsText;
        }

        var fullError = $"{ApiName} - Route: {RouteForLogging(route)} returned error with code {httpResponse.StatusCode}. {errorDetails}".Trim();

        _logger.LogError($"Error when calling API: {fullError}");

        return fullError;
    }

    private async Task<BadRequestException> CreateBadRequestExceptionAsync(string route, HttpResponseMessage httpResponse)
    {
        var message = await GetAndLogErrorAsync(route, httpResponse).ConfigureAwait(false);
        return new BadRequestException(message);
    }

    private async Task<ApiException> CreateApiExceptionAsync(string route, HttpResponseMessage httpResponse)
    {
        var message = await GetAndLogErrorAsync(route, httpResponse).ConfigureAwait(false);
        return new ApiException(message);
    }

    private async Task<UnauthorizedException> CreateUnauthorizedExceptionAsync(string route, HttpResponseMessage httpResponse)
    {
        var message = await GetAndLogErrorAsync(route, httpResponse).ConfigureAwait(false);
        return new UnauthorizedException(message);
    }
}
