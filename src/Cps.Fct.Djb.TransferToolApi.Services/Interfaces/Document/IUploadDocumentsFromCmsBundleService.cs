// <copyright file="IUploadDocumentsFromCmsBundleService.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Services.Interfaces.Document;

using System.Threading.Tasks;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.CaseCenter.Document;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;

/// <summary>
/// UploadDocumentsFromCmsBundle service interface.
/// </summary>
public interface IUploadDocumentsFromCmsBundleService
{
    /// <summary>
    /// Upload documents from a Cms bundle to a case in Case Center.
    /// </summary>
    /// <param name="inputUploadDocumentsFromCmsBundleDto">The payload for the documents to be uploaded.</param>
    /// <returns>Returns the case center case idfor the created case.</returns>
    public Task<HttpReturnResultDto<string>> UploadDocumentsFromCmsBundleAsync(UploadDocumentsFromCmsBundleDto inputUploadDocumentsFromCmsBundleDto);
}
