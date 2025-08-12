using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace Cps.Fct.Hk.Common.Functions.Middleware;

/// <summary>
/// Middleware to globally handle uncaught exceptions during function execution.
/// </summary>
[ExcludeFromCodeCoverage]
public class GlobalExceptionHandlerMiddleware : IFunctionsWorkerMiddleware
{
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalExceptionHandlerMiddleware"/> class.
    /// </summary>
    /// <param name="logger"></param>
    public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        _logger.LogInformation("Starting Func {FunctionName}", context.FunctionDefinition.Name);
        try
        {
            await next(context).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(GlobalExceptionHandlerMiddleware));
            throw;
        }
        finally
        {
            _logger.LogInformation("Ending Func {FunctionName}", context.FunctionDefinition.Name);
        }
    }
}
