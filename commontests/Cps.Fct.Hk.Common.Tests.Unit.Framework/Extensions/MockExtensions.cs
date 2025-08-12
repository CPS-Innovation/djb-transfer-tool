// <copyright file="MockExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.Tests.Unit.Framework.Extensions;

using Moq;
using Moq.Protected;

public static class MockExtensions
{
    private const string HandlerSendMethodName = "SendAsync";

    public static Mock<HttpMessageHandler> SetupMockHttpMessageHandler(
        this Mock<HttpMessageHandler> handler,
        HttpResponseMessage httpResponseMessage,
        Action<HttpRequestMessage, CancellationToken>? callback)
    {
        var returnResult = handler.Protected()
            .Setup<Task<HttpResponseMessage>>(HandlerSendMethodName, ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);

        if (callback != null)
        {
            returnResult.Callback(callback);
        }

        return handler;
    }

    public static Mock<HttpMessageHandler> SetupMockHttpMessageHandler(
        this Mock<HttpMessageHandler> handler,
        HttpResponseMessage httpResponseMessage)
    {
        return handler.SetupMockHttpMessageHandler(httpResponseMessage, null);
    }

    public static void VerifySendAsyncCalled(this Mock<HttpMessageHandler> handler, int times)
    {
        var args = ItExpr.IsAny<HttpRequestMessage>();
        handler.Protected().Verify(HandlerSendMethodName, Times.Exactly(times), args, ItExpr.IsAny<CancellationToken>());
    }
}
