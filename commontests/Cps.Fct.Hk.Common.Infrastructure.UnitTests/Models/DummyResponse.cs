
namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Models;

public sealed class DummyResponse : IResult
{
    public HttpStatusCode Status { get; set; }

    public object? CustomState { get; set; }
}
