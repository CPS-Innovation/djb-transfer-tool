// <copyright file="FakeServiceBusService{TOptions}.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.Tests.Unit.Framework.Fakes;
using System.Text.Json;
using Cps.Fct.Hk.Common.Contracts.ServiceBus;
using Cps.Fct.Hk.Common.Infrastructure.ServiceBus;

public class FakeServiceBusService<TOptions> : IAzureServiceBusTopic<TOptions>
    where TOptions : class, IAzureServiceBusTopicOptions
{
    private readonly ILogger<AzureServiceBusTopicService<TOptions>> logger;

    private readonly Dictionary<string, List<string>> store = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="FakeServiceBusService{TOptions}"/> class.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="options">Options.</param>
    public FakeServiceBusService(
        ILogger<AzureServiceBusTopicService<TOptions>> logger,
        IOptions<TOptions> options)
    {
        this.logger = logger;
        this.Options = options.Value;
        this.store.Add(Options.Topic, new());
    }

    public TOptions Options { get; }

    // Shouldn't be in this class
    public IEnumerable<string> GetTopicMessages()
    {
        return store.Single().Value;
    }

    public async Task SendMessageAsJsonAsync<T>(T messageObject)
        where T : class
    {
        await SendMessageAsync(JsonSerializer.Serialize(messageObject)).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public Task SendMessageAsync(string content)
    {
        store[Options.Topic].Add(content);

        logger.LogInformation("Completed sending {Content}", content);

        return Task.CompletedTask;
    }
}
