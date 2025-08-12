// <copyright file="FakeHttpClientHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.Tests.Unit.Framework.Fakes;

public class FakeHttpClientHandler : HttpMessageHandler
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FakeHttpClientHandler"/> class.
    /// </summary>
    public FakeHttpClientHandler()
    {
        this.Store = new();
    }

    public List<HttpRequest> Store { get; }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var content = await request!.Content!.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var httpMessage = new HttpRequest(request!.RequestUri!.AbsoluteUri, request.Method, request.Headers!.Authorization!.ToString(), content);
        Store.Add(httpMessage);

        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
        return httpResponseMessage;
    }
}
