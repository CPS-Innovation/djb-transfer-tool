namespace Cps.Fct.Hk.Common.Functions.UnitTests.Helpers;
public class HttpFunctionTestContext
{
    public HttpFunctionTestContext()
    {
        Request = FunctionTestHelpers.CreateHttpRequestData();
        Response = FunctionTestHelpers.CreateHttpResponseData();
        Request.Setup(rd => rd.CreateResponse())
            .Returns(Response.Object)
            .Verifiable();
    }

    public Mock<HttpRequestData> Request { get; }

    public Mock<HttpResponseData> Response { get; }
}
