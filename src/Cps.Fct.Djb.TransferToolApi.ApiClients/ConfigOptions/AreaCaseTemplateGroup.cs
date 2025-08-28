// <copyright file="AreaCaseTemplateGroup.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.ConfigOptions;

/// <summary>
/// Represents a group of court codes that resolve to the same Case Center template ID.
/// </summary>
public class AreaCaseTemplateGroup
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AreaCaseTemplateGroup"/> class.
    /// </summary>
    public AreaCaseTemplateGroup()
    {
        this.TemplateId = string.Empty;
        this.CourtCodes = new List<string>();
    }

    /// <summary>Gets the template ID applied to all <see cref="CourtCodes"/>.</summary>
    public string TemplateId { get; init; }

    /// <summary>Gets the court codes that should resolve to <see cref="TemplateId"/>.</summary>
    public List<string> CourtCodes { get; init; }
}
