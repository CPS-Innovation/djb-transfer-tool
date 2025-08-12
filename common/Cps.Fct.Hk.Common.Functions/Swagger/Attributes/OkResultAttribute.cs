using System.Net;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

/// <summary>
/// Attribute to generate <see cref="HttpStatusCode.OK"/> swagger documentation.
/// </summary>
public sealed class OkResultAttribute : OpenApiResponseWithoutBodyAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OkResultAttribute"/> class.
    /// </summary>
    /// <param name="description"></param>
    public OkResultAttribute(string? description = null)
        : base(HttpStatusCode.OK)
    {
        Description = string.IsNullOrWhiteSpace(description)
            ? "OK"
            : description;
    }
}
