using System.Collections.Immutable;
using Azure.Core.Serialization;
using Microsoft.Extensions.Options;

namespace Cps.Fct.Hk.Common.Functions.UnitTests;

public static class HttpRequestDataMock
{
    public static HttpRequestData Create(
        params FunctionParameter[] functionParameters)
    {
        var serviceProvider = SetupServiceProvider();

        var context = SetupFunctionContext(
            serviceProvider, functionParameters);

        var request = new Mock<HttpRequestData>(context);

        request.Setup(r => r.CreateResponse())
            .Returns(() =>
            {
                var response = new Mock<HttpResponseData>(context);
                response.SetupProperty(r => r.Headers, new HttpHeadersCollection())
                    .SetupProperty(r => r.StatusCode)
                    .SetupProperty(r => r.Body, new MemoryStream());

                return response.Object;
            });

        return request.Object;
    }

    private static FunctionContext SetupFunctionContext(
        IServiceProvider serviceProvider,
        IEnumerable<FunctionParameter> functionParameters)
    {
        var context = new Mock<FunctionContext>();
        context.Setup(c => c.InstanceServices).Returns(serviceProvider);

        var bindingContext = new Mock<BindingContext>();
        bindingContext.Setup(bc => bc.BindingData)
            .Returns(new Dictionary<string, object?>());
        context.Setup(c => c.BindingContext)
            .Returns(bindingContext.Object);

        var functionDefinition = new Mock<FunctionDefinition>();
        functionDefinition.Setup(fd => fd.Parameters)
            .Returns(functionParameters.ToImmutableArray());
        context.Setup(c => c.FunctionDefinition)
            .Returns(functionDefinition.Object);

        return context.Object;
    }

    private static IServiceProvider SetupServiceProvider()
    {
        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(sp => sp.GetService(
                typeof(IOptions<WorkerOptions>)))
            .Returns(new OptionsWrapper<WorkerOptions>(
                new WorkerOptions
                {
                    Serializer = new JsonObjectSerializer()
                }));

        return serviceProvider.Object;
    }
}
