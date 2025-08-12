// <copyright file="ServiceCollectionExtensions.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

using Cps.Fct.Hk.Common.DDEI.Client;

namespace Cps.Fct.Hk.Common.DDEI.Provider;

using Cps.Fct.Hk.Common.DDEI.Client.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add DDEI provider DI.
    /// </summary>
    /// <param name="services">Pass ServiceCollectionProvider.</param>
    /// <returns>Return service collection.</returns>
    public static IServiceCollection AddDDEIProvider(this IServiceCollection services)
    {
        services.AddDdeiApiClient<ClientEndpointOptions>(ClientEndpointOptions.DefaultSectionName);
        services.AddScoped<IPcdRequestProvider, PcdRequestProvider>();
        services.AddScoped<ICmsModernProvider, CmsModernProvider>();

        return services;
    }
}
