// <copyright file="MG3DocGenService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Services;

using System.Text.Json;
using System.Threading.Tasks;
using Cps.Fct.Hk.Common.DDEI.Client.Model;
using Cps.Fct.Djb.TransferToolApi.Services.Dto;

/// <summary>
/// MG3 Document generation service.
/// </summary>
public class MG3DocGenService
{
    private readonly IMdsApiClientFactory mdsApiClientFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="MG3DocGenService"/> class.
    /// </summary>
    /// <param name="mdsApiClientFactory">Pcd request provider.</param>
    public MG3DocGenService(IMdsApiClientFactory mdsApiClientFactory)
    {
        this.mdsApiClientFactory = mdsApiClientFactory ?? throw new ArgumentNullException(nameof(mdsApiClientFactory));
    }

    /// <summary>
    /// Sample get MDS Data method.
    /// </summary>
    /// <param name="caseId">Cookie id.</param>
    /// <param name="pcdId">pcd ID></param>
    /// <param name="cmsAuthValues">CMS Auth values.</param>
    /// <param name="correlationId">correlation id.</param>
    /// <returns>Returns MDS Data.</returns>
    public async Task GetMg3DocumentByPcdIdAsync(int caseId, int pcdId, CmsAuthValues cmsAuthValues, Guid correlationId)
    {
        var cookie = new MdsCookie(cmsAuthValues.CmsCookies, cmsAuthValues.CmsModernToken);
        var client = this.mdsApiClientFactory.Create(JsonSerializer.Serialize(cookie));
        _ = await client.GetCaseHistoryEventsAsync(caseId).ConfigureAwait(false);
    }
}
