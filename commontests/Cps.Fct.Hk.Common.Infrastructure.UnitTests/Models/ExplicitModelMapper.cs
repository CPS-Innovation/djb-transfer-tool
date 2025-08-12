namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Models;

internal sealed class ExplicitModelMapper : IModelMapper<MapperSource, MapperSourceDto>
{
    public MapperSourceDto Map(MapperSource source)
    {
        return new();
    }
}
