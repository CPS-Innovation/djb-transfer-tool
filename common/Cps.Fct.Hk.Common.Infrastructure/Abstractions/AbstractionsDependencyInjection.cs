using Cps.Fct.Hk.Common.Contracts.Abstractions;
using Cps.Fct.Hk.Common.Infrastructure.Abstractions.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Cps.Fct.Hk.Common.Infrastructure.Abstractions;

/// <summary>
/// Provides extension method(s) for adding abstractions to the services container.
/// </summary>
public static class AbstractionsDependencyInjection
{
    /// <summary>
    /// Adds common abstractions to the services container.
    /// </summary>
    /// <param name="services"></param>
    /// <returns><see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddAbstractionsDefaultImplementations(this IServiceCollection services)
    {

        services.AddSingleton<IAssemblyProvider, AssemblyProvider>();
        services.AddSingleton<IAssemblyInfoProvider, AssemblyInfoProvider>();
        services.AddSingleton<IEnvironmentValuesProvider, EnvironmentValuesProvider>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IGuidProvider, GuidProvider>();
        services.AddSingleton<IAuthorizationSettings, AuthorizationSettings>();
        services.AddSingleton<IEnumProvider, EnumProvider>();
        services.AddSingleton<IIntProvider, IntProvider>();

        return services;
    }
}
