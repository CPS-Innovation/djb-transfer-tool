namespace Cps.Fct.Hk.Common.Contracts.ServiceBus;

/// <summary>Interface allowing to interact with topics in an Azure Service Bus.</summary>
public interface IAzureServiceBusTopic
{
    /// <summary>Sends a message to a topic.</summary>
    /// <param name="content"></param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SendMessageAsync(string content);
}
