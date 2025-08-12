namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Abstractions;
using Cps.Fct.Hk.Common.Infrastructure.Abstractions.Implementations;
using Cps.Fct.Hk.Common.Infrastructure.UnitTests.Models;

public class MapsterModelMapperTests
{
    private readonly IModelMapper<DummyModel, DummyModelDto> _mapper;

    public MapsterModelMapperTests()
    {
        _mapper = new MapsterModelMapper<DummyModel, DummyModelDto>();
    }

    [Fact]
    public void Map_NewObject_Succeeds()
    {
        // Arrange
        var model = new DummyModel { Age = 10, Id = "abc", Name = "Test" };

        // Act
        var result = _mapper.Map(model);

        // Assert
        result.Age.ShouldBe(10);
        result.Id.ShouldBe("abc");
        result.Name.ShouldBe("Test");
    }
}
