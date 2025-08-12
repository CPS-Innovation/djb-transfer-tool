namespace Cps.Fct.Hk.Common.Contracts.ServiceBus;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Interface allowing to interact with topics in an Azure Service Bus.
/// </summary>
/// <typeparam name="TOptions">Options for configuring the topic.</typeparam>
public interface IAzureServiceBusTopic<out TOptions> : IAzureServiceBusTopic
    where TOptions : class, IAzureServiceBusTopicOptions
{
    /// <summary>
    /// Hack for DI registrations. Should find a better way like typed registrations
    /// a factory approach, etc.
    /// </summary>
    [ExcludeFromCodeCoverage]
    Type OptionsType => typeof(TOptions);

    /// <summary>
    /// Exposes the service options.
    /// </summary>
    TOptions Options { get; }
}
