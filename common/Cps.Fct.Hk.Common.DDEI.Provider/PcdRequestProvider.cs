// <copyright file="ApiServiceClient.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.DDEI.Provider;

using System;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Cps.Fct.Hk.Common.Contracts.Logging;
using Cps.Fct.Hk.Common.DDEI.Client;
using Cps.Fct.Hk.Common.DDEI.Client.Configuration;
using Cps.Fct.Hk.Common.DDEI.Client.Model;
using Cps.Fct.Hk.Common.DDEI.Provider.Models.Request.PcdRequests;
using Cps.Fct.Hk.Common.DDEI.Provider.Models.Response.PCD;
using Microsoft;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// The DDEI API service client.
/// </summary>
public class PcdRequestProvider : IPcdRequestProvider
{
    private readonly IDdeiApiClient _ddeiServiceClient;
    private readonly ILogger<PcdRequestProvider> logger;
    private readonly ClientEndpointOptions clientOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="PcdRequestProvider"/> class.
    /// </summary>
    /// <param name="loggerFactory">The logger factory.</param>
    /// <param name="ddeiServiceClient">The DDEI client.</param>
    /// <param name="clientOptions">The client endpoint options.</param>
    public PcdRequestProvider(
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
    public async Task<PcdRequestDto> GetPcdRequestBypcdIdAsync(GetPcdRequestByPcdIdCoreRequest request, CmsAuthValues cmsAuthValues)
    {
        Requires.NotNull(request);
        Requires.NotNull(cmsAuthValues);
        Requires.NotNull(cmsAuthValues.CmsCookies, nameof(cmsAuthValues.CmsCookies));
        Requires.NotNull(cmsAuthValues.CmsModernToken, nameof(cmsAuthValues.CmsModernToken));

        if (string.IsNullOrEmpty(cmsAuthValues.CmsCookies))
        {
            throw new ArgumentException($"{LoggingConstants.HskUiLogPrefix} CMS Cookies cannot be null or empty.", nameof(request));
        }

        var stopwatch = Stopwatch.StartNew();
        const string OperationName = "PcdRequestById";
        string path = "Undefined";
        string? errorResponse = null;

        try
        {
            path = this.clientOptions.RelativePath[OperationName]
                .Replace("{caseId}", request.caseId.ToString(CultureInfo.InvariantCulture))
                .Replace("{pcdId}", request.pcdId.ToString(CultureInfo.InvariantCulture));

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, path);

            PcdRequestDto? pcdRequestInfo = await this._ddeiServiceClient.HandleHttpRequestInternalAsync<PcdRequestDto>(httpRequest, cmsAuthValues, request).ConfigureAwait(false);

            this._ddeiServiceClient.LogOperationCompletedEvent(OperationName, path, request, stopwatch.Elapsed, $"caseId [{request.caseId}]");
            return pcdRequestInfo;
        }
        catch (Exception exception)
        {
            this._ddeiServiceClient.HandleException(OperationName, path, errorResponse, exception, request, stopwatch.Elapsed);
            throw;
        }
    }
}
