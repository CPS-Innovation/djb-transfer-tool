// <copyright file="CmsAreaToCaseCenterDataMappingsOptions.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.ConfigOptions;

/// <summary>
/// Represents the mapping of CMS areas to case center data.
/// </summary>
public class CmsAreaToCaseCenterDataMappingsOptions : Dictionary<string, CmsAreaMapping>
{
    public CmsAreaToCaseCenterDataMappingsOptions()
    : base(StringComparer.OrdinalIgnoreCase) { }
}
