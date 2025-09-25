// <copyright file="CmsAreaToCaseCenterDataMappingViaOptionsResolver.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.Resolvers;

using Cps.Fct.Djb.TransferToolApi.ApiClients.ConfigOptions;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Resolvers.Interfaces;
using Microsoft.Extensions.Options;

public class CmsAreaToCaseCenterDataMappingViaOptionsResolver : ICmsAreaToCaseCenterDataMappingResolver
{
    private readonly CmsAreaToCaseCenterDataMappingsOptions _options;

    public CmsAreaToCaseCenterDataMappingViaOptionsResolver(
        IOptionsMonitor<CmsAreaToCaseCenterDataMappingsOptions> options)
    {
        _options = options.CurrentValue
            ?? throw new InvalidOperationException("Missing Cms Area To Case Center Data Mappings options.");
    }

    public CmsAreaMapping? Resolve(string areaName)
    {
        if (string.IsNullOrWhiteSpace(areaName))
        {
            return null;
        }

        return _options.TryGetValue(areaName, out var mapping)
            ? mapping
            : null;
    }
}
