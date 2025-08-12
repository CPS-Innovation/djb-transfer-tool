namespace Cps.Fct.Hk.Common.Functions.Abstractions;

/// <summary>
/// Responsible for deserializing the request bodies and serializing the response bodies.
/// </summary>
public interface IHttpDataSerializer
{
    /// <summary>
    /// Deserializes the request body.
    /// </summary>
    /// <typeparam name="TRequestBody">
    /// Source type.
    /// </typeparam>
    /// <param name="request">
    /// The request data from the function.
    /// </param>
    /// <returns>Deserialized json body as target type.</returns>
    Task<TRequestBody> ReadRequestBodyAsync<TRequestBody>(HttpRequestData request);

    /// <summary>
    /// Serializes the response body.
    /// </summary>
    /// <typeparam name="TResponseBody">
    /// Destination type.
    /// </typeparam>
    /// <param name="response">
    /// The response data for the function.
    /// </param>
    /// <param name="responseBody">The body for the response data to include.</param>
    /// <returns>Response body.</returns>
    Task WriteResponseBodyAsync<TResponseBody>(HttpResponseData response, TResponseBody responseBody);
}
