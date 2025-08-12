// <copyright file="LogMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.Tests.Unit.Framework.Fakes;

using System.Globalization;

public class LogMessage
{
    public LogMessage(LogLevel logLevel, string message)
    {
        Level = logLevel;
        Message = message;
        Timestamp = DateTime.Now;
    }

    public DateTime Timestamp { get; }

    public string Message { get; }

    public LogLevel Level { get; }

    public override string? ToString()
    {
        return $"[{Level.ToString()[..3].ToUpperInvariant()}] [{Timestamp.ToString(Defaults.TimestampFormat, CultureInfo.InvariantCulture)}]: {Message}";
    }
}
