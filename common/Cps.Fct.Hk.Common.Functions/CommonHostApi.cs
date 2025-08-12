using System.Diagnostics.CodeAnalysis;
using Cps.Fct.Hk.Common.Functions.Middleware;
using Cps.Fct.Hk.Common.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Cps.Fct.Hk.Common.Functions;

/// <summary>
/// Provides common API for building, configuring and hosting an application.
/// </summary>
[ExcludeFromCodeCoverage]
public static class CommonHostApi
{
    /// <summary>
    /// Runs an application by instantiating a HostBuilder with common functionality and executing optional pre and post host creation hooks.
    /// </summary>
    /// <typeparam name="TStartupType">Startup file type (Commonly the Startup.cs file).</typeparam>
    /// <param name="args">Startup arguments.</param>
    /// <param name="preCreateHostBuilderHook">Any actions to execute before the HostBuilder is created.</param>
    /// <param name="postCreateHostBuilderHook">Any actions to execute after the HostBuilder is created, before it is built and ran.</param>
    public static void Run<TStartupType>(
        string[] args,
        Action? preCreateHostBuilderHook = null,
        Func<IHostBuilder, IHostBuilder>? postCreateHostBuilderHook = null)
        where TStartupType : CommonStartup
    {
        preCreateHostBuilderHook?.Invoke();

        var hostBuilder = CreateHostBuilder<TStartupType>(args);

        if (postCreateHostBuilderHook != null)
        {
            hostBuilder = postCreateHostBuilderHook(hostBuilder);
        }

        var host = hostBuilder.Build();
        host.Run();
    }

    /// <summary>
    /// CreateHostBuilder.
    /// </summary>
    /// <typeparam name="TStartupType"></typeparam>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <exception cref="EntryPointNotFoundException"></exception>
    private static IHostBuilder CreateHostBuilder<TStartupType>(string[] args)
        where TStartupType : CommonStartup
    {
        IConfiguration? config = null;

        var hostBuilder = Host
            .CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(builder => config = builder.LoadConfiguration("Development").Build())
            .ConfigureFunctionsWorkerDefaults(worker =>
            {
                worker.UseMiddleware<GlobalExceptionHandlerMiddleware>();
                worker.UseMiddleware<ExecutionTimeMiddleware>();
                worker.UseNewtonsoftJson();
            })
            .ConfigureLogging(logBuilder =>
            {
                // The ConfigureFunctionsWorkerDefaults method will add some framework loggers, but these
                // will interfere with the Serilog logger and create duplicates.
                logBuilder.ClearProviders();
            })
            .ConfigureServices((context, services) =>
            {
                var startup = Activator.CreateInstance<TStartupType>()
                    ?? throw new EntryPointNotFoundException($"Could not instantiate startup: {typeof(TStartupType).Name}");

                startup.ConfigureServices(services, context.Configuration);

            })
            .ConfigureOpenApi();

        return hostBuilder;
    }
}
