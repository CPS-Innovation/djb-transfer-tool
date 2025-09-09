// <copyright file="ICaseCenterApiClient.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.Clients.Interfaces;

using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.CaseCenter.Case;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.CaseCenter.Document;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;

/// <summary>
/// Contract for ICaseCenterApiClient.
/// </summary>
public interface ICaseCenterApiClient
{
    /// <summary>
    /// Calls the case center test api.
    /// </summary>
    /// <returns>Returns the test message if success else empty.</returns>
    public Task<HttpReturnResultDto<string>> CallCaseCenterTestApiAsync();

    /// <summary>
    /// Gets an authentication token from the Case Center API using the provided user credentials.
    /// </summary>
    /// <returns>A HttpReturnResultDto with the authentication token as a payload.</returns>
    public Task<HttpReturnResultDto<string>> GetAuthTokenAsync();

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
    public Task<HttpReturnResultDto<string>> CreateCaseAsync(string authenticationToken, CreateCaseDto createCaseDto);

    /// <summary>
    /// Gets case center case id based on the source system id.
    /// Returns HttpReturnResultDto with a case center case id if successful.
    ///   - id is non-null if success
    ///   - id is null if failure (then statusCode is the server's code or 500 if exception).
    /// </summary>
    /// <param name="authenticationToken">The authentication token for the operation.</param>
    /// <param name="sourceSystemCaseId">The source system case id.</param>
    /// <returns>
    /// A HttpResponseMessage.
    /// </returns>
    public Task<HttpReturnResultDto<string>> GetCaseIdAsync(string authenticationToken, string sourceSystemCaseId);

    /// <summary>
    /// Gets the section id for the specified section from the specified bundle from the specified case.
    /// Returns HttpReturnResultDto with a section id from case center if successful.
    ///   - id is non-null if success
    ///   - id is null if failure (then statusCode is the server's code or 500 if exception).
    /// </summary>
    /// <param name="authenticationToken">The authentication token for the operation.</param>
    /// <param name="caseId">The case center case id.</param>
    /// <param name="bundleId">The bundle id.</param>
    /// <param name="sectionName">The section name.</param>
    /// <returns>
    /// A HttpResponseMessage.
    /// </returns>
    public Task<HttpReturnResultDto<string>> GetSectionIdAsync(string authenticationToken, string caseId, string bundleId, string sectionName);

    /// <summary>
    /// Uploads multiple files to the indictments section of the master bundle in the specified case.
    /// Returns HttpReturnResultDto with a section id from case center if successful.
    ///   - id is non-null if success
    ///   - id is null if failure (then statusCode is the server's code or 500 if exception).
    /// </summary>
    /// <param name="authenticationToken">The authentication token for the operation.</param>
    /// <param name="caseId">The case center case id.</param>
    /// <param name="username">The username to impersonate.</param>
    /// <param name="filesToUpload">The files to upload id.</param>
    /// <returns>
    /// A HttpResponseMessage.
    /// </returns>
    public Task<HttpReturnResultDto<List<MultipleDocumentsUploadedFileDataDto>>> AddIndictmentsToCaseSectionIdAsync(string authenticationToken, string caseId, string username, IEnumerable<UploadMultipleDocumentsFileDataDto> filesToUpload);

    /// <summary>
    /// Uploads multiple files to the exhibits section of the master bundle in the specified case.
    /// Returns HttpReturnResultDto with a section id from case center if successful.
    ///   - id is non-null if success
    ///   - id is null if failure (then statusCode is the server's code or 500 if exception).
    /// </summary>
    /// <param name="authenticationToken">The authentication token for the operation.</param>
    /// <param name="caseId">The case center case id.</param>
    /// <param name="username">The username to impersonate.</param>
    /// <param name="filesToUpload">The files to upload id.</param>
    /// <returns>
    /// A HttpResponseMessage.
    /// </returns>
    public Task<HttpReturnResultDto<List<MultipleDocumentsUploadedFileDataDto>>> AddExhibitsToCaseSectionIdAsync(string authenticationToken, string caseId, string username, IEnumerable<UploadMultipleDocumentsFileDataDto> filesToUpload);
}
