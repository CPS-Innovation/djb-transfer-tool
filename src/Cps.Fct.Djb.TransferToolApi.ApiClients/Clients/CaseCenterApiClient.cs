// <copyright file="CaseCenterApiCaseClient.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.Clients;

using Cps.Fct.Djb.TransferTool.Services.Clients.Interfaces;
using Cps.Fct.Djb.TransferTool.Shared.ConfigOptions;
using Cps.Fct.Djb.TransferTool.Shared.Constants;
using Cps.Fct.Djb.TransferTool.Shared.Dtos.Auth;
using Cps.Fct.Djb.TransferTool.Shared.Dtos.Bundle;
using Cps.Fct.Djb.TransferTool.Shared.Dtos.Case;
using Cps.Fct.Djb.TransferTool.Shared.Dtos.Common;
using Cps.Fct.Djb.TransferTool.Shared.Dtos.User;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Clients.Interfaces;
using Microsoft;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;

/// <summary>
/// Class for interacting with the case center admin api.
/// </summary>
public class CaseCenterApiClient : ICaseCenterApiClient
{
    private readonly ILogger<CaseCenterApiClient> logger;

    public CaseCenterApiClient(ILogger<CaseCenterApiClient> logger, IHttpClientFactory clientFactory, IOptionsMonitor<ClientEndpointOptions> clientOptions)
        : base(clientFactory, clientOptions, CaseCenterConfigConstants.CaseCenterApiClientConfigurationName)
    {
        this.logger = logger;
    }

    /// <summary>
    /// Creates a Case Center case based on the payload data.
    /// Returns HttpReturnResultDto with a case center case id if successful.
    ///   - id is non-null if success
    ///   - id is null if failure (then statusCode is the server's code or 500 if exception)
    /// </summary>
    /// <param name="CreateCaseDto">The DTO containing the case payload data.</param>
    /// <returns>
    /// A HttpResponseMessage
    /// </returns>
    public async Task<HttpReturnResultDto<string>> CreateCaseAsync(CreateCaseDto createCaseDto)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            logger.LogInformation($"{LoggingConstants.DjbTransferToolLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(CreateCaseAsync)}: Milestone - Api Client about to process Api call.");

            Requires.NotNull(createCaseDto);
            Requires.NotNullOrEmpty(createCaseDto.CmsUrn);
            Requires.NotNullOrEmpty(createCaseDto.CaseCreator);
            Requires.NotNullOrEmpty(createCaseDto.OrganisationId);
            Requires.NotNullOrEmpty(createCaseDto.CaseTitle);
            Requires.NotNullOrEmpty(createCaseDto.TemplateId);
            Requires.NotNullOrEmpty(createCaseDto.Type);
            Requires.NotNullOrEmpty(createCaseDto.CmsUniqueId);
            Requires.NotNullOrEmpty(createCaseDto.CaseCenterAuthToken);

            var path = string.Format(this.clientOptions.RelativePath[CaseCenterConfigConstants.CaseCenterApiCreateCasePathName],
                createCaseDto?.CmsUrn, createCaseDto?.CaseCreator, createCaseDto?.OrganisationId, createCaseDto?.CaseTitle, createCaseDto?.TemplateId,
                createCaseDto?.Type, createCaseDto?.CmsUniqueId, createCaseDto?.DepartmentId);

            var response = await this.SendRequestAsync<object>(
                HttpMethod.Post,
                path: path,
                apiKey: createCaseDto?.CaseCenterAuthToken ?? string.Empty
            );

            // var response =  new HttpResponseMessage(System.Net.HttpStatusCode.Created) { Content = new StringContent(JsonSerializer.Serialize(new { success = true, returnValue = Guid.NewGuid().ToString("N") }), Encoding.UTF8, "application/json") };

            if (!response.IsSuccessStatusCode)
            {
                return HttpReturnResultDto<string>.Fail(response.StatusCode, response.ReasonPhrase);
            }

