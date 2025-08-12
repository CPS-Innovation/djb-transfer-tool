using System.Net;
using Cps.Fct.Hk.Common.Contracts.Abstractions;
using Cps.Fct.Hk.Common.Functions.Abstractions;
using Cps.Fct.Hk.Common.Functions.Swagger.Attributes;

namespace Cps.Fct.Hk.Common.Functions.Functions;

/// <summary>
/// Function that will provide diagnostic information.
/// </summary>
public class DiagnosticsFunction
{
    private readonly IAssemblyInfoProvider _assemblyInfo;
    private readonly IHttpDataSerializer _httpDataSerializer;

    /// <summary>
    /// Initializes a new instance of the <see cref="DiagnosticsFunction"/> class.
    /// </summary>
    /// <param name="assemblyInfo"></param>
    /// <param name="httpDataSerializer"></param>
    public DiagnosticsFunction(
        IAssemblyInfoProvider assemblyInfo,
        IHttpDataSerializer httpDataSerializer)
    {
        _assemblyInfo = assemblyInfo;
        _httpDataSerializer = httpDataSerializer;
    }

    /// <summary>
    /// Returns the Function app assembly version.
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [Function("Version")]
    [SwaggerOp("Gets the version information of the current function", "Diagnostics")]
#pragma warning disable S1135 // Track uses of "TODO" tags
    [OkJsonResult(typeof(string))] // TODO: Need new nuget version to fix: An item with the same key has already been added
    public async Task<HttpResponseData> VersionAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/diagnostics/version")] HttpRequestData req)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        await _httpDataSerializer.WriteResponseBodyAsync(response, _assemblyInfo.VersionInfo).ConfigureAwait(false);

        return response;
    }
}
