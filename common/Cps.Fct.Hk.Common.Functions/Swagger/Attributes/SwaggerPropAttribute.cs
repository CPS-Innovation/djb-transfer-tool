using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

/// <summary>
/// Attribute to generate swagger documentation for a property.
/// </summary>
public sealed class SwaggerPropAttribute : OpenApiPropertyAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SwaggerPropAttribute"/> class.
    /// </summary>
    /// <param name="description"></param>
    public SwaggerPropAttribute(string description)
    {
        Description = string.IsNullOrWhiteSpace(description) ? string.Empty : description;
        Nullable = false;
    }
}
