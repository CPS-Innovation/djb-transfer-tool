using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

namespace Cps.Fct.Hk.Common.Functions.UnitTests.Swagger.Attributes;

public class NoContentResultAttributeTests
{
    private const string DefaultDescriptionText = "No Content";

    [Fact]
    public void Attribute_OfType_OpenApiResponseWithBodyAttribute()
    {
        // Arrange & Act & Assert
        Assert.True(typeof(OpenApiResponseWithoutBodyAttribute)
            .IsAssignableFrom(typeof(NoContentResultAttribute)));
    }

    [Fact]
    public void Ctor_Default_SetsDefaultValues()
    {
        // Arrange & Act
        var attr = new NoContentResultAttribute();

        // Assert
        attr.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        attr.Description.ShouldBe(DefaultDescriptionText);
    }

    [Fact]
    public void Ctor_WithDescriptionProvided_SetsCorrectly()
    {
        // Arrange
        const string description = "Some description";

        // Act
        var attr = new NoContentResultAttribute(description);

        // Assert
        attr.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        attr.Description.ShouldBe(description);
    }

    [Fact]
    public void Ctor_WithEmptyStringDescriptionProvided_UsesDefaultDescription()
    {
        // Arrange
        var description = string.Empty;

        // Act
        var attr = new NoContentResultAttribute(description);

        // Assert
        attr.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        attr.Description.ShouldBe(DefaultDescriptionText);
    }
}
