// <copyright file="MdsApiClientFactory.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.Factories;

using Cps.Fct.Djb.TransferToolApi.ApiClients.Factories.Interfaces;
using Cps.MasterDataService.Infrastructure.ApiClient;
using Microsoft.Extensions.Configuration;

/// <inheritdoc />
public class MdsApiClientFactory : IMdsApiClientFactory
{
    private readonly IConfiguration configuration;
    private readonly IHttpClientFactory httpClientFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="MdsApiClientFactory"/> class.
    /// </summary>
    /// <param name="configuration">configuration.</param>
    /// <param name="httpClientFactory">http client factory.</param>
    public MdsApiClientFactory(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        this.configuration = configuration;
        this.httpClientFactory = httpClientFactory;
    }

    /// <inheritdoc />
    public IMdsApiClient Create(string cookieHeader)
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
