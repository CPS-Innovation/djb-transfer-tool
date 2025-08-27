// <copyright file="CaseCenterApiClient.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.Clients;

using System.Diagnostics;
using System.Net;
using System.Text;
using Cps.Fct.Djb.TransferTool.Shared.Constants;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Clients.Interfaces;
using Cps.Fct.Djb.TransferToolApi.ApiClients.ConfigOptions;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Constants;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Auth;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;
using Microsoft;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

/// <summary>
/// Class for interacting with the case center admin api.
/// </summary>
public class CaseCenterApiClient : ICaseCenterApiClient
{
    private readonly ILogger<CaseCenterApiClient> logger;
    private readonly HttpClient httpClient;
    private readonly ClientEndpointOptions clientEndpointOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="CaseCenterApiClient"/> class.
    /// </summary>
    /// <param name="logger">ILogger.</param>
    /// <param name="httpClient">HttpClient.</param>
    /// <param name="options">ClientEndpointOptions.</param>
    public CaseCenterApiClient(
        ILogger<CaseCenterApiClient> logger,
        HttpClient httpClient,
        ClientEndpointOptions options)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.clientEndpointOptions = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Gets an authentication token from the Case Center API using the provided user credentials.
    /// </summary>
    /// <returns>A HttpReturnResultDto with the authentication token as a payload.</returns>
    public async Task<HttpReturnResultDto<string>> GetAuthTokenAsync()
    {
        try
        {
            var sw = Stopwatch.StartNew();
            this.logger.LogInformation($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(this.GetAuthTokenAsync)}: starting.");

            var username = this.clientEndpointOptions.CredentialName ?? string.Empty;
            var password = this.clientEndpointOptions.CredentialPassword ?? string.Empty;

            Requires.NotNull(username);
            Requires.NotNull(password);

            var userCredentialsDto = new UserCredentialsDto()
            {
                Username = username,
                Password = password,
            };

            var path = this.clientEndpointOptions.RelativePath[CaseCenterConfigConstants.CaseCenterApiGetTokenPathName];

            var json = JsonConvert.SerializeObject(userCredentialsDto);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await this.httpClient.PostAsync(path, content).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                return HttpReturnResultDto<string>.Fail(response.StatusCode, response.ReasonPhrase);
            }

            var token = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(token))
            {
                return HttpReturnResultDto<string>.Fail(HttpStatusCode.InternalServerError, "No auth token returned");
            }

            this.logger.LogInformation($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(this.GetAuthTokenAsync)}: completed in [{sw.Elapsed}].");
            return HttpReturnResultDto<string>.Success(response.StatusCode, token);
        }
        catch (Exception ex)
        {
            var message = $"Case Center API client encountered an error: {ex.Message}";
            this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(this.GetAuthTokenAsync)}: {message}");
            return HttpReturnResultDto<string>.Fail(HttpStatusCode.InternalServerError, message);
        }
    }
}
