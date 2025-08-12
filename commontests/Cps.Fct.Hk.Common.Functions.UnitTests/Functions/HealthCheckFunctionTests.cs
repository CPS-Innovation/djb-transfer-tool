using Microsoft.Extensions.Diagnostics.HealthChecks;
using Cps.Fct.Hk.Common.Functions.Functions;
using Cps.Fct.Hk.Common.Functions.UnitTests.Helpers;

namespace Cps.Fct.Hk.Common.Functions.UnitTests.Functions;

public class HealthCheckFunctionTests
{
    private readonly Mock<HealthCheckService> _healthCheckService;
    private readonly HealthCheckFunction _sut;

    public HealthCheckFunctionTests()
    {
        _healthCheckService = new Mock<HealthCheckService>();
        _sut = new HealthCheckFunction(_healthCheckService.Object);
    }

    [Fact]
    public void RunAsync_HasFunctionAttribute()
    {
        // Arrange & Act
        var attribute = FunctionTestHelpers.MethodHasSingleAttribute<HealthCheckFunction, FunctionAttribute>(
            nameof(HealthCheckFunction.RunAsync));

        // Assert
        attribute.Name.ShouldBe("HealthCheckFunction");
    }

    [Fact]
    public void RunAsync_HasHttpTriggerAttributeWithCorrectValues()
    {
        // Arrange & Act & Assert
        FunctionTestHelpers.Function_HasHttpTriggerAttributeWithCorrectValues<HealthCheckFunction>(
            nameof(HealthCheckFunction.RunAsync),
            "health",
            new[] { "GET" },
            AuthorizationLevel.Anonymous);
    }

    [Fact]
    public async Task RunAsync_ValidHealthCheck_ReturnsOkResponse()
    {
        // Arrange
        var body = new MemoryStream(Encoding.UTF8.GetBytes(string.Empty));
        var req = new FakeHttpRequestData(new Mock<FunctionContext>().Object, new Uri("https://test/api/message"), body);
        var healthReport = new HealthReport(new Dictionary<string, HealthReportEntry>(), HealthStatus.Healthy, TimeSpan.FromSeconds(1));
        _healthCheckService.Setup(s => s.CheckHealthAsync(null, CancellationToken.None)).ReturnsAsync(healthReport);

        // Act
        var result = await _sut.RunAsync(req);

        // Assert
        Assert.NotNull(result);
        var bodyText = await result.Body.StreamToStringAsync();
        Assert.Equal((int)HttpStatusCode.OK, (int)result.StatusCode);
        Assert.Equal("Healthy", bodyText);
    }

    [Fact]
    public async Task RunAsync_InvalidHealthCheck_ReturnsInternalServerErrorResponse()
    {
        // Arrange
        var body = new MemoryStream(Encoding.UTF8.GetBytes(string.Empty));
        var req = new FakeHttpRequestData(new Mock<FunctionContext>().Object, new Uri("https://test/api/message"), body);
        var healthReport = new HealthReport(new Dictionary<string, HealthReportEntry>(), HealthStatus.Unhealthy, TimeSpan.FromSeconds(1));
        _healthCheckService.Setup(s => s.CheckHealthAsync(null, CancellationToken.None)).ReturnsAsync(healthReport);

        // Act
        var result = await _sut.RunAsync(req);

        // Assert
        Assert.NotNull(result);
        var bodyText = await result.Body.StreamToStringAsync();
        Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.Equal(/*lang=json,strict*/ "{\"status\":\"Unhealthy\",\"totalDurationSeconds\":\"1.000\",\"results\":[]}", bodyText);
    }
}
