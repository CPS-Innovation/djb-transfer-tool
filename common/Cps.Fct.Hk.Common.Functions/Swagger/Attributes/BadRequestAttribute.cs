using System.Net;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

/// <summary>
/// Attribute to generate <see cref="HttpStatusCode.BadRequest"/> swagger documentation.
/// </summary>
public sealed class BadRequestAttribute : OpenApiResponseWithoutBodyAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestAttribute"/> class.
    /// </summary>
    /// <param name="description"></param>
    public BadRequestAttribute(string? description = null)
        : base(HttpStatusCode.BadRequest)
    {
        Description = string.IsNullOrWhiteSpace(description)
            ? "Bad Request"
            : description;
    }
}
