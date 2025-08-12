// <copyright file="HttpRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.Tests.Unit.Framework.Fakes;

using System.Globalization;

public class HttpRequest
{
    public HttpRequest(string uri, HttpMethod method, string authorisation, string body)
    {
        Timestamp = DateTime.Now;
        Url = uri;
        Method = method;
        Authorisation = authorisation;
        Content = body;
    }

    public DateTime Timestamp { get; }

    public string Url { get; }

    public HttpMethod Method { get; }

    public string Authorisation { get; }

    public string Content { get; }

    public override string? ToString()
    {
        return $"[{Url.ToUpperInvariant()}] [{Method.ToString().ToUpperInvariant()}] [{Timestamp.ToString(Defaults.TimestampFormat, CultureInfo.InvariantCulture)}]: {Content}";
    }
}
