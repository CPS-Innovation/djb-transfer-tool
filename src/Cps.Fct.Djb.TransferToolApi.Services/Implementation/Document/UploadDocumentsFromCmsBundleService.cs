// <copyright file="UploadDocumentsFromCmsBundleService.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Services.Implementation.Case;

using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Cps.Fct.Djb.TransferTool.Shared.Constants;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Factories.Interfaces;
using Cps.Fct.Djb.TransferToolApi.Services.Interfaces.Document;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.CaseCenter.Document;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Mds;
using Microsoft;
using Microsoft.Extensions.Logging;
using Cps.Fct.Djb.TransferToolApi.Shared.Extensions;
using System.Globalization;
using System.Buffers.Text;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

/// <summary>
/// UploadDocumentsFromCmsBundle service.
/// </summary>
public class UploadDocumentsFromCmsBundleService : IUploadDocumentsFromCmsBundleService
{
    private readonly ILogger<UploadDocumentsFromCmsBundleService> logger;
    private readonly ICaseCenterApiClientFactory caseCenterApiClientFactory;
    private readonly IMdsApiClientFactory mdsApiClientFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="UploadDocumentsFromCmsBundleService"/> class.
    /// </summary>
    /// <param name="logger">ILogger UploadDocumentsFromCmsBundleService.</param>
    /// <param name="caseCenterApiClientFactory">ICaseCenterApiClientFactory.</param>
    /// <param name="mdsApiClientFactory">IMdsApiClientFactory.</param>
    public UploadDocumentsFromCmsBundleService(
        ILogger<UploadDocumentsFromCmsBundleService> logger,
        ICaseCenterApiClientFactory caseCenterApiClientFactory,
        IMdsApiClientFactory mdsApiClientFactory)
    {
        this.logger = logger;
        this.caseCenterApiClientFactory = caseCenterApiClientFactory ?? throw new ArgumentNullException(nameof(caseCenterApiClientFactory));
        this.mdsApiClientFactory = mdsApiClientFactory;
    }

    /// <summary>
    /// Upload documents from a Cms bundle to a case in Case Center.
    /// </summary>
    /// <param name="inputUploadDocumentsFromCmsBundleDto">The payload for the documents to be uploaded.</param>
    /// <returns>Returns the case center case idfor the created case.</returns>
    public async Task<HttpReturnResultDto<List<MultipleDocumentsUploadedFileDataDto>>> UploadDocumentsFromCmsBundleAsync(UploadDocumentsFromCmsBundleDto inputUploadDocumentsFromCmsBundleDto)
    {
        try
        {
            Requires.NotNull(inputUploadDocumentsFromCmsBundleDto);
            Requires.NotNull(inputUploadDocumentsFromCmsBundleDto.CmsCaseId);
            Requires.NotNull(inputUploadDocumentsFromCmsBundleDto.CmsBundleId);
            Requires.NotNull(inputUploadDocumentsFromCmsBundleDto.DocumentUploader);
            Requires.NotNull(inputUploadDocumentsFromCmsBundleDto.CmsModernAuthToken);
            Requires.NotNull(inputUploadDocumentsFromCmsBundleDto.CmsClassicAuthCookies);

            // get the case from MDS
            var cookie = new MdsCookie(inputUploadDocumentsFromCmsBundleDto.CmsClassicAuthCookies, inputUploadDocumentsFromCmsBundleDto.CmsModernAuthToken);
            var client = this.mdsApiClientFactory.Create(JsonSerializer.Serialize(cookie));
            var caseSummary = await client.GetCaseSummaryAsync(inputUploadDocumentsFromCmsBundleDto.CmsCaseId).ConfigureAwait(false);

            if (caseSummary is null)
            {
                var message = $"No case found in MDS for CMS Case ID {inputUploadDocumentsFromCmsBundleDto.CmsCaseId}";
                this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(UploadDocumentsFromCmsBundleService)}.{nameof(this.UploadDocumentsFromCmsBundleAsync)}: Milestone - {message}");
                return HttpReturnResultDto<List<MultipleDocumentsUploadedFileDataDto>>.Fail(HttpStatusCode.NotFound, message);
            }

            var caseCenterApiClient = this.caseCenterApiClientFactory.Create(string.Empty);

            var getAdminAuthTokenResponse = await caseCenterApiClient.GetAuthTokenAsync().ConfigureAwait(false);

            if (!getAdminAuthTokenResponse.IsSuccess)
            {
                var message = $"Unable to Case Center authentication token. Reason: {getAdminAuthTokenResponse.Message}";
                this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(UploadDocumentsFromCmsBundleService)}.{nameof(this.UploadDocumentsFromCmsBundleAsync)}: Milestone - {message}");
                return HttpReturnResultDto<List<MultipleDocumentsUploadedFileDataDto>>.Fail(HttpStatusCode.NotFound, message);
            }

