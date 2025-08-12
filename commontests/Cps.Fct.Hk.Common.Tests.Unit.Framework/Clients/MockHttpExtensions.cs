// <copyright file="MockHttpExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.Tests.Unit.Framework.Clients;

using RichardSzalay.MockHttp;

/// <summary>
/// MockHttpExtensions.
/// </summary>
public static class MockHttpExtensions
{
    public static MockedRequest RespondWithJsonObject<T>(this MockedRequest source, T value, HttpStatusCode status = HttpStatusCode.OK)
    {
        var content = JsonSerializer.Serialize(value);

        return source.Respond(status, "application/json", content);
    }
}
