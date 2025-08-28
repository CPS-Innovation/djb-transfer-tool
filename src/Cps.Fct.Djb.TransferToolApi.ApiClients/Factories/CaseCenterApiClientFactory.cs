// <copyright file="CaseCenterApiClientFactory.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.Factories;

using System.Net.Http.Headers;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Clients;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Clients.Interfaces;
using Cps.Fct.Djb.TransferToolApi.ApiClients.ConfigOptions;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Constants;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Factories.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <inheritdoc />
public class CaseCenterApiClientFactory : ICaseCenterApiClientFactory
{
    private readonly ILogger<CaseCenterApiClientFactory> logger;
    private readonly ILoggerFactory loggerFactory;
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ClientEndpointOptions clientOptions;
    private readonly CaseCenterOptions caseCenterOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="CaseCenterApiClientFactory"/> class.
    /// </summary>
    /// <param name="logger">logger.</param>
    /// <param name="loggerFactory">logger factory.</param>
    /// <param name="httpClientFactory">http client factory.</param>
    /// <param name="clientOptionsMonitor">client options.</param>
    /// <param name="caseCenterOptionsMonitor">case center options.</param>
    public CaseCenterApiClientFactory(
        ILogger<CaseCenterApiClientFactory> logger,
        ILoggerFactory loggerFactory,
        IHttpClientFactory httpClientFactory,
        IOptionsMonitor<ClientEndpointOptions> clientOptionsMonitor,
        IOptionsMonitor<CaseCenterOptions> caseCenterOptionsMonitor)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

        this.clientOptions = clientOptionsMonitor.Get(CaseCenterConfigConstants.CaseCenterApiClientConfigurationName)
            ?? throw new InvalidOperationException("Missing Case Center client options.");

        this.caseCenterOptions = caseCenterOptionsMonitor.CurrentValue
            ?? throw new InvalidOperationException("Missing Case Center options.");
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ICaseCenterApiClient"/> with the specified authentication token.
    /// </summary>
    /// <param name="authenticationToken">optional authentication token. Is needed for all other calls except authenticate on the CaseCenterApiClient.</param>
    /// <returns>ICaseCenterApiClient.</returns>
    public ICaseCenterApiClient Create(string? authenticationToken = "")
    {
        if (this.clientOptions.BaseAddress is null || !this.clientOptions.BaseAddress.IsAbsoluteUri)
        {
            throw new InvalidOperationException("Missing or invalid Case Center base address in configuration.");
        }

        if (this.clientOptions.Timeout <= TimeSpan.Zero)
        {
            throw new InvalidOperationException("Invalid HTTP client timeout in configuration.");
        }

        var httpClient = this.httpClientFactory.CreateClient(CaseCenterConfigConstants.CaseCenterApiClientConfigurationName);

        httpClient.BaseAddress = this.clientOptions.BaseAddress;
        httpClient.Timeout = this.clientOptions.Timeout;

        httpClient.DefaultRequestHeaders.Remove("Bearer");
        if (!string.IsNullOrWhiteSpace(authenticationToken))
        {
            httpClient.DefaultRequestHeaders.Add("Bearer", authenticationToken);
        }

        var json = new MediaTypeWithQualityHeaderValue("application/json");
        if (!httpClient.DefaultRequestHeaders.Accept.Contains(json))
        {
            httpClient.DefaultRequestHeaders.Accept.Add(json);
        }

        return new CaseCenterApiClient(
                        this.loggerFactory.CreateLogger<CaseCenterApiClient>(),
                        httpClient,
                        this.clientOptions,
                        this.caseCenterOptions);
    }
}
