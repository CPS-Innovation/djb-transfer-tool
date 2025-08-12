using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

namespace Cps.Fct.Hk.Common.Functions.UnitTests.Swagger.Attributes;

public class RequiredBodyAttributeTests
{
    [Fact]
    public void Attribute_OfType_OpenApiRequestBodyAttribute()
    {
        // Arrange & Act & Assert
        Assert.True(typeof(OpenApiRequestBodyAttribute).IsAssignableFrom(typeof(RequiredBodyAttribute)));
    }

    [Fact]
    public void Ctor_WithTypeNameProvided_SetsCorrectly()
    {
        // Arrange
        var entityType = typeof(RequiredBodyAttribute);

        // Act
        var attr = new RequiredBodyAttribute(entityType);

        // Assert
        Assert.Equal("application/json", attr.ContentType);
        Assert.Equal(entityType, attr.Type);
        Assert.Equal(entityType, attr.BodyType);
        Assert.True(attr.Required);
    }
}
