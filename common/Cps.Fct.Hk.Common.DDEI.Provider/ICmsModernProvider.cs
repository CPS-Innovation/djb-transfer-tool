// <copyright file="IDdeiServiceClient.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.DDEI.Provider;

using System.Threading.Tasks;
using Cps.Fct.Hk.Common.DDEI.Client.Model.Requests;

/// <summary>
/// The interface for the DDEI API service client.
/// </summary>
public interface ICmsModernProvider
{
    /// <summary>
    /// Get CMS modern token.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<string?> GetCmsModernTokenAsync(GetCmsModernTokenRequest request);
}
