// <copyright file="ServiceCollectionExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Services;

using Cps.Fct.Hk.Common.DDEI.Provider;
using Cps.Fct.Djb.TransferToolApi.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Factories.Interfaces;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Factories;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add DJB Transfer Tool Api DI.
    /// </summary>
    /// <param name="services">Pass ServiceCollectionProvider.</param>
    /// <returns>Return service collection.</returns>
    public static IServiceCollection AddDjbTransferToolApi(this IServiceCollection services)
    {
        services.AddDDEIProvider();

        services.AddScoped<ICookieService, CookieService>();
        services.AddScoped<IInitService, InitService>();
        services.AddHttpClient(); // Required for factory
        services.AddSingleton<IMdsApiClientFactory, MdsApiClientFactory>();
        return services;
    }
}
