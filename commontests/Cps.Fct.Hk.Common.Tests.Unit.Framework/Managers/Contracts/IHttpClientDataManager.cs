// <copyright file="IHttpClientDataManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.Tests.Unit.Framework.Managers.Contracts;

public interface IHttpClientDataManager
{
    bool DoesRequestExist(HttpMethod httpMethod, string url, string authorisation, string partialContent);

    int GetRequestCount();
}
