namespace Cps.Fct.Hk.Common.Functions;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Cps.Fct.Hk.Common.Contracts.Abstractions;
using Cps.Fct.Hk.Common.Contracts.Extensions;
using Cps.Fct.Hk.Common.Infrastructure.Abstractions;
using Cps.Fct.Hk.Common.Infrastructure.Configuration;
using Cps.Fct.Hk.Common.Infrastructure.Extensions;
using FunctionHealthCheck;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

/// <summary>Initialisation logic which is common to all Function solutions.</summary>
[ExcludeFromCodeCoverage]
public abstract class CommonStartup
{
    /// <summary>Configuration field.</summary>
    private IConfiguration? _configuration;

    /// <summary>Configuration property.</summary>
    /// <returns></returns>
    protected IConfiguration Configuration
    {
        get
        {
            if (_configuration == null)
            {
                throw new InvalidOperationException("The configuration field is null");
            }

            return _configuration;
        }
    }

    /// <summary>
    /// Application generic configuration values.
    /// </summary>
    protected IOptions<AppOptions>? AppOptions { get; private set; }

    /// <summary>
    /// This method gets called by the runtime.
    /// Adds common services to the container.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        _configuration = configuration;

        RegisterAbstractions(services);

        // Depends on AddAbstractionsDefaultImplementations
        var entryAssembly = GetEntryAssembly(services, configuration);
        var cpsAssemblies = entryAssembly.GetCPSAssemblies();

        services
            .AddApplicationInsightsTelemetryWorkerService()
            .AddOptions()
            .AddConfigurationOptions<AppOptions>(configuration)
            .AddConfigurationOptions<LoggingOptions>(configuration);

        var sp = services.BuildServiceProvider();
        AppOptions = sp.GetRequiredService<IOptions<AppOptions>>();

        services
            .AddApplicationMappers(cpsAssemblies);

        ConfigureRepositoriesHook(services, configuration);
        ConfigureClientsHook(services, configuration);
        ConfigureServicesHook(services, configuration);

        var healthChecksBuilder = services.AddFunctionHealthChecks();
        RegisterHealthChecks(healthChecksBuilder, services);
    }

    /// <summary>
    /// Register all abstractions.
    /// </summary>
    /// <param name="services"></param>
    protected virtual void RegisterAbstractions(IServiceCollection services)
    {
        services.AddAbstractionsDefaultImplementations();
    }

    /// <summary>
    /// Hook that executes at the end of ConfigureServices <see cref="ConfigureServices"/>.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    protected abstract void ConfigureServicesHook(
        IServiceCollection services, IConfiguration configuration);

    /// <summary>
    /// Book that registers application health checks.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="services"></param>
    protected abstract void RegisterHealthChecks(
        IHealthChecksBuilder builder, IServiceCollection services);

    /// <summary>
    /// Hook that executes at the end of ConfigureServices <see cref="ConfigureServices"/>
    /// to register clients and easily replace them in integration tests.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    protected virtual void ConfigureClientsHook(
        IServiceCollection services, IConfiguration configuration)
    {

    }

    /// <summary>
    /// Hook that executes at the end of ConfigureServices <see cref="ConfigureServices"/>
    /// to register repositories and easily replace them in integration tests.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    protected virtual void ConfigureRepositoriesHook(
        IServiceCollection services, IConfiguration configuration)
    {
    }

    private static Assembly GetEntryAssembly(
        IServiceCollection services, IConfiguration configuration)
    {
        Assembly entryAssembly;

        var environment = configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT");
        if (string.Equals(environment, "CI", StringComparison.CurrentCultureIgnoreCase))
        {
            var assemblyName = configuration.GetValue<string>(
                nameof(Infrastructure.Configuration.AppOptions.AppName))
                ?? Assembly.GetExecutingAssembly().GetName().Name;
            entryAssembly = Assembly.Load(assemblyName!);
        }
        else
        {
            var sp = services.BuildServiceProvider();
            var assemblyProvider = sp.GetService<IAssemblyProvider>()
                ?? throw new InvalidOperationException(
                    $"Could not resolve {nameof(IAssemblyProvider)}! Did you forget to register it?");

            entryAssembly = assemblyProvider.GetEntryAssembly();
        }

        // Make it available for anything that cannot get it injected
        StaticAssemblyProvider.GetEntryAssemblyFunc = () => entryAssembly;

        return entryAssembly;
    }
}
