using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

namespace Cps.Fct.Hk.Common.Functions.UnitTests.Swagger.Attributes;

public class RequiredUrlParameterAttributeTests
{
    [Fact]
    public void Attribute_OfType_OpenApiParameterAttribute()
    {
        // Arrange & Act & Assert
        Assert.True(typeof(OpenApiParameterAttribute).IsAssignableFrom(typeof(RequiredUrlParameterAttribute)));
    }

    [Fact]
    public void Ctor_WithInvalidParameterNameProvided_ThrowsException()
    {
        // Act
        var ex = Assert.Throws<ArgumentNullException>(
            () => new RequiredUrlParameterAttribute(string.Empty, typeof(string)));

        // Assert
        Assert.Equal("name", ex.ParamName);
        Assert.Equal("Non empty string required (Parameter 'name')", ex.Message);
    }

    [Fact]
    public void Ctor_WithParameterDetailsProvided_SetsCorrectly()
    {
        // Arrange
        const string parameterName = "globalCustomerId";
        var parameterType = typeof(string);

        // Act
        var attr = new RequiredUrlParameterAttribute(parameterName, parameterType);

        // Assert
        Assert.Equal(parameterName, attr.Name);
        Assert.Equal(ParameterLocation.Path, attr.In);
        Assert.True(attr.Required);
        Assert.Equal(parameterType, attr.Type);
        Assert.Equal(OpenApiVisibilityType.Important, attr.Visibility);
    }
}
