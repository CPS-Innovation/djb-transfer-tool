using System.Diagnostics.CodeAnalysis;
using Azure.Messaging.ServiceBus;
using Cps.Fct.Hk.Common.Contracts.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cps.Fct.Hk.Common.Infrastructure.ServiceBus;

/// <summary>
/// Generic service that allows the interaction with an Azure Service Bus Topic.
/// </summary>
/// <typeparam name="TOptions">Configuration options.</typeparam>
[ExcludeFromCodeCoverage]
public class AzureServiceBusTopicService<TOptions> : IAzureServiceBusTopic<TOptions>
    where TOptions : class, IAzureServiceBusTopicOptions
{
    private readonly ILogger<AzureServiceBusTopicService<TOptions>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureServiceBusTopicService{TOptions}"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public AzureServiceBusTopicService(
        ILogger<AzureServiceBusTopicService<TOptions>> logger,
        IOptions<TOptions> options)
    {
        _logger = logger;
        Options = options.Value;
    }

    /// <inheritdoc/>
    public TOptions Options { get; }

    /// <inheritdoc/>
    public async Task SendMessageAsync(string content)
    {
        _logger.LogInformation("Beginning sending message to {Topic} with content {Content}", Options.Topic, content);

        await using var client = new ServiceBusClient(Options.ConnectionString).ConfigureAwait(false);
        _logger.LogInformation("Completed sending message to {Topic} with content {Content}", Options.Topic, content);
        throw new NotImplementedException("method not implemented");
    }
}
