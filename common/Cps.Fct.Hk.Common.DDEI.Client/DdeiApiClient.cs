// <copyright file="ApiServiceClient.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.DDEI.Client;

using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using Cps.Fct.Hk.Common.Contracts.Logging;
using Cps.Fct.Hk.Common.DDEI.Client.Model;
using Cps.Fct.Hk.Common.DDEI.Client.Model.Requests;
using Cps.Fct.Hk.Common.DDEI.Client.Configuration.Extensions;
using Cps.Fct.Hk.Common.DDEI.Client.Configuration;
using Cps.Fct.Hk.Common.DDEI.Client.Diagnostics;

/// <summary>
/// The DDEI API service client.
/// </summary>
public class DdeiApiClient : IDdeiApiClient
{
    private readonly ILogger<DdeiApiClient> logger;
    private readonly IHttpClientFactory clientFactory;
    private readonly ClientEndpointOptions clientOptions;
    private readonly object sync = new();
    private AuthenticationHeader? authenticationHeader;

    /// <summary>
    /// Initializes a new instance of the <see cref="DdeiApiClient"/> class.
    /// </summary>
    /// <param name="loggerFactory">The logger factory.</param>
    /// <param name="clientFactory">The HTTP client factory.</param>
    /// <param name="clientOptions">The client endpoint options.</param>
    public DdeiApiClient(
        ILoggerFactory loggerFactory,
        IHttpClientFactory clientFactory,
        IOptions<ClientEndpointOptions> clientOptions)
    {
        Requires.NotNull(loggerFactory);
        Requires.NotNull(clientFactory);
        Requires.NotNull(clientOptions);

        this.logger = loggerFactory.CreateLogger<DdeiApiClient>();
        this.clientFactory = clientFactory;
        this.clientOptions = clientOptions.Value;
    }

    /// <inheritdoc/>
    public async Task<string?> GetCmsModernTokenAsync(GetCmsModernTokenRequest request)
    {
        Requires.NotNull(request);
        Requires.NotNull(request.CmsAuthValues);

        var stopwatch = Stopwatch.StartNew();
        const string OperationName = "GetCmsModernToken";
        string? results = null;
        string path = "Undefined";
        string? errorResponse = null;

        try
        {
            path = this.clientOptions.RelativePath[OperationName];
            HttpRequestMessage httpRequest = new(method: HttpMethod.Get, requestUri: path);
            if (string.IsNullOrEmpty(request.CmsAuthValues.CmsCookies))
            {
                throw new ArgumentException($"{LoggingConstants.HskUiLogPrefix} CMS Cookies cannot be null or empty.", nameof(request));
            }

            var data = await HandleHttpRequestInternalAsync<CmsModernTokenResult>(httpRequest, request.CmsAuthValues, new BaseRequest(request.CorrespondenceId)).ConfigureAwait(false);
            if (data?.CmsModernToken is not null)
            {
                results = data.CmsModernToken.ToString();
            }

            this.LogOperationCompletedEvent(OperationName, path, request, stopwatch.Elapsed, "");
        }
        catch (Exception exception)
        {
            this.HandleException(OperationName, path, errorResponse, exception, request, stopwatch.Elapsed);
            throw;
        }

        return results;
    }

    /// <summary>
    /// Handles an exception that occurred while calling the DDEI API.
    /// </summary>
    /// <param name="operationName">The operation name.</param>
    /// <param name="path">The relative path of the API call.</param>
    /// <param name="errorResponse">An error response from DDEI API if one is received.</param>
    /// <param name="exception">The exception to handle.</param>
    /// <param name="request">The request with a correspondence ID.</param>
    /// <param name="duration">The duration of the operation.</param>
    public void HandleException(
        string operationName,
        string path,
        string? errorResponse,
        Exception exception,
        BaseRequest request,
        TimeSpan duration)
    {
        Requires.NotNull(operationName);
        Requires.NotNull(path);
        Requires.NotNull(exception);

        const string LogMessage = DiagnosticsUtility.Error + @"Calling the DDEI-EAS API failed for {Operation} after {Duration}. Path: {Path}, Correspondence ID: {CorrespondenceId}, Failure: {Reason}
 - Failure response: {FailureResponse}";
        this.logger.LogError(
            exception,
            LoggingConstants.HskUiLogPrefix + " " + LogMessage,
            operationName,
            duration,
            path,
            request?.CorrespondenceId,
            exception.ToAggregatedMessage(),
            errorResponse);
    }

