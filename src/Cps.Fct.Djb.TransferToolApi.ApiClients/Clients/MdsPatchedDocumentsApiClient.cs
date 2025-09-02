// <copyright file="MdsPatchedDocumentsApiClient.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.Clients;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net;

using System.Text;
using Cps.MasterDataService.Infrastructure.ApiClient;
using System.Net.Http;
using System.Threading;

/// <summary>
/// Mds Patched documents client.
/// </summary>
public class MdsPatchedDocumentsApiClient : MdsApiClient, IMdsApiClient
{
    private readonly HttpClient httpClient;

    public MdsPatchedDocumentsApiClient(HttpClient httpClient) : base(httpClient)
    {
        this.httpClient = httpClient;
    }

    public override async Task<Stream> GetMaterialDocumentAsync(int caseId, string doubleEncodedFilePath, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(doubleEncodedFilePath))
        {
            throw new ArgumentNullException(nameof(doubleEncodedFilePath));
        }

        var urlBuilder = new StringBuilder();
        if (!string.IsNullOrEmpty(BaseUrl))
        {
            urlBuilder.Append(BaseUrl.TrimEnd('/'));
        }
        urlBuilder.Append("/cases/");
        urlBuilder.Append(Uri.EscapeDataString(caseId.ToString(CultureInfo.InvariantCulture)));
        urlBuilder.Append("/material-document/");
        urlBuilder.Append(doubleEncodedFilePath);

        // Remove 'using' here - don't dispose yet
        var request = new HttpRequestMessage(HttpMethod.Get, urlBuilder.ToString());
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));

        // Remove 'using' here - don't dispose yet
        var response = await this.httpClient
            .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct)
            .ConfigureAwait(false);

        var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
        if (response.Content?.Headers != null)
        {
            foreach (var item in response.Content.Headers)
            {
                headers[item.Key] = item.Value;
            }
        }

        var status = (int)response.StatusCode;
        if (status == 200)
        {
            try
            {
                var contentStream = response.Content != null
                    ? await response.Content.ReadAsStreamAsync(ct).ConfigureAwait(false)
                    : null;

                if (contentStream == null)
                {
                    response.Dispose();
                    request.Dispose();
                    throw new ApiException("Response was null which was not expected.",
                        status, string.Empty, headers, null);
                }

                // Copy to MemoryStream so we can dispose the HTTP objects
                var memoryStream = new MemoryStream();
                await contentStream.CopyToAsync(memoryStream, ct).ConfigureAwait(false);
                memoryStream.Position = 0;

                // Now we can safely dispose
                response.Dispose();
                request.Dispose();

                return memoryStream; // This IS System.IO.Stream
            }
            catch (Exception ex)
            {
                response.Dispose();
                request.Dispose();
                throw;
            }
        }
        else
        {
            response.Dispose();
            request.Dispose();
            // ... your existing error handling ...
            throw new ApiException($"Unexpected status code: {status}", status, string.Empty, headers, null);
        }
    }
}
