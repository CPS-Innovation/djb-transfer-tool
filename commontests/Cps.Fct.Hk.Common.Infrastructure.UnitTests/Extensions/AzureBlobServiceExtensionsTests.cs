namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Extensions;

using Microsoft.Extensions.Logging;
using Cps.Fct.Hk.Common.Contracts.BlobStorage;
using Cps.Fct.Hk.Common.Contracts.Extensions;

public class AzureBlobServiceExtensionsTests
{
    [Fact]
    public async Task UploadAsJsonAsync_WhenCalled_CallsUploadAsync()
    {
        // Arrange
        var sut = new Mock<IAzureBlobService>();
        const string container = "test-container";
        const string blobName = "test-blob-name";
        var blobObject = new AzureBlobStorageOptionsBase
        {
            ConnectionString = "my-conn-string"
        };
        var blobObjectSerialised = JsonSerializer.Serialize(blobObject);

        // Act
        await sut.Object.UploadAsJsonAsync(container, blobName, blobObject);

        // Assert
        sut.Verify(s => s.UploadAsync(container, blobName, blobObjectSerialised), Times.Once);
    }

    [Fact]
    public async Task UploadBlobAsync_WhenCalled_CallsUploadAsync()
    {
        // Arrange
        var sut = new Mock<IAzureBlobService<AzureBlobStorageOptionsBase>>();
        var logger = new Mock<ILogger<AzureBlobStorageOptionsBase>>();
        const string container = "test-success";
        const string blobName = "test-blob-name";

        var blobObject = new AzureBlobStorageOptionsBase
        {
            ConnectionString = "my-conn-string"
        };

        blobObject.Containers.Add("success", "test-success");

        sut.Setup(x => x.Options).Returns(blobObject);
        var blobObjectSerialised = JsonSerializer.Serialize(blobObject);

        // Act
        await sut.Object.UploadBlobAsync(
            blobName,
            blobObjectSerialised,
            "success",
            logger.Object);

        // Assert
        sut.Verify(s => s.UploadAsync(container, blobName, blobObjectSerialised), Times.Once);
    }
}
