using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

/// <summary>
/// Attribute to generate swagger documentation for highlighting that a property is nullable.
/// </summary>
public sealed class SwaggerNullPropAttribute : OpenApiPropertyAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SwaggerNullPropAttribute"/> class.
    /// </summary>
    /// <param name="description"></param>
    public SwaggerNullPropAttribute(string description)
    {
        Description = string.IsNullOrWhiteSpace(description) ? string.Empty : description;
        Nullable = true;
    }
}
