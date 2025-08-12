using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

namespace Cps.Fct.Hk.Common.Functions.UnitTests.Swagger.Attributes;

public class InternalServerErrorJsonAttributeTests
{
    [Fact]
    public void Attribute_OfType_OpenApiResponseWithBodyAttribute()
    {
        // Arrange & Act & Assert
        Assert.True(typeof(OpenApiResponseWithBodyAttribute)
            .IsAssignableFrom(typeof(InternalServerErrorJsonAttribute)));
    }

    [Fact]
    public void Ctor_WithTypeNameProvided_SetsCorrectly()
    {
        // Arrange
        var entityType = typeof(InternalServerErrorJsonAttribute);

        // Act
        var attr = new InternalServerErrorJsonAttribute(entityType);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, attr.StatusCode);
        Assert.Equal("application/json", attr.ContentType);
        Assert.Equal(entityType, attr.BodyType);
    }
}
