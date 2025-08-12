using System.Net;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

/// <summary>
/// Attribute to generate <see cref="HttpStatusCode.NotFound"/> swagger documentation.
/// </summary>
public sealed class NotFoundAttribute : OpenApiResponseWithoutBodyAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundAttribute"/> class.
    /// </summary>
    /// <param name="entityName"></param>
    /// <exception cref="ArgumentNullException">When no entityname is provided.</exception>
    public NotFoundAttribute(string entityName)
        : base(HttpStatusCode.NotFound)
    {
        if (string.IsNullOrWhiteSpace(entityName))
        {
            throw new ArgumentNullException(nameof(entityName), "Non empty string required");
        }

        EntityName = entityName;
        Description = $"{entityName} not found";
    }

    /// <summary>
    /// The entity that was not found.
    /// </summary>
    public string EntityName { get; }
}
