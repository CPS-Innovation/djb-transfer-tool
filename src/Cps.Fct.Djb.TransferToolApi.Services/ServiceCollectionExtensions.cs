// <copyright file="ServiceCollectionExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Services;

using Cps.Fct.Djb.TransferToolApi.ApiClients.Clients;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Clients.Interfaces;
using Cps.Fct.Djb.TransferToolApi.ApiClients.ConfigOptions;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Constants;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Factories;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Factories.Interfaces;
using Cps.Fct.Djb.TransferToolApi.Services.Contracts;
using Cps.Fct.Djb.TransferToolApi.Services.Interfaces;
using Cps.Fct.Hk.Common.DDEI.Provider;
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

        services.AddScoped<ICookieService, CookieService>();
        services.AddScoped<IInitService, InitService>();
        services.AddHttpClient(); // Required for factory
        services.AddSingleton<IMdsApiClientFactory, MdsApiClientFactory>();

        // case center api
        // services.Configure<CaseCenterOptions>(configuration.GetSection(CaseCenterConfigConstants.CaseCenterSettingsConfigurationName));
        services.Configure<ClientEndpointOptions>(CaseCenterConfigConstants.CaseCenterApiClientConfigurationName, configuration.GetSection(CaseCenterConfigConstants.CaseCenterApiClientConfigurationName));

        services.AddScoped<ICreateCaseCenterCaseService, CreateCaseCenterCaseService>();
        services.AddSingleton<ICaseCenterApiClientFactory, CaseCenterApiClientFactory>();

        return services;
    }
}
