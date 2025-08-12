using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Cps.Fct.Hk.Common.Contracts.Configuration;
using Cps.Fct.Hk.Common.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceCollectionExtensions = Cps.Fct.Hk.Common.Infrastructure.Extensions.ServiceCollectionExtensions;

namespace Cps.Fct.Hk.Common.Infrastructure.Abstractions;

/// <summary>
/// Provides extension methods for injecting IOptions configurations.
/// </summary>
[ExcludeFromCodeCoverage]
public static class OptionsDependencyInjection
{
    /// <summary>
    /// Register Mappers Dependencies.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="cpsAssemblies"></param>
    /// <returns></returns>
    public static IServiceCollection AddApplicationOptions(
        this IServiceCollection services,
        IConfiguration configuration,
        IEnumerable<Assembly> cpsAssemblies)
    {
        var optionsTypes = new[] { typeof(IConfigurationOptions) };

        var regOptionsMethod = typeof(ServiceCollectionExtensions).GetMethod(
            nameof(ServiceCollectionExtensions.AddConfigurationOptions),
            BindingFlags.Static | BindingFlags.Public)
            ?? throw new InvalidOperationException("Could not find method to configure options: " +
                $"${nameof(ServiceCollectionExtensions.AddConfigurationOptions)}");

        foreach (var assembly in cpsAssemblies)
        {
            foreach (var item in assembly.GetTypes())
            {
                var isConcrete = !item.IsAbstract && !item.IsInterface && !item.IsGenericType;
                if (isConcrete)
                {
                    foreach (var optionsType in optionsTypes.Where(opType => IsItemOptionsType(item, opType)))
                    {
                        regOptionsMethod
                            .MakeGenericMethod(item)
                            .Invoke(null, new object[] { services, configuration });
                    }
                }
            }
        }

        static bool IsItemOptionsType(Type item, Type optionsType)
        {
            return item.GetInterfaces().Any(i => i == optionsType);
        }

        return services;
    }
}
