using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

/// <summary>
/// Attribute to generate swagger documentation for highlighting the body is required.
/// </summary>
public sealed class RequiredBodyAttribute : OpenApiRequestBodyAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequiredBodyAttribute"/> class.
    /// </summary>
    /// <param name="type"></param>
    public RequiredBodyAttribute(Type type)
        : base("application/json", type)
    {
        Type = type;
        Required = true;
    }

    /// <summary>
    /// The type of object required in the request body.
    /// </summary>
    public Type Type { get; }
}
