using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

namespace Cps.Fct.Hk.Common.Functions.UnitTests.Swagger.Attributes;

public class OkResultAttributeTests
{
    private const string DefaultDescriptionText = "OK";

    [Fact]
    public void Attribute_OfType_OpenApiResponseWithBodyAttribute()
    {
        // Arrange & Act & Assert
        Assert.True(typeof(OpenApiResponseWithoutBodyAttribute)
            .IsAssignableFrom(typeof(OkResultAttribute)));
    }

    [Fact]
    public void Ctor_Default_Ok()
    {
        // Arrange & Act
        var attr = new OkResultAttribute();

        // Assert
        Assert.Equal(HttpStatusCode.OK, attr.StatusCode);
        Assert.Equal(DefaultDescriptionText, attr.Description);
    }

    [Fact]
    public void Ctor_WithDescriptionProvided_SetsCorrectly()
    {
        // Arrange
        const string description = "Okie dokie!";

        // Act
        var attr = new OkResultAttribute(description);

        // Assert
        Assert.Equal(HttpStatusCode.OK, attr.StatusCode);
        Assert.Equal(description, attr.Description);
    }

    [Fact]
    public void Ctor_WithEmptyStringDescriptionProvided_UsesDefaultDescription()
    {
        // Arrange
        var description = string.Empty;

        // Act
        var attr = new OkResultAttribute(description);

        // Assert
        Assert.Equal(HttpStatusCode.OK, attr.StatusCode);
        Assert.Equal(DefaultDescriptionText, attr.Description);
    }
}
