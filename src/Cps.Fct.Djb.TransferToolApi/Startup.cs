// <copyright file="Startup.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi;

using System.Diagnostics.CodeAnalysis;
using Cps.Fct.Djb.TransferToolApi.Services;
using Cps.Fct.Hk.Common.Contracts.Abstractions;
using Cps.Fct.Hk.Common.Functions;
using Cps.Fct.Hk.Common.Infrastructure.Abstractions.Implementations;
using Cps.Fct.Hk.Common.Infrastructure.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Startup class configures services and the app's request pipeline.
/// </summary>
[ExcludeFromCodeCoverage]
public class Startup : CommonStartup
{
    /// <inheritdoc/>
    protected override void ConfigureServicesHook(
        IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IAuthorizationSettings, DisableAuthorizationSettings>();

        services
            .AddDjbTransferToolApi(configuration);
    }

    /// <inheritdoc/>
    protected override void ConfigureRepositoriesHook(
        IServiceCollection services, IConfiguration configuration)
    {
        base.ConfigureRepositoriesHook(services, configuration);
    }

    /// <inheritdoc/>
    protected override void ConfigureClientsHook(
        IServiceCollection services, IConfiguration configuration)
    {
        base.ConfigureClientsHook(services, configuration);
    }

    /// <inheritdoc/>
    protected override void RegisterHealthChecks(
        IHealthChecksBuilder builder, IServiceCollection services)
    {
        builder
            .AddCheck<AppSettingHealthCheck>("DDEIClient:BaseAddress");

        RegisterTempHealthChecks(services);
    }

    private static void RegisterTempHealthChecks(
         IServiceCollection services)
    {
        _ = services;
    }
}
