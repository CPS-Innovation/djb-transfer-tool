using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

namespace Cps.Fct.Hk.Common.Functions.UnitTests.Swagger.Attributes;

public class SwaggerPropAttributeTests
{
    [Fact]
    public void Attribute_OfType_OpenApiPropertyAttribute()
    {
        // Arrange & Act & Assert
        Assert.True(typeof(OpenApiPropertyAttribute).IsAssignableFrom(typeof(SwaggerPropAttribute)));
    }

    [Fact]
    public void Ctor_WithDescriptionProvided_SetsCorrectly()
    {
        // Arrange
        const string description = "Property Description";

        // Act
        var attr = new SwaggerPropAttribute(description);

        // Assert
        Assert.Equal(description, attr.Description);
        Assert.False(attr.Nullable);
    }

    [Fact]
    public void Ctor_WithEmptyDescriptionProvided_SetsCorrectlyToEmptyString()
    {
        // Arrange
        var description = string.Empty;

        // Act
        var attr = new SwaggerPropAttribute(description);

        // Assert
        Assert.Equal(string.Empty, attr.Description);
        Assert.False(attr.Nullable);
    }
}
