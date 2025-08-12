using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cps.Fct.Hk.Common.Contracts.Exceptions;
using Cps.Fct.Hk.Common.Contracts.Models;

namespace Cps.Fct.Hk.Common.Contracts.Extensions;

/// <summary>
/// Provides extensions for the HttpClient class.
/// </summary>
public static class HttpClientExtensions
{
    private static string JsonContentType { get; } = "application/json";

    private static JsonSerializerOptions SerializerOptions { get; } = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    /// <summary>
    /// Get with deserialized Json response.
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="route"></param>
    /// <typeparam name="TResponse">Response body model.</typeparam>
    /// <returns></returns>
    public static async Task<TResponse?> GetAsync<TResponse>(this HttpClient httpClient, string route)
    {
        var httpResponse = await httpClient.GetAsync(route).ConfigureAwait(false);

        return await HandleResponseAsync<TResponse>(httpResponse, route).ConfigureAwait(false);
    }

    /// <summary>
    /// Post with no body or response body.
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="route"></param>
    /// <exception cref="ApiException">Api Exception.</exception>
    /// <exception cref="NotFoundException">Not Found Exception.</exception>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task PostAsync(this HttpClient httpClient, string route)
    {
        using var content = new StringContent(string.Empty);

        var httpResponse = await httpClient.PostAsync(route, content).ConfigureAwait(false); // could potentially send null?

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw await HandleUnsuccessfulResponseAsync(route, httpResponse).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Post with JSON serialized request body, no response body.
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="route"></param>
    /// <param name="requestModel"></param>
    /// <typeparam name="TRequest">Request body model.</typeparam>
    /// <exception cref="ApiException">Api Exception.</exception>
    /// <exception cref="NotFoundException">Not Found Exception.</exception>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task<HttpResponseMessage> PostAsync<TRequest>(this HttpClient httpClient, string route, TRequest requestModel)
    {
        var json = JsonSerializer.Serialize(requestModel, SerializerOptions);
        using var content = new StringContent(json, Encoding.UTF8, JsonContentType);

        var httpResponse = await httpClient.PostAsync(route, content).ConfigureAwait(false);

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw await HandleUnsuccessfulResponseAsync(route, httpResponse).ConfigureAwait(false);
        }

        return httpResponse;
    }

    /// <summary>
    /// Post with JSON serialized request body and response body.
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="route"></param>
    /// <param name="requestModel"></param>
    /// <typeparam name="TRequest">Request body model.</typeparam>
    /// <typeparam name="TResponse">Response body model.</typeparam>
    /// <returns></returns>
    public static async Task<TResponse?> PostAsync<TRequest, TResponse>(this HttpClient httpClient, string route, TRequest requestModel)
    {
        var json = JsonSerializer.Serialize(requestModel, SerializerOptions);
        using var content = new StringContent(json, Encoding.UTF8, JsonContentType);

        var httpResponse = await httpClient.PostAsync(route, content).ConfigureAwait(false);

        return await HandleResponseAsync<TResponse>(httpResponse, route).ConfigureAwait(false);
    }

    /// <summary>
    /// Patch with JSON serialized request body, no response body.
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="route"></param>
    /// <param name="requestModel"></param>
    /// <typeparam name="TRequest">Request body model.</typeparam>
    /// <exception cref="ApiException">Api Exception.</exception>
    /// <exception cref="NotFoundException">Not Found Exception.</exception>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task<HttpResponseMessage> PatchAsync<TRequest>(this HttpClient httpClient, string route, TRequest requestModel)
    {
        var json = JsonSerializer.Serialize(requestModel, SerializerOptions);
        using var content = new StringContent(json, Encoding.UTF8, JsonContentType);

        var httpResponse = await httpClient.PatchAsync(route, content).ConfigureAwait(false);

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw await HandleUnsuccessfulResponseAsync(route, httpResponse).ConfigureAwait(false);
        }

