using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace Cps.Fct.Hk.Common.Functions.Swagger;

/// <summary>
/// Swagger document configuration options.
/// </summary>
[ExcludeFromCodeCoverage]
public class OpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions
{
    /// <inheritdoc/>
    public override OpenApiInfo Info { get; set; } = new()
    {
        Version = GetOpenApiDocVersion(),
        Title = GetOpenApiDocTitle()
    };

    /// <inheritdoc/>
    public override OpenApiVersionType OpenApiVersion { get; set; } = GetOpenApiVersion();
}
