using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Cps.Fct.Hk.Common.Infrastructure.HealthChecks;

namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.HealthChecks;

public class AppSettingHealthCheckTests
{
    private readonly HealthCheckContext _context;
    private readonly Mock<IConfiguration> _configuration;
    private readonly AppSettingHealthCheck _sut;

    public AppSettingHealthCheckTests()
    {
        var healthCheck = new Mock<IHealthCheck>();
        var registration = new HealthCheckRegistration("Partner_AppSetting", healthCheck.Object, null, null);
        _context = new HealthCheckContext { Registration = registration };
        _configuration = new Mock<IConfiguration>();
        _sut = new AppSettingHealthCheck(_configuration.Object);
    }

    [Fact]
    public async Task CheckHealthAsync_AppSettingExists_ReturnsHealthyResponse()
    {
        // Arrange
        _configuration.Setup(c => c["Partner_AppSetting"]).Returns("PET");

        // Act
        var result = await _sut.CheckHealthAsync(_context);

        // Assert
        result.Status.ShouldBe(HealthStatus.Healthy);
        result.Description.ShouldBe("Successfully validated app setting Partner_AppSetting");
    }

    [Fact]
    public async Task CheckHealthAsync_AppSettingDoesNotExist_ReturnsHealthyResponse()
    {
        // Arrange
        _configuration.Setup(c => c["Partner_AppSetting"]).Returns(string.Empty);

        // Act
        var result = await _sut.CheckHealthAsync(_context);

        // Assert
        result.Status.ShouldBe(HealthStatus.Unhealthy);
        result.Description.ShouldBe("Error occurred validating app setting Partner_AppSetting as it is null or empty");
    }
}
