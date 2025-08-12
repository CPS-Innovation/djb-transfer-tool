using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

/// <summary>
/// Attribute to generate swagger documentation for highlighting a specific url parameter is required.
/// </summary>
public sealed class RequiredUrlParameterAttribute : OpenApiParameterAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequiredUrlParameterAttribute"/> class.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <exception cref="ArgumentNullException">When the name of the required parameter is null.</exception>
    public RequiredUrlParameterAttribute(string name, Type type)
        : base(name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name), "Non empty string required");
        }

        In = ParameterLocation.Path;
        Required = true;
        Type = type;
        Visibility = OpenApiVisibilityType.Important;
    }
}
