using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace Cps.Fct.Hk.Common.Functions.Middleware;

/// <summary>
/// Middleware to measure and log function execution time.
/// </summary>
[ExcludeFromCodeCoverage]
public class ExecutionTimeMiddleware : IFunctionsWorkerMiddleware
{
    private readonly ILogger<ExecutionTimeMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutionTimeMiddleware"/> class.
    /// </summary>
    /// <param name="logger"></param>
    public ExecutionTimeMiddleware(ILogger<ExecutionTimeMiddleware> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var watch = Stopwatch.StartNew();
        try
        {
            await next(context).ConfigureAwait(false);
        }
        finally
        {
            watch.Stop();
            _logger.LogTrace($"{context.FunctionDefinition.Name} took {watch.ElapsedMilliseconds}ms to execute.");
        }
    }
}
