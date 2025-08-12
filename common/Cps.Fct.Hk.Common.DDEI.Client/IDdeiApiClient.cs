// <copyright file="IDdeiServiceClient.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.DDEI.Client;

using System.Threading.Tasks;
using Cps.Fct.Hk.Common.DDEI.Client.Model;

/// <summary>
/// The interface for the DDEI API service client.
/// </summary>
public interface IDdeiApiClient
{
    /// <summary>
    /// Calls DDEI API and returns response.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="httpRequest"></param>
    /// <param name="cmsAuthValues"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<TResponse> HandleHttpRequestInternalAsync<TResponse>(HttpRequestMessage httpRequest, CmsAuthValues cmsAuthValues, BaseRequest request);

    /// <summary>
    /// Handles API exception.
    /// </summary>
    /// <param name="operationName"></param>
    /// <param name="path"></param>
    /// <param name="errorResponse"></param>
    /// <param name="exception"></param>
    /// <param name="request"></param>
    /// <param name="duration"></param>
    void HandleException(
            string operationName,
            string path,
            string? errorResponse,
            Exception exception,
            BaseRequest request,
            TimeSpan duration);

    /// <summary>
    /// Log completed event information.
    /// </summary>
    /// <param name="operationName"></param>
    /// <param name="path"></param>
    /// <param name="request"></param>
    /// <param name="duration"></param>
    /// <param name="additionalInfo"></param>
    void LogOperationCompletedEvent(
        string operationName,
        string path,
        BaseRequest request,
        TimeSpan duration,
        string additionalInfo);
}
