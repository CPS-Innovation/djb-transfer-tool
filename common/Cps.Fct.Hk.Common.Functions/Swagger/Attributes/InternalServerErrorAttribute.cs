using System.Net;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

/// <summary>
/// Attribute to generate <see cref="HttpStatusCode.InternalServerError"/> swagger documentation.
/// </summary>
public sealed class InternalServerErrorAttribute : OpenApiResponseWithoutBodyAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InternalServerErrorAttribute"/> class.
    /// </summary>
    /// <param name="description"></param>
    public InternalServerErrorAttribute(string? description = null)
        : base(HttpStatusCode.InternalServerError)
    {
        Description = string.IsNullOrWhiteSpace(description)
            ? "Internal Server Error"
            : description;
    }
}
