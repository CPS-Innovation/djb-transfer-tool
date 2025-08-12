using System.ComponentModel.DataAnnotations;
using Cps.Fct.Hk.Common.Contracts.Configuration;

namespace Cps.Fct.Hk.Common.Infrastructure.Configuration;

/// <summary>
/// Provides options for configuring application logging.
/// </summary>
public class LoggingOptions : IConfigurationOptions
{
    /// <inheritdoc/>
    public string? SectionName => "Application";

    /// <summary>
    /// Application Name.
    /// </summary>
    [Required]
    public string ApplicationName { get; set; } = "DocGen";
}
