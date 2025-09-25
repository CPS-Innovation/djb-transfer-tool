// <copyright file="CmsAreaMapping.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.ConfigOptions;

/// <summary>
/// Represents the mapping of CMS areas to case templates and department ids.
/// </summary>
public class CmsAreaMapping
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CmsAreaMapping"/> class.
    /// </summary>
    public CmsAreaMapping()
    {
        this.TemplateId = string.Empty;
        this.DepartmentId = string.Empty;
    }

    /// <summary>Gets the template ID for the linked area.</summary>
    public string TemplateId { get; init; }

    /// <summary>Gets the department ID for the linked area.</summary>
    public string DepartmentId { get; init; }
}
