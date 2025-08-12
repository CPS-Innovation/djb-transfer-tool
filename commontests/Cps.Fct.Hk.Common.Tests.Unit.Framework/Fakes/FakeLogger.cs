// <copyright file="FakeLogger.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.Tests.Unit.Framework.Fakes;
public class FakeLogger<T> : ILogger<T>
{
    private readonly LoggerDataStore dataStore;

    public FakeLogger(LoggerDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var message = state != null ? state!.ToString() : string.Empty;
        dataStore.Log(logLevel, message ?? string.Empty);

        if (exception != null)
        {
            dataStore.Log(logLevel, exception.Message);
        }
    }
}
