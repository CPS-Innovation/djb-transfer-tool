using System.Net;
using System.Text.Json;
using Cps.Fct.Hk.Common.Contracts.Models;
using Cps.Fct.Hk.Common.Functions.Abstractions;
using Cps.Fct.Hk.Common.Functions.Extensions;
using Microsoft.Net.Http.Headers;

namespace Cps.Fct.Hk.Common.Functions.Extensions;

/// <summary>
/// Extensions to help with HttpRequests.
/// </summary>
public static class HttpRequestExtensions
{
    /// <summary>
    /// Deserializes the request body into the provided type.
    /// </summary>
    /// <typeparam name="TValue">Type to deserialize into.</typeparam>
    /// <param name="request"></param>
    /// <returns></returns>
    public static async Task<TValue> BodyToAsync<TValue>(this HttpRequestData request)
    {
        request.Body.Position = 0;
        var value = await JsonSerializer.DeserializeAsync<TValue>(
            request.Body,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).ConfigureAwait(false);

        return value!;
    }

    /// <summary>
    /// Serializes the provided type and creates a response object.
    /// </summary>
    /// <typeparam name="T">Type to be serialized.</typeparam>
    /// <param name="request"></param>
    /// <param name="internalServerError"></param>
    /// <param name="dataSerializer"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static async Task<HttpResponseData> CreateResponseFromAsync<T>(
        this HttpRequestData request, HttpStatusCode internalServerError, IHttpDataSerializer dataSerializer, Result<T> result)
    {
        var response = request.CreateResponse();

        await response.WriteBodyContentAsync(dataSerializer, result).ConfigureAwait(false);
        response.StatusCode = result.Status;
        response.AddHeaders(result);

        return response;
    }

    /// <summary>
    /// Creates a standard http response data instance.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="httpStatusCode">Status code of response.</param>
    /// <param name="contentType">Content type of response.</param>
    /// <param name="message">Message of response.</param>
    /// <returns>HttpResponseData instance.</returns>
    public static HttpResponseData CreateResponse(
        this HttpRequestData request,
        HttpStatusCode httpStatusCode,
        string contentType,
        string message)
    {
        var response = request.CreateResponse(httpStatusCode);
        response.Headers.Add("Content-Type", contentType);
        response.WriteString(message);
        return response;
    }

    private static void AddHeaders(
        this HttpResponseData responseData, IResult result)
    {
        switch (result.Status)
        {
            case HttpStatusCode.Created:
                if (result.CustomState is Uri uri)
                {
                    responseData.Headers.Add(HeaderNames.Location, uri.ToString());
                }

                break;
        }
    }

    private static async Task WriteBodyContentAsync<T>(
        this HttpResponseData response, IHttpDataSerializer dataSerializer, Result<T> result)
    {
        var bodyContent = typeof(T) == typeof(Unit)
            ? result.CustomState
            : result.Value ?? result.CustomState;

        if (bodyContent == null)
        {
            return;
        }

        await dataSerializer.WriteResponseBodyAsync(response, bodyContent).ConfigureAwait(false);
    }
}
