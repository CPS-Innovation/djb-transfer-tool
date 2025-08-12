using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

namespace Cps.Fct.Hk.Common.Functions.UnitTests.Swagger.Attributes;

public class InternalServerErrorAttributeTests
{
    private const string DefaultDescriptionText = "Internal Server Error";

    [Fact]
    public void Attribute_OfType_OpenApiResponseWithoutBodyAttribute()
    {
        // Arrange & Act & Assert
        Assert.True(typeof(OpenApiResponseWithoutBodyAttribute)
            .IsAssignableFrom(typeof(InternalServerErrorAttribute)));
    }

    [Fact]
    public void Ctor_Default_InternalServerError()
    {
        // Arrange & Act
        var attr = new InternalServerErrorAttribute();

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, attr.StatusCode);
        Assert.Equal(DefaultDescriptionText, attr.Description);
    }

    [Fact]
    public void Ctor_WithDescriptionProvided_SetsCorrectly()
    {
        // Arrange
        const string description = "One or more critical dependencies are unavailable.";

        // Act
        var attr = new InternalServerErrorAttribute(description);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, attr.StatusCode);
        Assert.Equal(description, attr.Description);
    }

    [Fact]
    public void Ctor_WithEmptyStringDescriptionProvided_UsesDefaultDescription()
    {
        // Arrange
        var description = string.Empty;

        // Act
        var attr = new InternalServerErrorAttribute(description);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, attr.StatusCode);
        Assert.Equal(DefaultDescriptionText, attr.Description);
    }
}
