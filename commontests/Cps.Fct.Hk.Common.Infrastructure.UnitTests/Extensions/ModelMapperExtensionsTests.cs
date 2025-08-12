namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Extensions;

public class ModelMapperExtensionsTests
{
    private readonly Mock<IModelMapper<SourceDto, DestinationDto>> _mapper = new();

    [Fact]
    public void Map_Single_MapsAndPerformsPostMapAction()
    {
        // Arrange
        var source1 = Mock.Of<SourceDto>();
        var dest1 = Mock.Of<DestinationDto>();

        _mapper.Setup(x => x.Map(source1)).Returns(dest1).Verifiable();

        // Act
        var result = _mapper.Object.Map(
            source1,
            d => d.PostMapProp = "1234");

        // Assert
        result.PostMapProp.ShouldBe("1234");
        _mapper.Verify();
    }

    [Fact]
    public void MapArray_WithMultiple_MapsAllAndPerformsPostMapAction()
    {
        // Arrange
        var source1 = Mock.Of<SourceDto>();
        var source2 = Mock.Of<SourceDto>();

        var dest1 = Mock.Of<DestinationDto>();
        var dest2 = Mock.Of<DestinationDto>();

        _mapper.Setup(x => x.Map(source1)).Returns(dest1).Verifiable();
        _mapper.Setup(x => x.Map(source2)).Returns(dest2).Verifiable();

        // Act
        var result = _mapper.Object.MapArray(
            new[] { source1, source2 },
            d => d.PostMapProp = "1234");

        // Assert
        result.All(x => x.PostMapProp == "1234").ShouldBeTrue();
        result.Length.ShouldBe(2);
        result.ShouldContain(dest1);
        result.ShouldContain(dest2);
        _mapper.Verify();
    }

    public class SourceDto
    {
        public string? PostMapProp { get; set; }
    }

    public class DestinationDto
    {
        public string? PostMapProp { get; set; }
    }
}
