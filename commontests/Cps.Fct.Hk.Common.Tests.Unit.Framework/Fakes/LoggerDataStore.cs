// <copyright file="LoggerDataStore.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.Tests.Unit.Framework.Fakes;

public class LoggerDataStore
{
    public List<LogMessage> Store { get; } = new();

    public void Log(LogLevel level, string message)
    {
        Store.Add(new LogMessage(level, message));
    }
}
