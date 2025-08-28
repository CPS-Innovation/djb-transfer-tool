// <copyright file="CaseCenterApiClient.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.Clients;

using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Text;
using Cps.Fct.Djb.TransferTool.Shared.Constants;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Clients.Interfaces;
using Cps.Fct.Djb.TransferToolApi.ApiClients.ConfigOptions;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Constants;
using Cps.Fct.Djb.TransferToolApi.Shared.Constants;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Auth;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Case;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;
using Cps.Fct.Djb.TransferToolApi.Shared.Extensions;
using Cps.Fct.Djb.TransferToolApi.Shared.Helpers;
using Microsoft;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

/// <summary>
/// Class for interacting with the case center admin api.
/// </summary>
public class CaseCenterApiClient : ICaseCenterApiClient
{
    private readonly ILogger<CaseCenterApiClient> logger;
    private readonly HttpClient httpClient;
    private readonly ClientEndpointOptions clientEndpointOptions;
    private readonly CaseCenterOptions caseCenterOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="CaseCenterApiClient"/> class.
    /// </summary>
    /// <param name="logger">ILogger.</param>
    /// <param name="httpClient">HttpClient.</param>
    /// <param name="clientEndpointOptions">ClientEndpointOptions.</param>
    /// <param name="caseCenterOptions">CaseCenterOptions.</param>
    public CaseCenterApiClient(
        ILogger<CaseCenterApiClient> logger,
        HttpClient httpClient,
        ClientEndpointOptions clientEndpointOptions,
        CaseCenterOptions caseCenterOptions)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.clientEndpointOptions = clientEndpointOptions ?? throw new ArgumentNullException(nameof(clientEndpointOptions));
        this.caseCenterOptions = caseCenterOptions ?? throw new ArgumentNullException(nameof(caseCenterOptions));
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

