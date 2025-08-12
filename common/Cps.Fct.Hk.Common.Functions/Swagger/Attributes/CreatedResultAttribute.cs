using System.Net;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

/// <summary>
/// Attribute to generate <see cref="HttpStatusCode.Created"/> swagger documentation.
/// </summary>
public sealed class CreatedResultAttribute : OpenApiResponseWithoutBodyAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreatedResultAttribute"/> class.
    /// </summary>
    public CreatedResultAttribute()
        : base(HttpStatusCode.Created)
    {
    }
}
