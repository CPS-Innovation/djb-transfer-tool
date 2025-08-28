// <copyright file="ResponseHeaderHelperTests.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Tests.Utils;

using Cps.Fct.Djb.TransferToolApi.Utils;
using Microsoft.AspNetCore.Http;
using Xunit;

/// <summary>
/// Unit tests for the <see cref="ResponseHeaderHelper"/> class.
/// </summary>
public class ResponseHeaderHelperTests
{
    /// <summary>
    /// Test to ensure that SetNoCacheHeaders sets Cache-Control and Pragma headers correctly.
    /// </summary>
    [Fact]
    public void SetNoCacheHeaders_ShouldSetCacheControlAndPragmaHeaders()
    {
        // Arrange
        HttpResponse mockResponse = new DefaultHttpContext().Response;

        // Act
        ResponseHeaderHelper.SetNoCacheHeaders(mockResponse);

        // Assert
        Assert.Equal("no-store, no-cache", mockResponse.Headers["Cache-Control"].ToString());
        Assert.Equal("no-cache", mockResponse.Headers["Pragma"].ToString());
    }

    /// <summary>
    /// Test to ensure that SetSecurityHeaders sets the appropriate security headers.
    /// </summary>
    [Fact]
    public void SetSecurityHeaders_ShouldSetSecurityRelatedHeaders()
    {
        // Arrange
        HttpResponse mockResponse = new DefaultHttpContext().Response;

        // Act
        ResponseHeaderHelper.SetSecurityHeaders(mockResponse);

        // Assert
        Assert.Equal("default-src 'self'; script-src 'self'; object-src 'none'; frame-ancestors 'none';", mockResponse.Headers["Content-Security-Policy"].ToString());
        Assert.Equal("nosniff", mockResponse.Headers["X-Content-Type-Options"].ToString());
        Assert.Equal("max-age=31536000; includeSubDomains; preload", mockResponse.Headers["Strict-Transport-Security"].ToString());
        Assert.Equal("no-referrer", mockResponse.Headers["Referrer-Policy"].ToString());
        Assert.Equal("geolocation=(), camera=(), microphone=(), payment=()", mockResponse.Headers["Permissions-Policy"].ToString());
    }

    /// <summary>
    /// Test to ensure that SetCacheHeaders sets the Cache-Control header correctly for the default duration.
    /// </summary>
    [Fact]
    public void SetCacheHeaders_ShouldSetCacheControlHeaderForDefaultDuration()
    {
        // Arrange
        HttpResponse mockResponse = new DefaultHttpContext().Response;

        // Act
        ResponseHeaderHelper.SetCacheHeaders(mockResponse);

        // Assert
        Assert.Equal("public, max-age=900", mockResponse.Headers["Cache-Control"].ToString());
    }

    /// <summary>
    /// Test to ensure that SetCacheHeaders sets the Cache-Control header correctly for a custom duration.
    /// </summary>
    [Fact]
    public void SetCacheHeaders_ShouldSetCacheControlHeaderForCustomDuration()
    {
        // Arrange
        HttpResponse mockResponse = new DefaultHttpContext().Response;
        int customDuration = 1800; // 30 minutes

        // Act
        ResponseHeaderHelper.SetCacheHeaders(mockResponse, customDuration);

        // Assert
        Assert.Equal($"public, max-age={customDuration}", mockResponse.Headers["Cache-Control"].ToString());
    }
}