        return httpResponse;
    }

    /// <summary>
    /// Put with JSON serialized request body, no response body.
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="route"></param>
    /// <param name="requestModel"></param>
    /// <typeparam name="TRequest">Request body model.</typeparam>
    /// <exception cref="ApiException">Api Exception.</exception>
    /// <exception cref="NotFoundException">Not Found Exception.</exception>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task<HttpResponseMessage> PutAsync<TRequest>(this HttpClient httpClient, string route, TRequest requestModel)
    {
        var json = JsonSerializer.Serialize(requestModel, SerializerOptions);
        using var content = new StringContent(json, Encoding.UTF8, JsonContentType);

        var httpResponse = await httpClient.PutAsync(route, content).ConfigureAwait(false);

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw await HandleUnsuccessfulResponseAsync(route, httpResponse).ConfigureAwait(false);
        }

        return httpResponse;
    }

    /// <summary>
    /// Put with JSON serialized request body and response body.
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="route"></param>
    /// <param name="requestModel"></param>
    /// <typeparam name="TRequest">Request body model.</typeparam>
    /// <typeparam name="TResponse">Response body model.</typeparam>
    /// <returns></returns>
    public static async Task<TResponse?> PutAsync<TRequest, TResponse>(this HttpClient httpClient, string route, TRequest requestModel)
    {
        var httpResponse = await httpClient.PutAsync(route, requestModel).ConfigureAwait(false);

        return await HandleResponseAsync<TResponse>(httpResponse, route).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates and returns header entity.
    /// </summary>
    /// <typeparam name="TRequest">Request body model.</typeparam>
    /// <param name="httpClient"></param>
    /// <param name="url"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public static async Task<string> CreateEntityAsync<TRequest>(this HttpClient httpClient, string url, TRequest request)
    {
        var response = await httpClient.PostAsync(url, request).ConfigureAwait(false);

        if (response.StatusCode != HttpStatusCode.Created)
        {
            throw new ApiException($"Unexpected Status code received: {response.StatusCode}");
        }

        var location = response.Headers.Location;
        return location is null
            ? throw new ApiException("Location header is missing from the response")
            : location.ToString().Split('/').Last();
    }

    /// <summary>
    /// Handle an Unsuccessful Response.
    /// </summary>
    /// <param name="route"></param>
    /// <param name="httpResponse"></param>
    private static async Task<Exception> HandleUnsuccessfulResponseAsync(string route, HttpResponseMessage httpResponse)
    {
#pragma warning disable IDE0072
        return httpResponse.StatusCode switch
#pragma warning restore IDE0072
        {
            HttpStatusCode.NotFound => new NotFoundException($"Route: {route} returned Not Found"),
            HttpStatusCode.BadRequest => await CreateBadRequestExceptionAsync(route, httpResponse).ConfigureAwait(false),
            _ => new ApiException(
                $"Response for requested URI: {route} did not return successfully. Status Code: {httpResponse.StatusCode}")
        };
    }

    /// <summary>
    /// Create BadRequestException from <paramref name="httpResponse"/> content using ValidationProblemDetails,
    /// and fall back to string if deserialization fails.
    /// </summary>
    /// <param name="route"></param>
    /// <param name="httpResponse"></param>
    /// <returns></returns>
    private static async Task<BadRequestException> CreateBadRequestExceptionAsync(string route, HttpResponseMessage httpResponse)
    {
        var errorDetailsText = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        var errorDetails = string.Empty;

        try
        {
            var validationDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(errorDetailsText, SerializerOptions);
            if (validationDetails != null)
            {
                errorDetails = $"{validationDetails.Title}: {string.Join(Environment.NewLine, validationDetails.Errors.SelectMany(e => e.Value))}";
            }
        }
        catch (JsonException)
        {
            errorDetails = errorDetailsText;
        }

        var details = $"Route: {route} returned Bad Request. {errorDetails}".Trim();

        return new BadRequestException(details);
    }

    private static async Task<TResponse?> HandleResponseAsync<TResponse>(HttpResponseMessage httpResponse, string route)
    {
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw await HandleUnsuccessfulResponseAsync(route, httpResponse).ConfigureAwait(false);
        }

        if (httpResponse.StatusCode == HttpStatusCode.NoContent)
        {
            return default;
        }

        var result = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

        return JsonSerializer.Deserialize<TResponse>(result, SerializerOptions)!;
    }
}
