using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

namespace Cps.Fct.Hk.Common.Functions.UnitTests.Swagger.Attributes;

public class BearerAuthAttributeTests
{
    [Fact]
    public void Attribute_OfType_OpenApiSecurityAttribute()
    {
        // Arrange & Act & Assert
        Assert.True(typeof(OpenApiSecurityAttribute).IsAssignableFrom(typeof(BearerAuthAttribute)));
    }

    [Fact]
    public void Ctor_Default_SetsPropertiesCorrectly()
    {
        // Arrange & Act
        var attr = new BearerAuthAttribute();

        // Assert
        Assert.Equal("JWT", attr.BearerFormat);
        Assert.Equal(OpenApiSecuritySchemeType.Bearer, attr.Scheme);
        Assert.Equal("basic_auth", attr.SchemeName);
        Assert.Equal(SecuritySchemeType.Http, attr.SchemeType);
    }
}
