using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

namespace Cps.Fct.Hk.Common.Functions.UnitTests.Swagger.Attributes;

public class CreatedResultAttributeTests
{
    [Fact]
    public void Attribute_OfType_OpenApiResponseWithoutBodyAttribute()
    {
        // Arrange & Act & Assert
        Assert.True(typeof(OpenApiResponseWithoutBodyAttribute).IsAssignableFrom(typeof(CreatedResultAttribute)));
    }

    [Fact]
    public void Ctor_WithTypeNameProvided_SetsCorrectly()
    {
        // Arrange & Act
        var attr = new CreatedResultAttribute();

        // Assert
        Assert.Equal(HttpStatusCode.Created, attr.StatusCode);
    }
}
