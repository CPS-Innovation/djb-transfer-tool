using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

namespace Cps.Fct.Hk.Common.Functions.UnitTests.Swagger.Attributes;

public class NotFoundAttributeTests
{
    [Fact]
    public void Attribute_OfType_OpenApiResponseWithoutBodyAttribute()
    {
        // Arrange & Act & Assert
        Assert.True(typeof(OpenApiResponseWithoutBodyAttribute).IsAssignableFrom(typeof(NotFoundAttribute)));
    }

    [Fact]
    public void Ctor_WithEntityNameProvided_SetsCorrectly()
    {
        // Arrange
        const string entityName = "Customer";

        // Act
        var attr = new NotFoundAttribute(entityName);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, attr.StatusCode);
        Assert.Equal(entityName, attr.EntityName);
        Assert.Equal("Customer not found", attr.Description);
    }

    [Fact]
    public void Ctor_WithInvalidEntityNameProvided_ThrowsException()
    {
        // Act
        var ex = Assert.Throws<ArgumentNullException>(
            () => new NotFoundAttribute(string.Empty));

        // Assert
        Assert.Equal("entityName", ex.ParamName);
        Assert.Equal("Non empty string required (Parameter 'entityName')", ex.Message);
    }
}