    /// <summary>
    /// Creates a Case Center case based on the payload data.
    /// Returns HttpReturnResultDto with a case center case id if successful.
    ///   - id is non-null if success
    ///   - id is null if failure (then statusCode is the server's code or 500 if exception).
    /// </summary>
    /// <param name="authenticationToken">The authentication token for the operation.</param>
    /// <param name="createCaseDto">The DTO containing the case payload data.</param>
    /// <returns>
    /// A HttpResponseMessage.
    /// </returns>
    public async Task<HttpReturnResultDto<string>> CreateCaseAsync(string authenticationToken, CreateCaseDto createCaseDto)
    {
        try
        {
            var sw = Stopwatch.StartNew();
            this.logger.LogInformation($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(this.CreateCaseAsync)}: starting.");

            Requires.NotNull(this.clientEndpointOptions);
            Requires.NotNull(createCaseDto);

            Requires.NotNull(createCaseDto);
            Requires.NotNullOrWhiteSpace(authenticationToken);

            Requires.NotNull(createCaseDto.CmsCaseId);
            Requires.NotNullOrEmpty(createCaseDto.CaseCreator);
            Requires.NotNullOrEmpty(createCaseDto.CaseUrn);
            Requires.NotNullOrEmpty(createCaseDto.AreaCrownCourtCode);
            Requires.NotNullOrEmpty(createCaseDto.CaseTitle);

            var areaCaseTemplateIds = this.caseCenterOptions.AreaCaseTemplateIds;
            var organisationId = this.caseCenterOptions?.OrganisationId ?? string.Empty;
            var organisationType = this.caseCenterOptions?.OrganisationType ?? string.Empty;

            DictionaryHelper.NotNullOrEmpty(areaCaseTemplateIds);
            Requires.NotNullOrEmpty(organisationId);
            Requires.NotNullOrEmpty(organisationType);

            areaCaseTemplateIds.TryGetValue(createCaseDto.AreaCrownCourtCode, out string? templateId);

            if (string.IsNullOrWhiteSpace(templateId))
            {
                throw new KeyNotFoundException($"No template configured for court code '{createCaseDto.AreaCrownCourtCode}'.");
            }

            // FXS: Having to hard code this department id as the using the template id does not work, results in a 404 from Case Center API.
            var departmentId = "d03a553a5f5e4965849b5d57665e2e87";

            var path = string.Format(
                CultureInfo.InvariantCulture,
                this.clientEndpointOptions.RelativePath[CaseCenterConfigConstants.CaseCenterApiCreateCasePathName],
                Uri.EscapeDataString(createCaseDto?.CaseUrn ?? string.Empty),
                Uri.EscapeDataString(createCaseDto?.CaseCreator ?? string.Empty),
                Uri.EscapeDataString(organisationId ?? string.Empty),
                Uri.EscapeDataString(createCaseDto?.CaseTitle ?? string.Empty),
                Uri.EscapeDataString(templateId ?? string.Empty),
                Uri.EscapeDataString(organisationType ?? string.Empty),
                Uri.EscapeDataString(createCaseDto?.CmsCaseId.ToString(CultureInfo.InvariantCulture) ?? string.Empty),
                Uri.EscapeDataString(departmentId ?? string.Empty));

            var response = await this.SendRequestAsync<object>(
                HttpMethod.Post,
                path: path,
                apiKey: authenticationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                return HttpReturnResultDto<string>.Fail(response.StatusCode, response.ReasonPhrase);
            }

            string responsePayload = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(responsePayload))
            {
                return HttpReturnResultDto<string>.Fail(HttpStatusCode.InternalServerError, "No case center case id returned");
            }

            this.logger.LogInformation($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(this.CreateCaseAsync)}: completed in [{sw.Elapsed}].");
            return HttpReturnResultDto<string>.Success(response.StatusCode, responsePayload);
        }
        catch (Exception ex)
        {
            var message = $"Case Center API client encountered an error: {ex.Message}";
            this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(this.CreateCaseAsync)}: {message}");
            return HttpReturnResultDto<string>.Fail(HttpStatusCode.InternalServerError, message);
        }
    }

    /// <summary>
    /// Sends an HTTP request to the configured base address + specified path.
    /// If <paramref name="payload"/> is non-null, it will be serialized to JSON and sent as the request body.
    /// </summary>
    /// <typeparam name="TPayload">The type of the payload to serialize as JSON.</typeparam>
    /// <param name="method">The HTTP method (GET, POST, PUT, DELETE, etc.).</param>
    /// <param name="path">The path appended to <see cref="clientOptions.BaseAddress"/>.</param>
    /// <param name="apiKey">The auth token to be added as an API Key.</param>
    /// <param name="payload">Optional object to serialize in the request body. If null, no body is set.</param>
    /// <param name="contentType">Optional content type, defaulted to application/json to serialize in the request body.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The <see cref="HttpResponseMessage"/> returned by the server.</returns>
    private async Task<HttpResponseMessage> SendRequestAsync<TPayload>(
    HttpMethod method,
    string path,
    string apiKey = "",
    TPayload? payload = default,
    string contentType = ApiContentTypeConstants.ApplicationJson,
    CancellationToken cancellationToken = default)
    {
        var fullUri = new Uri(new Uri(this.clientEndpointOptions.BaseAddress.AbsoluteUri, UriKind.Absolute), path);
        var request = new HttpRequestMessage(method, fullUri);

        if (payload is not null)
        {
            switch (contentType)
            {
                case ApiContentTypeConstants.ApplicationJson:
                    {
                        var json = payload.ToJsonPayload();
                        request.Content = new StringContent(json, Encoding.UTF8, ApiContentTypeConstants.ApplicationJson);
                        break;
                    }

                case ApiContentTypeConstants.MultipartFormData:
                    {
                        var formData = new MultipartFormDataContent();
                        var resolver = new DefaultContractResolver();
                        var contract = resolver.ResolveContract(typeof(TPayload)) as JsonObjectContract;

                        if (contract != null)
                        {
                            foreach (var jsonProp in contract.Properties)
                            {
                                if (jsonProp.Ignored)
                                {
                                    continue;
                                }

                                if (string.IsNullOrWhiteSpace(jsonProp.UnderlyingName))
                                {
                                    continue;
                                }

                                var clrProp = typeof(TPayload).GetProperty(jsonProp.UnderlyingName, BindingFlags.Public | BindingFlags.Instance);
                                if (clrProp == null)
                                {
                                    continue;
                                }

                                var value = clrProp.GetValue(payload);
                                if (value == null)
                                {
                                    continue;
                                }

                                var name = jsonProp.PropertyName;

                                if (string.IsNullOrWhiteSpace(name))
                                {
                                    continue;
                                }

                                string stringValue = value switch
                                {
                                    DateTimeOffset dto => dto.ToUniversalTime().ToString("o"),
                                    DateTime dt => dt.ToUniversalTime().ToString("o"),
                                    bool b => b.ToString().ToLowerInvariant(),
                                    _ => value?.ToString() ?? string.Empty,
                                };

                                formData.Add(new StringContent(stringValue!), name);
                            }
                        }

                        request.Content = formData;
                        break;
                    }

                default:
                    throw new NotSupportedException($"Unsupported content type: {contentType}");
            }
        }

        if (!string.IsNullOrWhiteSpace(apiKey))
        {
            request.Headers.Add("Bearer", apiKey);
        }

        return await this.httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}
