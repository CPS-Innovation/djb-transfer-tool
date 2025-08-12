using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

namespace Cps.Fct.Hk.Common.Functions.UnitTests.Swagger.Attributes;

public class SwaggerOpAttributeTests
{
    [Fact]
    public void Attribute_OfType_OpenApiParameterAttribute()
    {
        // Arrange & Act & Assert
        Assert.True(typeof(OpenApiOperationAttribute).IsAssignableFrom(typeof(SwaggerOpAttribute)));
    }

    [Fact]
    public void Ctor_WithEmptySummaryProvided_SetsCorrectly()
    {
        // Arrange
        var summary = string.Empty;

        // Act
        var attr = new SwaggerOpAttribute(summary);

        // Assert
        Assert.Equal(string.Empty, attr.Summary);
        Assert.Empty(attr.Tags);
        Assert.Equal(OpenApiVisibilityType.Important, attr.Visibility);
    }

    [Fact]
    public void Ctor_WithSummaryAndTagProvided_SetsCorrectly()
    {
        // Arrange
        const string description = "Some endpoint operation description";
        const string tag1 = "tag1";

        // Act
        var attr = new SwaggerOpAttribute(description, tag1);

        // Assert
        Assert.Equal(description, attr.Summary);
        Assert.NotEmpty(attr.Tags);
        Assert.Single(attr.Tags);
        Assert.Equal(tag1, attr.Tags[0]);
        Assert.Equal(OpenApiVisibilityType.Important, attr.Visibility);
    }

    [Fact]
    public void Ctor_WithSummaryAndTagsProvided_SetsCorrectly()
    {
        // Arrange
        const string description = "Some endpoint operation description";
        const string tag1 = "tag1";
        const string tag2 = "tag2";

        // Act
        var attr = new SwaggerOpAttribute(description, tag1, tag2);

        // Assert
        Assert.Equal(description, attr.Summary);
        Assert.NotEmpty(attr.Tags);
        Assert.Equal(2, attr.Tags.Length);
        Assert.Equal(tag1, attr.Tags[0]);
        Assert.Equal(tag2, attr.Tags[1]);
        Assert.Equal(OpenApiVisibilityType.Important, attr.Visibility);
    }

    [Fact]
    public void Ctor_WithSummaryProvided_SetsCorrectly()
    {
        // Arrange
        const string description = "Some endpoint operation description";

        // Act
        var attr = new SwaggerOpAttribute(description);

        // Assert
        Assert.Equal(description, attr.Summary);
        Assert.Empty(attr.Tags);
        Assert.Equal(OpenApiVisibilityType.Important, attr.Visibility);
    }
}