            inputUploadDocumentsFromCmsBundleDto.CaseCenterAuthToken = getAdminAuthTokenResponse.Data;

            var getCaseIdResponse = await caseCenterApiClient.GetCaseIdAsync(inputUploadDocumentsFromCmsBundleDto.CaseCenterAuthToken, inputUploadDocumentsFromCmsBundleDto.CmsCaseId.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);

            if (!getCaseIdResponse.IsSuccess)
            {
                var message = $"Unable to Case Center case id. Reason: {getCaseIdResponse.Message}";
                this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(UploadDocumentsFromCmsBundleService)}.{nameof(this.UploadDocumentsFromCmsBundleAsync)}: Milestone - {message}");
                return HttpReturnResultDto<List<MultipleDocumentsUploadedFileDataDto>>.Fail(HttpStatusCode.NotFound, message);
            }

            var caseCenterCaseId = getCaseIdResponse.Data;

            // get the bundle material from MDS
            var bundleMaterials = await client.ListBundleMaterialsAsync(inputUploadDocumentsFromCmsBundleDto.CmsCaseId, inputUploadDocumentsFromCmsBundleDto.CmsBundleId).ConfigureAwait(false);

            if (bundleMaterials is null)
            {
                var message = $"No bundles found in MDS for CMS Bundle ID {inputUploadDocumentsFromCmsBundleDto.CmsBundleId}";
                this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(UploadDocumentsFromCmsBundleService)}.{nameof(this.UploadDocumentsFromCmsBundleAsync)}: Milestone - {message}");
                return HttpReturnResultDto<List<MultipleDocumentsUploadedFileDataDto>>.Fail(HttpStatusCode.NotFound, message);
            }

            // for each document in the bundle, get the document from MDS
            var downloadTasks = bundleMaterials.Select(async bm =>
            {
                if (string.IsNullOrWhiteSpace(bm.Link))
                {
                    return new KeyValuePair<int, UploadMultipleDocumentsFileDataDto?>(bm.DocumentId, null);
                }

                var encodedDownloadPath = Uri.EscapeDataString(Uri.EscapeDataString(bm.Link));

                try
                {
                    using var fileResponse = await client
                        .GetMaterialDocumentAsync(inputUploadDocumentsFromCmsBundleDto.CmsCaseId, encodedDownloadPath)
                        .ConfigureAwait(false);

                    string fileName = fileResponse.GetFileName();
                    string contentType = fileResponse.GetContentType();
                    byte[] fileContents = await fileResponse.ToByteArrayAsync().ConfigureAwait(false);
                    string base64Content = Convert.ToBase64String(fileContents);

                    byte[] digest = SHA256.HashData(Encoding.UTF8.GetBytes(base64Content));
                    string checksum = Convert.ToHexString(digest).ToLowerInvariant();

                    return new KeyValuePair<int, UploadMultipleDocumentsFileDataDto?>(bm.DocumentId, new UploadMultipleDocumentsFileDataDto()
                    {
                        FileName = fileName,
                        ContentType = contentType,
                        Content = fileContents,
                        Base64EncodedContent = base64Content,
                        Checksum = checksum,
                        CmsDocumentId = bm.DocumentId,
                    });
                }
                catch(Exception getMaterialDocumentException)
                {
                    var message = $"Get document from CMS encountered an error: {getMaterialDocumentException.Message}";
                    this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(UploadDocumentsFromCmsBundleService)}.{nameof(this.UploadDocumentsFromCmsBundleAsync)}: Milestone - {message}");
                    return new KeyValuePair<int, UploadMultipleDocumentsFileDataDto?>(bm.DocumentId, null);
                }
            });

            var documentsFromMds = (await Task.WhenAll(downloadTasks).ConfigureAwait(false)).ToList();

            // all files that were not downloaded
            var documentsNotDownloaded = documentsFromMds
                .Where(x => x.Value is null);
            var indictmentDocuments = documentsFromMds
                .Where(x => x.Value is not null && x.Value.FileName.StartsWith("Indictment/", StringComparison.InvariantCultureIgnoreCase));
            var exhibitDocuments = documentsFromMds
                .Where(x => x.Value is not null && !x.Value.FileName.StartsWith("Indictment/", StringComparison.InvariantCultureIgnoreCase));

            getAdminAuthTokenResponse = await caseCenterApiClient.GetAuthTokenAsync().ConfigureAwait(false);

            if (!getAdminAuthTokenResponse.IsSuccess)
            {
                var message = $"Unable to Case Center authentication token. Reason: {getAdminAuthTokenResponse.Message}";
                this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(UploadDocumentsFromCmsBundleService)}.{nameof(this.UploadDocumentsFromCmsBundleAsync)}: Milestone - {message}");
                return HttpReturnResultDto<List<MultipleDocumentsUploadedFileDataDto>>.Fail(HttpStatusCode.NotFound, message);
            }

            var addIndictmentDocumentsResponse = await caseCenterApiClient
                .AddIndictmentsToCaseSectionIdAsync(
                inputUploadDocumentsFromCmsBundleDto.CaseCenterAuthToken,
                caseCenterCaseId,
                inputUploadDocumentsFromCmsBundleDto.DocumentUploader,
                indictmentDocuments.Select(x => x.Value).ToList()).ConfigureAwait(false);

            var addExhibitDocumentsResponse = await caseCenterApiClient
                .AddIndictmentsToCaseSectionIdAsync(
                inputUploadDocumentsFromCmsBundleDto.CaseCenterAuthToken,
                caseCenterCaseId,
                inputUploadDocumentsFromCmsBundleDto.DocumentUploader,
                exhibitDocuments.Select(x => x.Value).ToList()).ConfigureAwait(false);

            var responseData = new List<MultipleDocumentsUploadedFileDataDto>();
            if (addIndictmentDocumentsResponse.IsSuccess && addIndictmentDocumentsResponse.Data is not null)
            {
                responseData.AddRange(addIndictmentDocumentsResponse.Data);
            }

            if (addExhibitDocumentsResponse.IsSuccess && addExhibitDocumentsResponse.Data is not null)
            {
                responseData.AddRange(addExhibitDocumentsResponse.Data);
            }

            if (documentsNotDownloaded.Any())
            {
                responseData.AddRange(documentsNotDownloaded.Select(x => new MultipleDocumentsUploadedFileDataDto
                {
                    CmsDocumentId = x.Key,
                    ErrorCode = "Document could not be downloaded from CMS.",
                    ErrorMessage = "Document could not be downloaded from CMS.",
                    UploadStatus = false,
                    Filename = bundleMaterials.Any(bm => bm.DocumentId == x.Key && !string.IsNullOrWhiteSpace(bm.OriginalFileName)) ? bundleMaterials.First(bm => bm.DocumentId == x.Key).OriginalFileName ?? "Unknown" : "Unknown",
                }));
            }

            return HttpReturnResultDto<List<MultipleDocumentsUploadedFileDataDto>>.Success(HttpStatusCode.Created, responseData);
        }
        catch (Exception ex)
        {
            var message = $"Service encountered an error: {ex.Message}";
            this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(UploadDocumentsFromCmsBundleService)}.{nameof(this.UploadDocumentsFromCmsBundleAsync)}: Milestone - {message}");
            return HttpReturnResultDto<List<MultipleDocumentsUploadedFileDataDto>>.Fail(HttpStatusCode.InternalServerError, message);
        }
    }
}
