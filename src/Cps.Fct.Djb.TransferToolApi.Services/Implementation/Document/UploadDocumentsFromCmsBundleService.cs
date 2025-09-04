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
    public async Task<HttpReturnResultDto<string>> UploadDocumentsFromCmsBundleAsync(UploadDocumentsFromCmsBundleDto inputUploadDocumentsFromCmsBundleDto)
    {
        try
        {
            Requires.NotNull(inputUploadDocumentsFromCmsBundleDto);
            Requires.NotNull(inputUploadDocumentsFromCmsBundleDto.CmsCaseId);
            Requires.NotNull(inputUploadDocumentsFromCmsBundleDto.DocumentUploader);
            Requires.NotNull(inputUploadDocumentsFromCmsBundleDto.CmsModernAuthToken);
            Requires.NotNull(inputUploadDocumentsFromCmsBundleDto.CmsClassicAuthCookies);

            //// get the case from MDS
            //var cookie = new MdsCookie(inputCreateCaseDto.CmsClassicAuthCookies, inputCreateCaseDto.CmsModernAuthToken);
            //var client = this.mdsApiClientFactory.Create(JsonSerializer.Serialize(cookie));
            //var caseSummary = await client.GetCaseSummaryAsync(inputCreateCaseDto.CmsCaseId).ConfigureAwait(false);

            //if (caseSummary is null)
            //{
            //    var message = $"No case found in MDS for CMS Case ID {inputCreateCaseDto.CmsCaseId}";
            //    this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(CreateCaseService)}.{nameof(this.CreateCaseAsync)}: Milestone - {message}");
            //    return HttpReturnResultDto<string>.Fail(HttpStatusCode.NotFound, message);
            //}

            //var downloadPath = "99ciHJPOTKUy$$TenxU9Jw_GUpiOHJuR6LBQ$$joxczrJjD__qRSFI/Indictment_1 .docx";
            //var encodedDownloadPath = Uri.EscapeDataString(Uri.EscapeDataString(downloadPath));

            //using (var fileResponse = await client.GetMaterialDocumentAsync(inputCreateCaseDto.CmsCaseId, encodedDownloadPath).ConfigureAwait(false))
            //{
            //    string fileName = fileResponse.GetFileName();
            //    string contentType = fileResponse.GetContentType();
            //    await fileResponse.SaveToFileAsync(Directory.GetCurrentDirectory()).ConfigureAwait(false);
            //    Console.WriteLine($"File saved as: {fileName}");
            //}

            //#pragma warning disable SA1101 // Prefix local calls with this
            //var caseToCreateDto = inputCreateCaseDto with
            //{
            //    AreaCrownCourtCode = caseSummary.NextHearingVenueCode ?? string.Empty,
            //    CaseUrn = caseSummary.Urn ?? string.Empty,
            //    CaseTitle = caseSummary.LeadDefendantFirstNames + " " + caseSummary.LeadDefendantSurname?.ToUpper(),
            //};
            //#pragma warning restore SA1101

            //// get the admin auth token from case center
            //var caseCenterApiClient = this.caseCenterApiClientFactory.Create(string.Empty);

            //var getAdminAuthTokenResponse = await caseCenterApiClient.GetAuthTokenAsync().ConfigureAwait(false);

            //if (!getAdminAuthTokenResponse.IsSuccess)
            //{
            //    return getAdminAuthTokenResponse;
            //}

            //var caseCenterAuthToken = getAdminAuthTokenResponse.Data;

            //// now create the case
            //var createCaseCenterCaseResponse = await caseCenterApiClient.CreateCaseAsync(caseCenterAuthToken, caseToCreateDto).ConfigureAwait(false);

            //if (!createCaseCenterCaseResponse.IsSuccess)
            //{
            //    return createCaseCenterCaseResponse;
            //}

            //var caseCenterCaseId = createCaseCenterCaseResponse.Data;

            //return HttpReturnResultDto<string>.Success(HttpStatusCode.Created, caseCenterCaseId);

            throw new NotImplementedException();
        }
        catch (Exception ex)
        {
            var message = $"Service encountered an error: {ex.Message}";
            this.logger.LogError($"{LoggingConstants.DjbTransferToolApiLogPrefix}.{nameof(UploadDocumentsFromCmsBundleService)}.{nameof(this.UploadDocumentsFromCmsBundleAsync)}: Milestone - {message}");
            return HttpReturnResultDto<string>.Fail(HttpStatusCode.InternalServerError, message);
        }
    }
}
