// <copyright file="IAddDocumentToCaseCenterCaseService.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Services.Implementation.Interfaces;

using System.Threading.Tasks;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.CaseCenter;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;

/// <summary>
/// Add a document to a case center case service interface.
/// </summary>
public interface IAddDocumentToCaseCenterCaseService
{
    /// <summary>
    /// Adds a document to a case in Case Center.
    /// </summary>
    /// <param name="addDocumentToCaseDto">The dto containing all the information for the document to upload.</param>
    /// <returns>Returns the case center document id for the added document.</returns>
    public Task<HttpReturnResultDto<string>> AddDocumentToCaseCenterCaseAsync(AddDocumentToCaseDto addDocumentToCaseDto);
}
