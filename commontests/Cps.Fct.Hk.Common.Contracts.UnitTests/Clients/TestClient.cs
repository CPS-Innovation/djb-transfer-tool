using Cps.Fct.Hk.Common.Contracts.Clients;

namespace Cps.Fct.Hk.Common.Contracts.UnitTests.Clients;

public class TestClient : ExternalJsonClient<TestErrorResponse>
{
    public TestClient(
        HttpClient httpClient,
        ILogger<ExternalJsonClient<TestErrorResponse>> logger)
        : base(httpClient, logger)
    {
    }

    protected override string ApiName => "Tests";

    protected override string RouteForLogging(string? route)
    {
        return base.RouteForLogging(route) + "Suffix";
    }

    public Task<string?> TestGetAsync(string route)
    {
        return GetAsync<string>(route);
    }

    public Task<string?> TestPostAsync(string route, TestErrorResponse request)
    {
        return PostAsync<TestErrorResponse, string>(route, request);
    }

    public Task<string?> TestSendAsync(HttpRequestMessage message)
    {
        return SendAsync<string>(message);
    }

    public JsonSerializerOptions TestSerializerOptions => SerializerOptions;
}
