using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

namespace Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

/// <summary>
/// Attribute to generate swagger documentation for an endpoint.
/// </summary>
public sealed class SwaggerOpAttribute : OpenApiOperationAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SwaggerOpAttribute"/> class.
    /// </summary>
    /// <param name="summary"></param>
    /// <param name="tags"></param>
    public SwaggerOpAttribute(string summary, params string[] tags)
        : base(operationId: null, tags)
    {
        Summary = string.IsNullOrWhiteSpace(summary) ? string.Empty : summary;
        Visibility = OpenApiVisibilityType.Important;
    }
}
