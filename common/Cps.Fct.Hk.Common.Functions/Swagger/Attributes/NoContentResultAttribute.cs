using System.Net;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

/// <summary>
/// Attribute to generate <see cref="HttpStatusCode.NoContent"/> swagger documentation.
/// </summary>
public sealed class NoContentResultAttribute : OpenApiResponseWithoutBodyAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NoContentResultAttribute"/> class.
    /// </summary>
    /// <param name="description"></param>
    public NoContentResultAttribute(string? description = null)
        : base(HttpStatusCode.NoContent)
    {
        Description = string.IsNullOrWhiteSpace(description)
            ? "No Content"
            : description;
    }
}
