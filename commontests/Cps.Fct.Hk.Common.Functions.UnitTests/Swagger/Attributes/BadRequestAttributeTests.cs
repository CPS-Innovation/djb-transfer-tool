using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

namespace Cps.Fct.Hk.Common.Functions.UnitTests.Swagger.Attributes;

public class BadRequestAttributeTests
{
    [Fact]
    public void Attribute_OfType_OpenApiResponseWithoutBodyAttribute()
    {
        // Arrange & Act & Assert
        Assert.True(typeof(OpenApiResponseWithoutBodyAttribute).IsAssignableFrom(typeof(BadRequestAttribute)));
    }

    [Fact]
    public void Ctor_Default_BadRequest()
    {
        // Arrange & Act
        var attr = new BadRequestAttribute();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, attr.StatusCode);
        Assert.Equal("Bad Request", attr.Description);
    }

    [Fact]
    public void Ctor_WithDescriptionProvided_SetsCorrectly()
    {
        // Arrange
        const string description = "Field should not be null";

        // Act
        var attr = new BadRequestAttribute(description);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, attr.StatusCode);
        Assert.Equal(description, attr.Description);
    }

    [Fact]
    public void Ctor_WithEmptyStringDescriptionProvided_UsesDefaultDescription()
    {
        // Arrange
        var description = string.Empty;

        // Act
        var attr = new BadRequestAttribute(description);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, attr.StatusCode);
        Assert.Equal("Bad Request", attr.Description);
    }
}
