// <copyright file="ICmsAreaToCaseCenterDataMappingResolver.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.Resolvers.Interfaces;

using Cps.Fct.Djb.TransferToolApi.ApiClients.ConfigOptions;

/// <summary>
/// Contract for ICmsAreaToCaseCenterDataMappingResolver.
/// </summary>
public interface ICmsAreaToCaseCenterDataMappingResolver
{
    /// <summary>
    /// Resolve a CMS area name to a <see cref="CmsAreaMapping"/>.
    /// </summary>
    /// <param name="areaName">The name of the area.</param>
    /// <returns>Null or the area mapping.</returns>
    CmsAreaMapping? Resolve(string areaName);
}
