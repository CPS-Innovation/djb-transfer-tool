using System.Net;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

/// <summary>
/// Attribute to generate <see cref="HttpStatusCode.OK"/> swagger documentation.
/// </summary>
public sealed class OkJsonResultAttribute : OpenApiResponseWithBodyAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OkJsonResultAttribute"/> class.
    /// </summary>
    /// <param name="bodyType"></param>
    public OkJsonResultAttribute(Type bodyType)
        : base(HttpStatusCode.OK, "application/json", bodyType)
    {
    }
}
