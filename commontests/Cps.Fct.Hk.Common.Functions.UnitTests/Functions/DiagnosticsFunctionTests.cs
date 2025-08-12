using Cps.Fct.Hk.Common.Functions.Abstractions;
using Cps.Fct.Hk.Common.Functions.Functions;
using Cps.Fct.Hk.Common.Functions.Swagger.Attributes;
using Cps.Fct.Hk.Common.Functions.UnitTests.Helpers;

namespace Cps.Fct.Hk.Common.Functions.UnitTests.Functions;

public class DiagnosticsFunctionTests
{
    [Fact]
    public void Version_HasFunctionAttribute()
    {
        // Arrange & Act
        var attribute = FunctionTestHelpers.MethodHasSingleAttribute<DiagnosticsFunction, FunctionAttribute>(
            nameof(DiagnosticsFunction.VersionAsync));

        // Assert
        Assert.Equal("Version", attribute.Name);
    }

    [Fact]
    public void Version_HasSwaggerOpAttribute()
    {
        // Arrange & Act
        var attribute = FunctionTestHelpers.MethodHasSingleAttribute<DiagnosticsFunction, SwaggerOpAttribute>(
            nameof(DiagnosticsFunction.VersionAsync));

        // Assert
        attribute.Tags.ShouldHaveSingleItem();
        attribute.Tags[0].ShouldBe("Diagnostics");
    }

    [Fact]
    public void Version_HasOkJsonResultAttribute()
    {
        // Arrange & Act
        var attribute = FunctionTestHelpers.MethodHasSingleAttribute<DiagnosticsFunction, OkJsonResultAttribute>(
            nameof(DiagnosticsFunction.VersionAsync));

        // Assert
        attribute.BodyType.ShouldBe(typeof(string));
    }

    [Fact]
    public void Version_HasHttpTriggerAttributeWithCorrectValues()
    {
        // Arrange & Act & Assert
        FunctionTestHelpers.Function_HasHttpTriggerAttributeWithCorrectValues<DiagnosticsFunction>(
            nameof(DiagnosticsFunction.VersionAsync),
            "v1/diagnostics/version",
            new[] { "GET" },
            AuthorizationLevel.Anonymous);
    }

    [Fact]
    public async Task Version_ReturnsExpectedResult()
    {
        // Arrange
        var ctx = new HttpFunctionTestContext();
        const HttpStatusCode statusCode = HttpStatusCode.OK;

        var assemblyInfoProvider = new Mock<IAssemblyInfoProvider>();
        var versionInfo = VersionInformation.Default();
        assemblyInfoProvider.Setup(x => x.VersionInfo).Returns(versionInfo).Verifiable();

        var httpDataSerializerMock = new Mock<IHttpDataSerializer>();
        httpDataSerializerMock
            .Setup(x => x.WriteResponseBodyAsync(ctx.Response.Object, versionInfo))
            .Callback<HttpResponseData, VersionInformation>((res, version) =>
                ctx.Response
                    .Setup(x => x.Body)
                    .Returns(new MemoryStream(Encoding.UTF8.GetBytes("json version"))));
        var sut = new DiagnosticsFunction(assemblyInfoProvider.Object, httpDataSerializerMock.Object);

        // Act
        var result = await sut.VersionAsync(ctx.Request.Object);

        // Assert
        result.ShouldNotBeNull();
        var bodyText = await result.Body.StreamToStringAsync();
        bodyText.ShouldBe("json version");

        ctx.Request.Verify(rd => rd.CreateResponse(), Times.Once);
        ctx.Response.VerifySet(
            rd => rd.StatusCode = It.Is<HttpStatusCode>(sc => sc == statusCode),
            Times.Once);
        result.ShouldBe(ctx.Response.Object);
    }
}
