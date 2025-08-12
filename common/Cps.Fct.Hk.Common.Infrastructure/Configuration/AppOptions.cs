using System.ComponentModel.DataAnnotations;
using Cps.Fct.Hk.Common.Contracts.Configuration;
using Cps.Fct.Hk.Common.Contracts.Cors;
    
namespace Cps.Fct.Hk.Common.Infrastructure.Configuration;

/// <summary>
/// Provides options for your application configuration.
/// </summary>
public class AppOptions : IConfigurationOptions
{
    /// <inheritdoc/>
    public string? SectionName => "Application";

    /// <summary>
    /// Application Name.
    /// </summary>
    [Required]
    public string AppName { get; set; } = string.Empty;

    /// <summary>
    /// Application Display Name.
    /// </summary>
    [Required]
    public string ApplicationDisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Flag to determine if the configuration store should be used.
    /// </summary>
    [Required]
    public bool UseAzureAppConfiguration { get; set; } = false;

    /// <summary>
    /// Flag to determine if all global keys should be retrieved from the configuration store.
    /// </summary>
    [Required]
    public bool UseGlobalConfiguration { get; set; } = false;

    /// <summary>
    /// Flag to determine if Cors should be used.
    /// </summary>
    [Required]
    public bool UseCors { get; set; } = true;

    /// <summary>
    /// Cors Policy.
    /// </summary>
    [Required]
    public CorsPolicy CorsPolicy { get; set; } = new();
}
