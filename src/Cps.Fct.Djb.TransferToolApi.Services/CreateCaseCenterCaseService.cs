// <copyright file="CreateCaseCenterCaseService.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Services;

using System.Net;
using System.Threading.Tasks;
using Cps.Fct.Djb.TransferToolApi.Services.Interfaces;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;

/// <summary>
/// Create case center case service.
/// </summary>
public class CreateCaseCenterCaseService : ICreateCaseCenterCaseService
{
    private readonly IMdsApiClientFactory mdsApiClientFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCaseCenterCaseService"/> class.
    /// </summary>
    /// <param name="mdsApiClientFactory">Pcd request provider.</param>
    public CreateCaseCenterCaseService(IMdsApiClientFactory mdsApiClientFactory)
    {
        this.mdsApiClientFactory = mdsApiClientFactory ?? throw new ArgumentNullException(nameof(mdsApiClientFactory));
    }

    /// <summary>
    /// Create a case in Case Center.
    /// </summary>
    /// <returns>Returns the case center case idfor the created case.</returns>
    public async Task<HttpReturnResultDto<string>> CreateCaseAsync()
    {
        return await Task.FromResult(
               HttpReturnResultDto<string>.Success(
                   HttpStatusCode.OK,
                   "Fake case created successfully")).ConfigureAwait(false);
    }
}
