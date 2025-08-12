// <copyright file="ServiceCollectionExtensions.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.DDEI.Client;
using Cps.Fct.Hk.Common.DDEI.Client.Configuration;

using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds options to be used with the <c>IOptions</c> pattern.
    /// See https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-7.0.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options entity.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="sectionName">The configuration section name.</param>
    /// <returns>Fluently returns the service collection.</returns>
    public static IServiceCollection AddDdeiApiClient<TOptions>(this IServiceCollection services, string sectionName)
    where TOptions : class
    {
        services.AddSingleton<IDdeiApiClient, DdeiApiClient>();

        services.AddHttpClient(
                   name: ClientEndpointOptions.DefaultSectionName,
                   configureClient: (services, options) =>
                   {
                       ClientEndpointOptions endpointOptions = services.GetRequiredService<IOptions<ClientEndpointOptions>>().Value;
                       options.BaseAddress = endpointOptions.BaseAddress;
                       options.Timeout = endpointOptions.Timeout;
                       options.DefaultRequestHeaders
                           .Accept
                           .Add(new MediaTypeWithQualityHeaderValue("application/json", 1.0));
                   });

        services
            .AddOptions<TOptions>()
            .Configure<IConfiguration>((options, config) =>
            {
                config
                    .GetSection(sectionName)
                    .Bind(options);
            })
            .ValidateDataAnnotations();

        return services;
    }
}
