using Cps.Fct.Hk.Common.Contracts.Clients;

namespace Cps.Fct.Hk.Common.Contracts.UnitTests.Clients;

public class TestErrorResponse : IExternalErrorResponse
{
    public string ErrorDetails { get; set; } = string.Empty;

    public string GetError()
    {
        return ErrorDetails;
    }
}
