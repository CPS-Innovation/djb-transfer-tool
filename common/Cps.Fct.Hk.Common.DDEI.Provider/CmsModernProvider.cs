// <copyright file="ApiServiceClient.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.DDEI.Provider;

using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Cps.Fct.Hk.Common.Contracts.Logging;
using Cps.Fct.Hk.Common.DDEI.Client;
using Cps.Fct.Hk.Common.DDEI.Client.Configuration;
using Cps.Fct.Hk.Common.DDEI.Client.Model;
using Cps.Fct.Hk.Common.DDEI.Client.Model.Requests;
using Microsoft;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// The DDEI API service client.
/// </summary>
public class CmsModernProvider : ICmsModernProvider
{
    private readonly IDdeiApiClient _ddeiServiceClient;
    private readonly ILogger<PcdRequestProvider> logger;
    private readonly ClientEndpointOptions clientOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="CmsModernProvider"/> class.
    /// </summary>
    /// <param name="ddeiServiceClient"></param>
    /// <param name="loggerFactory"></param>
    /// <param name="clientOptions"></param>
    public CmsModernProvider(
        IDdeiApiClient ddeiServiceClient,
        ILoggerFactory loggerFactory,
        IOptions<ClientEndpointOptions> clientOptions)
    {
        Requires.NotNull(ddeiServiceClient);
        Requires.NotNull(loggerFactory);
        Requires.NotNull(clientOptions);

        this.logger = loggerFactory.CreateLogger<PcdRequestProvider>();
        this._ddeiServiceClient = ddeiServiceClient;
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

            var data = await _ddeiServiceClient.HandleHttpRequestInternalAsync<CmsModernTokenResult>(httpRequest, request.CmsAuthValues, new BaseRequest(request.CorrespondenceId)).ConfigureAwait(false);
            if (data?.CmsModernToken is not null)
            {
                results = data.CmsModernToken.ToString();
            }

            this._ddeiServiceClient.LogOperationCompletedEvent(OperationName, path, request, stopwatch.Elapsed, "");
        }
        catch (Exception exception)
        {
            this._ddeiServiceClient.HandleException(OperationName, path, errorResponse, exception, request, stopwatch.Elapsed);
            throw;
        }

        return results;
    }
}
