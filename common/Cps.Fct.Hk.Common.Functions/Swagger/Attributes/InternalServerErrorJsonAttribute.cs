using System.Net;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

/// <summary>
/// Attribute to generate <see cref="HttpStatusCode.InternalServerError"/> swagger documentation.
/// </summary>
public sealed class InternalServerErrorJsonAttribute : OpenApiResponseWithBodyAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InternalServerErrorJsonAttribute"/> class.
    /// </summary>
    /// <param name="bodyType"></param>
    public InternalServerErrorJsonAttribute(Type bodyType)
        : base(HttpStatusCode.InternalServerError, "application/json", bodyType)
    {
    }
}
