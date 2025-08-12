namespace Cps.Fct.Hk.Common.Contracts.ServiceBus;
/// <summary>
/// Configuration options when working with <see cref="IAzureServiceBusTopic{TOptions}"/>.
/// </summary>
public interface IAzureServiceBusTopicOptions
{
    /// <summary>
    /// Connection String to service bus.
    /// </summary>
    public string ConnectionString { get; }

    /// <summary>
    /// Topic name within service bus.
    /// </summary>
    public string Topic { get; }
}
