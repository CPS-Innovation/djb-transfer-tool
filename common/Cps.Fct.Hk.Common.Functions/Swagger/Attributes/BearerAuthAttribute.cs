using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

/// <summary>
/// Bearer token attribute for decorating azure functions.
/// </summary>
public sealed class BearerAuthAttribute : OpenApiSecurityAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BearerAuthAttribute"/> class.
    /// </summary>
    public BearerAuthAttribute()
        : base("basic_auth", SecuritySchemeType.Http)
    {
        Scheme = OpenApiSecuritySchemeType.Bearer;
        BearerFormat = "JWT";
        Description = "JWT Authorization header using the Bearer scheme. Enter just the token without the 'Bearer' prefix";
    }
}
