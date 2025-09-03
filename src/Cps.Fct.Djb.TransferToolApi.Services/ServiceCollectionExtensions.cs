// <copyright file="ServiceCollectionExtensions.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Services;

using Cps.Fct.Djb.TransferToolApi.ApiClients.ConfigOptions;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Constants;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Factories;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Factories.Interfaces;
using Cps.Fct.Djb.TransferToolApi.Services.Implementation;
using Cps.Fct.Djb.TransferToolApi.Services.Implementation.Interfaces;
using Cps.Fct.Djb.TransferToolApi.Shared.Interfaces;
using Cps.Fct.Hk.Common.DDEI.Provider;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add DJB Transfer Tool Api DI.
    /// </summary>
    /// <param name="services">Pass ServiceCollectionProvider.</param>
    /// <param name="configuration">Pass IConfiguration.</param>
    /// <returns>Return service collection.</returns>
    public static IServiceCollection AddDjbTransferToolApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDDEIProvider();

        services.AddHttpClient(); // Required for factory
        services.AddSingleton<IMdsApiClientFactory, MdsApiClientFactory>();

        services.AddScoped<IValidationService, ValidationService>();
        services.AddScoped<IDebugService, DebugService>();

        var assembliesWithValidators = AppDomain.CurrentDomain.GetAssemblies()
        .Where(asm =>
        {
            // If *any* type in the assembly implements IValidatorDependencyScanner (and is neither abstract nor an interface),
            // then we include that assembly in the results.
            return asm.GetTypes().Any(t =>
                !t.IsAbstract &&
                !t.IsInterface &&
                typeof(IValidatorDependencyScanner).IsAssignableFrom(t));
        })
        .ToArray();
        services.AddValidatorsFromAssemblies(assembliesWithValidators);

        var assembliesWithAutoMapperProfiles = AppDomain.CurrentDomain.GetAssemblies()
        .Where(asm =>
        {
            // If *any* type in the assembly implements IAutoMapperDependencyScanner (and is neither abstract nor an interface),
            // then we include that assembly in the results.
            return asm.GetTypes().Any(t =>
                !t.IsAbstract &&
                !t.IsInterface &&
                typeof(IAutoMapperDependencyScanner).IsAssignableFrom(t));
        })
        .ToArray();
        services.AddAutoMapper(assembliesWithAutoMapperProfiles);

        // case center api
        services.Configure<CaseCenterOptions>(configuration.GetSection(CaseCenterConfigConstants.CaseCenterConfigurationName));
        services.PostConfigure<CaseCenterOptions>(CaseCenterOptionsPostConfigure.BuildLookup);
        services.Configure<ClientEndpointOptions>(CaseCenterConfigConstants.CaseCenterApiClientConfigurationName, configuration.GetSection(CaseCenterConfigConstants.CaseCenterApiClientConfigurationName));

        services.AddScoped<ICreateCaseCenterCaseService, CreateCaseCenterCaseService>();
        services.AddSingleton<ICaseCenterApiClientFactory, CaseCenterApiClientFactory>();

        return services;
    }
}
