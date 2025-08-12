using System.Diagnostics.CodeAnalysis;
using Azure.Core.Serialization;
using Newtonsoft.Json;

namespace Cps.Fct.Hk.Common.Functions.Abstractions.Implementations;

/// <inheritdoc />
[ExcludeFromCodeCoverage]
internal sealed class HttpDataSerializer : IHttpDataSerializer
{
    private readonly ObjectSerializer _objectSerializer;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpDataSerializer"/> class.
    /// </summary>
    /// <param name="objectSerializer"></param>
    public HttpDataSerializer(ObjectSerializer objectSerializer)
    {
        _objectSerializer = objectSerializer;
    }

    /// <inheritdoc/>
    public async Task<TRequestBody> ReadRequestBodyAsync<TRequestBody>(HttpRequestData request)
    {
        return await request.ReadFromJsonAsync<TRequestBody>(_objectSerializer).ConfigureAwait(false)
            ?? throw new JsonSerializationException("Could not deserialize request body.");
    }

    /// <inheritdoc/>
    public async Task WriteResponseBodyAsync<TResponseBody>(HttpResponseData response, TResponseBody responseBody)
    {
        await response.WriteAsJsonAsync(responseBody, _objectSerializer).ConfigureAwait(false);
    }
}
