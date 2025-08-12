using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

namespace Cps.Fct.Hk.Common.Functions.UnitTests.Swagger.Attributes;

public class OkJsonResultAttributeTests
{
    [Fact]
    public void Attribute_OfType_OpenApiResponseWithBodyAttribute()
    {
        // Arrange & Act & Assert
        Assert.True(typeof(OpenApiResponseWithBodyAttribute).IsAssignableFrom(typeof(OkJsonResultAttribute)));
    }

    [Fact]
    public void Ctor_WithTypeNameProvided_SetsCorrectly()
    {
        // Arrange
        var entityType = typeof(OkJsonResultAttribute);

        // Act
        var attr = new OkJsonResultAttribute(entityType);

        // Assert
        Assert.Equal(HttpStatusCode.OK, attr.StatusCode);
        Assert.Equal("application/json", attr.ContentType);
        Assert.Equal(entityType, attr.BodyType);
    }
}