    /// <summary>
    /// Logs an operation completed event.
    /// </summary>
    /// <param name="operationName">The operation name.</param>
    /// <param name="path">The relative path of the API call.</param>
    /// <param name="request">The request with a correspondence ID.</param>
    /// <param name="duration">The duration of the operation.</param>
    /// <param name="additionalInfo">Any additional information.</param>
    public void LogOperationCompletedEvent(
        string operationName,
        string path,
        BaseRequest request,
        TimeSpan duration,
        string additionalInfo)
    {
        const string LogMessage = @"Calling the DDEI-EAS API succeeded for {Operation} after {Duration}. Path: {Path}, Correspondence ID: {CorrespondenceId}
 - Additional info: {AdditionalInfo}";

        this.logger.LogInformation(
            LoggingConstants.HskUiLogPrefix + " " + LogMessage,
            operationName,
            duration,
            path,
            request.CorrespondenceId,
            additionalInfo);
    }

    /// <summary>
    /// Ensures that the necessary authentication headers are added to the specified HTTP request message.
    /// </summary>
    /// <param name="httpRequest">The HTTP request message to which the authentication headers will be added.</param>
    /// <param name="cmsCookies">The CMS cookies used for authentication.</param>
    /// <param name="correspondenceId">The unique identifier for the correspondence associated with the request.</param>
    /// <param name="cmsToken">An optional CMS token used for authentication. If not provided, a new token will be generated if specified.</param>
    /// <param name="generateTokenForCmsModernTokenRetrieval">Indicates whether to generate a new token for CMS Modern token retrieval. Defaults to false.</param>
    public virtual void EnsureAuthenticationHeaders(
        HttpRequestMessage httpRequest,
        string cmsCookies,
        Guid correspondenceId,
        string? cmsToken = null,
        bool generateTokenForCmsModernTokenRetrieval = false)
    {
        if (string.IsNullOrWhiteSpace(cmsCookies))
        {
            this.logger.LogError($"{LoggingConstants.HskUiLogPrefix} CmsCookies is null or empty");
            throw new ArgumentException($"{LoggingConstants.HskUiLogPrefix} CmsCookies is null or empty");
            //return;
        }

        // The special characters encoded as follows:
        // = becomes %3D
        // ; becomes %3B
        // Spaces become %20

        string? encodedCc = cmsCookies;
        string? encodedCt = generateTokenForCmsModernTokenRetrieval ? Guid.NewGuid().ToString() : cmsToken;

        try
        {
            string decodedCc = Uri.UnescapeDataString(encodedCc);
            string decodedCt = Uri.UnescapeDataString(encodedCt ?? string.Empty);

            if (!string.IsNullOrWhiteSpace(decodedCc) && decodedCt != null)
            {
                lock (this.sync)
                {
                    if (this.authenticationHeader is not null && this.authenticationHeader.ExpiryTime < DateTimeOffset.UtcNow.AddHours(1))
                    {
                        this.authenticationHeader = null;
                    }
                    if (this.authenticationHeader is null)
                    {
                        var authenticationContext = new AuthenticationContext(decodedCc, decodedCt, DateTimeOffset.UtcNow.AddHours(1));
                        this.authenticationHeader = new AuthenticationHeader(authenticationContext);
                    }
                }
            }

            if (this.authenticationHeader is not null)
            {
                httpRequest.Headers.Add(AuthenticationHeader.AuthenticationHeaderName, this.authenticationHeader.ToString());
            }

            this.SetFunctionKeyHeader(httpRequest);
            httpRequest.Headers.Add("Correlation-Id", $"{correspondenceId}");
        }
        catch (UriFormatException ex)
        {
            this.logger.LogError(ex, $"{LoggingConstants.HskUiLogPrefix} Failed to decode URI components");
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, $"{LoggingConstants.HskUiLogPrefix} An unexpected error occurred while setting authentication headers");
        }
    }

    /// <summary>
    /// Internal method to process DDEI calls to reduce code duplication.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="httpRequest"></param>
    /// <param name="cmsAuthValues"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<TResponse> HandleHttpRequestInternalAsync<TResponse>(HttpRequestMessage httpRequest, CmsAuthValues cmsAuthValues, BaseRequest request)
    {
        Guid correspondenceId = request.CorrespondenceId != Guid.Empty ? request.CorrespondenceId : default;
        HttpClient client = this.clientFactory.CreateClient(ClientEndpointOptions.DefaultSectionName);
        this.EnsureAuthenticationHeaders(
            httpRequest,
            cmsAuthValues.CmsCookies,
            request.CorrespondenceId,
            cmsAuthValues.CmsModernToken);

        HttpResponseMessage httpResponse = await client.SendAsync(httpRequest).ConfigureAwait(false);
        string? errorResponse = await DetermineDdeiErrorResponse(httpResponse).ConfigureAwait(false);

        if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException(errorResponse);
        }

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new HttpRequestException(errorResponse);
        }

        httpResponse.EnsureSuccessStatusCode();
        string content = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

        if (content.Contains("Invalid Cms Auth Values. It may be that you do not have CMS cookies", StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException(errorResponse);
        }

        try
        {
            TResponse? pcdRequestInfo = JsonSerializer.Deserialize<TResponse>(content, ApplicationOptions.ApplicationSerializerOptions);

            return pcdRequestInfo == null
                ? throw new InvalidOperationException($"{LoggingConstants.HskUiLogPrefix} Failed to deserialize {nameof(TResponse)}. JSON content: {content}")
                : pcdRequestInfo;
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"{LoggingConstants.HskUiLogPrefix} Failed to deserialize {nameof(TResponse)} due to JSON exception.", ex);
        }
    }

    /// <summary>
    /// Asynchronously determines the error response from a failed HTTP request.
    /// </summary>
    /// <param name="httpResponse">The <see cref="HttpResponseMessage"/> received from an HTTP request.</param>
    /// <returns>
    /// A string containing the error response content if the request was not successful; otherwise, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// This method checks if the HTTP response indicates a failure. If the status code indicates an error, the response content is read and returned as a string.
    /// Otherwise, the method returns <c>null</c>.
    /// </remarks>
    private static async Task<string?> DetermineDdeiErrorResponse(HttpResponseMessage httpResponse)
    {
        string? errorResponse = null;
        if (!httpResponse.IsSuccessStatusCode)
        {
            errorResponse = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        return errorResponse;
    }

    /// <summary>
    /// Sets the function key header on the specified HTTP request.
    /// </summary>
    /// <param name="httpRequest">The <see cref="HttpRequestMessage"/> to which the function key header will be added.</param>
    /// <remarks>
    /// This method adds the `x-functions-key` header to the provided HTTP request using the function key stored in the client options. 
    /// If the function key is not null or whitespace and is longer than seven characters, a sanitised version of the key (showing only the last seven characters) is logged for security purposes.
    /// </remarks>
    private void SetFunctionKeyHeader(HttpRequestMessage httpRequest)
    {
        const int Seven = 7;
        string sanitisedFunctionKey = "Unknown";
        if (!string.IsNullOrWhiteSpace(this.clientOptions.FunctionKey))
        {
            httpRequest.Headers.Add("x-functions-key", this.clientOptions.FunctionKey);
            if (this.clientOptions.FunctionKey.Length > Seven)
            {
                // Last 7 characters
                sanitisedFunctionKey = $"...{this.clientOptions.FunctionKey[^Seven..]}";
            }
        }

        this.logger.LogInformation($"{LoggingConstants.HskUiLogPrefix} Calling the DDEI-EAS API with sanitised function key '{sanitisedFunctionKey}'");
    }
}
