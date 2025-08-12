// <copyright file="HttpClientDataManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.Tests.Unit.Framework.Managers;
using Cps.Fct.Hk.Common.Tests.Unit.Framework.Fakes;
using Cps.Fct.Hk.Common.Tests.Unit.Framework.Managers.Contracts;

public class HttpClientDataManager : IHttpClientDataManager
{
    private readonly FakeHttpClientHandler fakeHttpClientHandler;

    public HttpClientDataManager(FakeHttpClientHandler fakeHttpClientHandler)
    {
        this.fakeHttpClientHandler = fakeHttpClientHandler;
    }

    public bool DoesRequestExist(HttpMethod httpMethod, string url, string authorisation, string partialContent)
    {
        var matchingRequests = fakeHttpClientHandler.Store
            .Where(s => s.Method == httpMethod && s.Url == url && s.Authorisation == authorisation && s.Content.Contains(partialContent));

        var matchingRequestCount = matchingRequests.Count();

        if (matchingRequestCount > 1)
        {
            throw new InvalidOperationException($"Requests matching {url}-{httpMethod}-{authorisation}-{partialContent} were found more than once");
        }

        return matchingRequestCount == 1;
    }

    public int GetRequestCount()
    {
        return fakeHttpClientHandler.Store.Count;
    }
}