            string responsePayload = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(responsePayload))
            {
                return HttpReturnResultDto<string>.Fail(HttpStatusCode.InternalServerError, "No case center case id returned");
            }

            logger.LogInformation($"{LoggingConstants.DjbTransferToolLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(CreateCaseAsync)}: Milestone: Api Client completed Api call in [{stopwatch.Elapsed}]");
            return HttpReturnResultDto<string>.Success(response.StatusCode, responsePayload);
        }
        catch (Exception ex)
        {
            var message = $"Api Client encountered an error: {ex.Message}";
            logger.LogError($"{LoggingConstants.DjbTransferToolLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(CreateCaseAsync)}: Milestone - {message}");
            return HttpReturnResultDto<string>.Fail(HttpStatusCode.InternalServerError, message);
        }
    }

    /// <summary>
    /// Gets the specified Case Center case id based on the payload data.
    /// Returns HttpReturnResultDto with a payload of the case center case if successful.
    ///   - id is non-null if success
    ///   - id is null if failure (then statusCode is the server's code or 500 if exception)
    /// </summary>
    /// <param name="GetCaseCenterCaseIdDto">The DTO containing the case payload data.</param>
    /// <returns>
    /// A HttpResponseMessage
    /// </returns>
    public async Task<HttpReturnResultDto<string>> GetCaseCenterCaseIdAsync(GetCaseCenterCaseIdDto getCaseCenterCaseIdDto)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            logger.LogInformation($"{LoggingConstants.DjbTransferToolLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(GetCaseCenterCaseIdAsync)}: Milestone - Api Client about to process Api call.");

            Requires.NotNull(getCaseCenterCaseIdDto);
            Requires.NotNullOrEmpty(getCaseCenterCaseIdDto.CaseCenterAuthToken);
            Requires.NotNullOrEmpty(getCaseCenterCaseIdDto.CmsUniqueId);

            var path = string.Format(this.clientOptions.RelativePath[CaseCenterConfigConstants.CaseCenterApiGetCasePathName],
                getCaseCenterCaseIdDto.CmsUniqueId);

            var response = await this.SendRequestAsync<object>(
                HttpMethod.Post,
                path: path,
                apiKey: getCaseCenterCaseIdDto?.CaseCenterAuthToken ?? string.Empty
            );

            if (!response.IsSuccessStatusCode)
            {
                return HttpReturnResultDto<string>.Fail(response.StatusCode, response.ReasonPhrase);
            }

            string responsePayload = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(responsePayload))
            {
                return HttpReturnResultDto<string>.Fail(HttpStatusCode.InternalServerError, "Invalid case id");
            }

            logger.LogInformation($"{LoggingConstants.DjbTransferToolLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(GetCaseCenterCaseIdAsync)}: Milestone: Api Client completed Api call in [{stopwatch.Elapsed}]");
            return HttpReturnResultDto<string>.Success(response.StatusCode, responsePayload);
        }
        catch (Exception ex)
        {
            var message = $"Api Client encountered an error: {ex.Message}";
            logger.LogError($"{LoggingConstants.DjbTransferToolLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(GetCaseCenterCaseIdAsync)}: Milestone - {message}");
            return HttpReturnResultDto<string>.Fail(HttpStatusCode.InternalServerError, message);
        }
    }

    /// <summary>
    /// Gets the specified bundle in Case Center case based on the payload data.
    /// Returns HttpReturnResultDto with a list of ids of the case center case bundles if successful.
    ///   - id is non-null if success
    ///   - id is null if failure (then statusCode is the server's code or 500 if exception)
    /// </summary>
    /// <param name="GetCaseBundleDto">The DTO containing the case payload data.</param>
    /// <returns>
    /// A HttpResponseMessage
    /// </returns>
    public async Task<HttpReturnResultDto<List<string>>> GetCaseBundlesAsync(GetCaseBundleDto getCaseBundleDto)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            logger.LogInformation($"{LoggingConstants.DjbTransferToolLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(GetCaseBundlesAsync)}: Milestone - Api Client about to process Api call.");

            Requires.NotNull(getCaseBundleDto);
            Requires.NotNullOrEmpty(getCaseBundleDto.CaseCenterAuthToken);
            Requires.NotNullOrEmpty(getCaseBundleDto.CaseCenterCaseId);

            var path = string.Format(this.clientOptions.RelativePath[CaseCenterConfigConstants.CaseCenterApiGetBundlesInCasePathName],
                getCaseBundleDto.CaseCenterCaseId);

            var response = await this.SendRequestAsync<object>(
                HttpMethod.Post,
                path: path,
                apiKey: getCaseBundleDto?.CaseCenterAuthToken ?? string.Empty
            );

            if (!response.IsSuccessStatusCode)
            {
                return HttpReturnResultDto<List<string>>.Fail(response.StatusCode, response.ReasonPhrase);
            }

            string responsePayload = await response.Content.ReadAsStringAsync();

            var bundles = JsonConvert.DeserializeObject<List<BundleSummaryDto>>(responsePayload);

            if (bundles is null || !bundles.Any())
            {
                return HttpReturnResultDto<List<string>>.Fail(HttpStatusCode.InternalServerError, "No bundles in case found");
            }

            var bundleIds = new List<string>();

            if (string.IsNullOrWhiteSpace(getCaseBundleDto?.BundleName))
            {
                bundleIds = bundles.Select(x => x.Id).ToList();
            } 
            else
            {
                var matchingBundle = bundles.FirstOrDefault(x => x.Name.Equals(getCaseBundleDto?.BundleName, StringComparison.InvariantCultureIgnoreCase));

                if (matchingBundle is null)
                {
                    return HttpReturnResultDto<List<string>>.Fail(HttpStatusCode.InternalServerError, "Specified bundle not found in case");
                } 
                else
                {
                    bundleIds.Add(matchingBundle.Id);
                }
            }

            logger.LogInformation($"{LoggingConstants.DjbTransferToolLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(GetCaseBundlesAsync)}: Milestone: Api Client completed Api call in [{stopwatch.Elapsed}]");
            return HttpReturnResultDto<List<string>>.Success(response.StatusCode, bundleIds);
        }
        catch (Exception ex)
        {
            var message = $"Api Client encountered an error: {ex.Message}";
            logger.LogError($"{LoggingConstants.DjbTransferToolLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(GetCaseBundlesAsync)}: Milestone - {message}");
            return HttpReturnResultDto<List<string>>.Fail(HttpStatusCode.InternalServerError, message);
        }
    }

    /// <summary>
    /// Gets the specified section in Case Center bundle based on the payload data.
    /// Returns HttpReturnResultDto with a list of ids of the case center case bundle sections if successful.
    ///   - id is non-null if success
    ///   - id is null if failure (then statusCode is the server's code or 500 if exception)
    /// </summary>
    /// <param name="GetCaseBundleSectionDto">The DTO containing the case payload data.</param>
    /// <returns>
    /// A HttpResponseMessage
    /// </returns>
    public async Task<HttpReturnResultDto<List<string>>> GetBundleSectionsAsync(GetBundleSectionDto getBundleSectionDto)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            logger.LogInformation($"{LoggingConstants.DjbTransferToolLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(GetBundleSectionsAsync)}: Milestone - Api Client about to process Api call.");

            Requires.NotNull(getBundleSectionDto);
            Requires.NotNullOrEmpty(getBundleSectionDto.CaseCenterAuthToken);
            Requires.NotNullOrEmpty(getBundleSectionDto.CaseCenterCaseId);
            Requires.NotNullOrEmpty(getBundleSectionDto.BundleId);

            var path = string.Format(this.clientOptions.RelativePath[CaseCenterConfigConstants.CaseCenterApiGetSectionsInBundlePathName],
                getBundleSectionDto.CaseCenterCaseId, getBundleSectionDto.BundleId);

            var response = await this.SendRequestAsync<object>(
                HttpMethod.Post,
                path: path,
                apiKey: getBundleSectionDto?.CaseCenterAuthToken ?? string.Empty
            );

            if (!response.IsSuccessStatusCode)
            {
                return HttpReturnResultDto<List<string>>.Fail(response.StatusCode, response.ReasonPhrase);
            }

            string responsePayload = await response.Content.ReadAsStringAsync();

            var bundleSections = JsonConvert.DeserializeObject<List<BundleSectionSummaryDto>>(responsePayload);

            if (bundleSections is null || !bundleSections.Any())
            {
                return HttpReturnResultDto<List<string>>.Fail(HttpStatusCode.InternalServerError, "No sections in bundle found");
            }

            var bundleSectionIds = new List<string>();

            if (string.IsNullOrWhiteSpace(getBundleSectionDto?.SectionName))
            {
                bundleSectionIds = bundleSections.Select(x => x.SectionId).ToList();
            } 
            else
            {
                var matchingBundleSection = bundleSections.FirstOrDefault(x => x.SectionName.Equals(getBundleSectionDto?.SectionName, StringComparison.InvariantCultureIgnoreCase));

                if (matchingBundleSection is null)
                {
                    return HttpReturnResultDto<List<string>>.Fail(HttpStatusCode.InternalServerError, "Specified section not found in bundle");
                } 
                else
                {
                    bundleSectionIds.Add(matchingBundleSection.SectionId);
                }
            }

            logger.LogInformation($"{LoggingConstants.DjbTransferToolLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(GetBundleSectionsAsync)}: Milestone: Api Client completed Api call in [{stopwatch.Elapsed}]");
            return HttpReturnResultDto<List<string>>.Success(response.StatusCode, bundleSectionIds);
        }
        catch (Exception ex)
        {
            var message = $"Api Client encountered an error: {ex.Message}";
            logger.LogError($"{LoggingConstants.DjbTransferToolLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(GetBundleSectionsAsync)}: Milestone - {message}");
            return HttpReturnResultDto<List<string>>.Fail(HttpStatusCode.InternalServerError, message);
        }
    }

    /// <summary>
    /// Gets the first template in Case Center case based on the payload data.
    /// Returns HttpReturnResultDto with a id of the template case center case if successful.
    ///   - id is non-null if success
    ///   - id is null if failure (then statusCode is the server's code or 500 if exception)
    /// </summary>
    /// <param name="AuthenticatedUserDto">The DTO containing the case payload data.</param>
    /// <returns>
    /// A HttpResponseMessage
    /// </returns>
    public async Task<HttpReturnResultDto<string>> GetTemplateCaseAsync(AuthenticatedUserDto authenticatedUserDto)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            logger.LogInformation($"{LoggingConstants.DjbTransferToolLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(GetTemplateCaseAsync)}: Milestone - Api Client about to process Api call.");

            Requires.NotNull(authenticatedUserDto);
            Requires.NotNullOrEmpty(authenticatedUserDto.CaseCenterAuthToken);

            var path = this.clientOptions.RelativePath[CaseCenterConfigConstants.CaseCenterApiGetTemplateCasesPathName];

            var response = await this.SendRequestAsync<object>(
                HttpMethod.Post,
                path: path,
                apiKey: authenticatedUserDto?.CaseCenterAuthToken ?? string.Empty
            );

            if (!response.IsSuccessStatusCode)
            {
                return HttpReturnResultDto<string>.Fail(response.StatusCode, response.ReasonPhrase);
            }

            string responsePayload = await response.Content.ReadAsStringAsync();

            var templateCases = JsonConvert.DeserializeObject<List<CaseSummaryDto>>(responsePayload);

            if (templateCases is null || !templateCases.Any() || string.IsNullOrWhiteSpace(templateCases.First().PartitionKey))
            {
                return HttpReturnResultDto<string>.Fail(HttpStatusCode.InternalServerError, "No case center templates found");
            }

            logger.LogInformation($"{LoggingConstants.DjbTransferToolLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(GetTemplateCaseAsync)}: Milestone: Api Client completed Api call in [{stopwatch.Elapsed}]");
            return HttpReturnResultDto<string>.Success(response.StatusCode, templateCases.First().PartitionKey);
        }
        catch (Exception ex)
        {
            var message = $"Api Client encountered an error: {ex.Message}";
            logger.LogError($"{LoggingConstants.DjbTransferToolLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(GetTemplateCaseAsync)}: Milestone - {message}");
            return HttpReturnResultDto<string>.Fail(HttpStatusCode.InternalServerError, message);
        }
    }

    /// <summary>
    /// Adds the specified user to the specified Case Center case based on the payload data.
    /// Returns HttpReturnResultDto with flag if successful.
    ///   - is true if success
    ///   - is false if failure (then statusCode is the server's code or 500 if exception)
    /// </summary>
    /// <param name="AddUserToCaseDto">The DTO containing the payload data.</param>
    /// <returns>
    /// A HttpResponseMessage
    /// </returns>
    public async Task<HttpReturnResultDto<bool>> AddUserToCaseAsync(AddUserToCaseDto addUserToCase)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            logger.LogInformation($"{LoggingConstants.DjbTransferToolLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(AddUserToCaseAsync)}: Milestone - Api Client about to process Api call.");

            Requires.NotNull(addUserToCase);
            Requires.NotNullOrEmpty(addUserToCase.CaseCenterAuthToken);
            Requires.NotNullOrEmpty(addUserToCase.Username);
            Requires.NotNullOrEmpty(addUserToCase.CaseCenterCaseId);

            var path = string.Format(this.clientOptions.RelativePath[CaseCenterConfigConstants.CaseCenterApiAddUserToCasePathName],
                addUserToCase.CaseCenterCaseId, addUserToCase.Username, addUserToCase.BundleIdsAsString);

            var response = await this.SendRequestAsync<object>(
                HttpMethod.Post,
                path: path,
                apiKey: addUserToCase?.CaseCenterAuthToken ?? string.Empty
            );

            if (!response.IsSuccessStatusCode)
            {
                return HttpReturnResultDto<bool>.Fail(response.StatusCode, response.ReasonPhrase);
            }

            logger.LogInformation($"{LoggingConstants.DjbTransferToolLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(AddUserToCaseAsync)}: Milestone: Api Client completed Api call in [{stopwatch.Elapsed}]");
            return HttpReturnResultDto<bool>.Success(response.StatusCode, true);
        }
        catch (Exception ex)
        {
            var message = $"Api Client encountered an error: {ex.Message}";
            logger.LogError($"{LoggingConstants.DjbTransferToolLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(AddUserToCaseAsync)}: Milestone - {message}");
            return HttpReturnResultDto<bool>.Fail(HttpStatusCode.InternalServerError, message);
        }
    }

    /// <summary>
    /// Adds the specified document to the specified Case Center case based on the payload data.
    /// Returns HttpReturnResultDto with document id if successful.
    ///   - id is non-null if success
    ///   - id is null if failure (then statusCode is the server's code or 500 if exception)
    /// </summary>
    /// <param name="AddDocumentToCaseDto">The DTO containing the payload data.</param>
    /// <returns>
    /// A HttpResponseMessage
    /// </returns>
    public async Task<HttpReturnResultDto<string>> AddDocumentToCaseAsync(AddDocumentToCaseDto addDocumentToCase)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            logger.LogInformation($"{LoggingConstants.DjbTransferToolLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(AddDocumentToCaseAsync)}: Milestone - Api Client about to process Api call.");

            Requires.NotNull(addDocumentToCase);
            Requires.NotNullOrEmpty(addDocumentToCase.CaseCenterAuthToken);
            Requires.NotNullOrEmpty(addDocumentToCase.CaseId);
            Requires.NotNullOrEmpty(addDocumentToCase.SectionId);
            Requires.NotNullOrEmpty(addDocumentToCase.Username);
            Requires.Argument(addDocumentToCase.Date != default, nameof(addDocumentToCase.Date), "Parameter 'Date' must not be the default value.");
            Requires.NotNullOrEmpty(addDocumentToCase.FileData);
            Requires.NotNullOrEmpty(addDocumentToCase.FileSize);
            Requires.NotNullOrEmpty(addDocumentToCase.FileName);
            Requires.NotNullOrEmpty(addDocumentToCase.FileExtension);
            Requires.NotNullOrEmpty(addDocumentToCase.CheckSum);

            var path = this.clientOptions.RelativePath[CaseCenterConfigConstants.CaseCenterApiUploadDocumentToCasePathName];

            //var response = await this.SendRequestAsync<AddDocumentToCaseDto>(
            //    HttpMethod.Post,
            //    path: path,
            //    payload: addDocumentToCase, 
            //    contentType: ApiContentTypeConstants.MultipartFormData,
            //    apiKey: addDocumentToCase?.CaseCenterAuthToken ?? string.Empty
            //);

            var response = await this.UploadDocumentAsync(path, addDocumentToCase, addDocumentToCase?.CaseCenterAuthToken ?? string.Empty);

            if (!response.IsSuccessStatusCode)
            {
                return HttpReturnResultDto<string>.Fail(response.StatusCode, response.ReasonPhrase);
            }

            string responsePayload = await response.Content.ReadAsStringAsync();
            var addedDocument  = JsonConvert.DeserializeObject<DocumentAddedToCaseDto>(responsePayload);

            if (addedDocument is null || string.IsNullOrWhiteSpace(addedDocument?.DocumentId))
            {
                return HttpReturnResultDto<string>.Fail(HttpStatusCode.InternalServerError, "Could not add document");
            }

            logger.LogInformation($"{LoggingConstants.DjbTransferToolLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(AddDocumentToCaseAsync)}: Milestone: Api Client completed Api call in [{stopwatch.Elapsed}]");
            return HttpReturnResultDto<string>.Success(response.StatusCode, addedDocument?.DocumentId);
        }
        catch (Exception ex)
        {
            var message = $"Api Client encountered an error: {ex.Message}";
            logger.LogError($"{LoggingConstants.DjbTransferToolLogPrefix}.{nameof(CaseCenterApiClient)}.{nameof(AddDocumentToCaseAsync)}: Milestone - {message}");
            return HttpReturnResultDto<string>.Fail(HttpStatusCode.InternalServerError, message);
        }
    }
}
