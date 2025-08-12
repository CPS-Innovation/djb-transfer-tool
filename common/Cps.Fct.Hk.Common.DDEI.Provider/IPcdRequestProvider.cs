// <copyright file="IDdeiServiceClient.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.DDEI.Provider;

using System.Threading.Tasks;
using Cps.Fct.Hk.Common.DDEI.Client.Model;
using Cps.Fct.Hk.Common.DDEI.Provider.Models.Request.PcdRequests;
using Cps.Fct.Hk.Common.DDEI.Provider.Models.Response.PCD;

/// <summary>
/// The interface for the DDEI API service client.
/// </summary>
public interface IPcdRequestProvider
{
    /// <summary>
    /// Get PCD request by pcd request id. 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cmsAuthValues"></param>
    /// <returns></returns>
    Task<PcdRequestDto> GetPcdRequestBypcdIdAsync(GetPcdRequestByPcdIdCoreRequest request, CmsAuthValues cmsAuthValues);
}
