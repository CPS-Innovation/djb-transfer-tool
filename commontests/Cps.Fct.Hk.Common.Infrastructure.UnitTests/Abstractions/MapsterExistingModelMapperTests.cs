namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Abstractions;
using Cps.Fct.Hk.Common.Infrastructure.Abstractions.Implementations;
using Cps.Fct.Hk.Common.Infrastructure.UnitTests.Models;

public class MapsterExistingModelMapperTests
{
    private readonly IExistingModelMapper<DummyModel, DummyModelDto> _mapper;

    public MapsterExistingModelMapperTests()
    {
        _mapper = new MapsterExistingModelMapper<DummyModel, DummyModelDto>();
    }

    [Fact]
    public void Map_ToExistingObject_Succeeds()
    {
        // Arrange
        var model = new DummyModel { Age = 10, Id = "abc" };

        var destination = new DummyModelDto { Id = "def", Name = "Test" };

        // Act
        var result = _mapper.Map(model, destination);

        // Assert
        result.Age.ShouldBe(10);
        result.Id.ShouldBe("abc");
        result.Name.ShouldBe(string.Empty);
    }
}
