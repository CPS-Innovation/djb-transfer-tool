using Cps.Fct.Hk.Common.Contracts.Configuration;
using Cps.Fct.Hk.Common.Contracts.ServiceBus;

namespace Cps.Fct.Hk.Common.Infrastructure.ServiceBus;

/// <summary>
/// EntityTopicOptions.
/// </summary>
public class EntityTopicOptions : IConfigurationOptions, IAzureServiceBusTopicOptions
{
    /// <summary>
    /// Topic connection string.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Topic.
    /// </summary>
    public string Topic { get; set; } = string.Empty;

    /// <summary>
    /// SectionName.
    /// </summary>
    public string? SectionName { get; } = nameof(EntityTopicOptions);
}
