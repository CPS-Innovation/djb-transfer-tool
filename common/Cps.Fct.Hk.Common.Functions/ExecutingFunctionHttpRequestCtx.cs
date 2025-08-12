namespace Cps.Fct.Hk.Common.Functions;
/// <summary>
/// Wrapper object for providing Function request contextual information.
/// </summary>
public class ExecutingFunctionHttpRequestCtx : ExecutingFunctionCtx
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutingFunctionHttpRequestCtx"/> class.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="request"></param>
    /// <exception cref="ArgumentNullException">When context or request are null.</exception>
    public ExecutingFunctionHttpRequestCtx(FunctionContext context, HttpRequestData request)
        : base(context)
    {
        Request = request ?? throw new ArgumentNullException(nameof(request));
    }

    /// <summary>
    /// The incoming HttpRequest data.
    /// </summary>
    public HttpRequestData Request { get; }
}
