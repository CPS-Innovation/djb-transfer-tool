// <copyright file="MdsApiClientFactory.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.Factories;

using Cps.Fct.Djb.TransferToolApi.ApiClients.Clients.Interfaces;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Constants;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Factories.Interfaces;
using Cps.Fct.Hk.Common.DDEI.Client.Configuration;
using Cps.MasterDataService.Infrastructure.ApiClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <inheritdoc />
public class CaseCenterApiClientFactory : ICaseCenterApiClientFactory
{
    private readonly ILogger<CaseCenterApiClientFactory> logger;
    private readonly IHttpClientFactory httpClientFactory;
    protected readonly ClientEndpointOptions clientOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="CaseCenterApiClientFactory"/> class.
    /// </summary>
    /// <param name="logger">logger.</param>
    /// <param name="httpClientFactory">http client factory.</param>
    /// <param name="clientOptions">client options.</param>
    public CaseCenterApiClientFactory(
        ILogger<CaseCenterApiClientFactory> logger,
        IHttpClientFactory httpClientFactory,
        IOptionsMonitor<ClientEndpointOptions> clientOptions)
    {
        this.logger = logger;
        this.httpClientFactory = httpClientFactory;
        this.clientOptions = clientOptions.Get(CaseCenterConfigConstants.CaseCenterApiClientConfigurationName);
    }

    /// <inheritdoc />
    public ICaseCenterApiClient Create(string cookieHeader)
    {
        if (string.IsNullOrWhiteSpace(cookieHeader))
        {
            throw new ArgumentNullException(nameof(cookieHeader), "Cookie header is required.");
        }

        var functionKey = this.configuration.GetValue<string>("DDEIClient:FunctionKey");
        if (string.IsNullOrWhiteSpace(functionKey))
        {
            throw new InvalidOperationException("Missing MDS function key in configuration.");
        }

        var baseUrl = this.configuration.GetValue<string>("DDEIClient:BaseAddress");
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new InvalidOperationException("Missing MDS base url in configuration.");
        }

        var client = this.httpClientFactory.CreateClient("MdsClient");

        client.DefaultRequestHeaders.Add("x-functions-key", functionKey);
        client.DefaultRequestHeaders.Add("Cms-Auth-Values", cookieHeader);

        return new MdsApiClient(client) { BaseUrl = baseUrl };
    }
}
