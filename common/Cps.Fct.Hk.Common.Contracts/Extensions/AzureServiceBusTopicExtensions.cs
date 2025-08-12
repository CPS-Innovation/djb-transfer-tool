using System.Text.Json;
using Cps.Fct.Hk.Common.Contracts.ServiceBus;

namespace Cps.Fct.Hk.Common.Contracts.Extensions;

/// <summary>Provides extensions for the Azure Topic Service interface.</summary>
public static class AzureServiceBusTopicExtensions
{
    /// <summary>Convert a blob to json before uploading.</summary>
    /// <typeparam name="T">Type of object to serialise.</typeparam>
    /// <param name="azureBlobService"></param>
    /// <param name="messageObject"></param>
    /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task SendMessageAsJsonAsync<T>(this IAzureServiceBusTopic azureBlobService, T messageObject)
    {
        await azureBlobService.SendMessageAsync(JsonSerializer.Serialize(messageObject)).ConfigureAwait(false);
    }
}
