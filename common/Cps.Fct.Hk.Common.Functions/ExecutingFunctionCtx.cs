namespace Cps.Fct.Hk.Common.Functions;
/// <summary>
/// Wrapper object for providing Function contextual information.
/// </summary>
public class ExecutingFunctionCtx
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutingFunctionCtx"/> class.
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="ArgumentNullException">When context or request are null.</exception>
    public ExecutingFunctionCtx(FunctionContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// The executing function context.
    /// </summary>
    public FunctionContext Context { get; }
}
