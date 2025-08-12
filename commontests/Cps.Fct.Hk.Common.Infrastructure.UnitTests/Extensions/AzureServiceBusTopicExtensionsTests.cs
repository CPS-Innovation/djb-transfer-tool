namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Extensions;
using AutoFixture;
using AutoFixture.AutoMoq;
using Cps.Fct.Hk.Common.Contracts.ServiceBus;

public class AzureServiceBusTopicExtensionsTests
{
    private readonly IFixture _fixture;

    public AzureServiceBusTopicExtensionsTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
    }

    [Fact]
    public async Task UploadAsJsonAsync_WhenCalled_CallsUploadAsync()
    {
        // Arrange
        var sut = new Mock<IAzureServiceBusTopic>();
        var messageObject = _fixture.Create<IAzureServiceBusTopicOptions>();
        var messageObjectSerialised = JsonSerializer.Serialize(messageObject);

        // Act
        await sut.Object.SendMessageAsJsonAsync(messageObject);

        // Assert
        sut.Verify(s => s.SendMessageAsync(messageObjectSerialised), Times.Once);
    }
}
