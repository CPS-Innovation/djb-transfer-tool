// <copyright file="CaseCenterOptions.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.ConfigOptions;

/// <summary>
/// The options for a case center.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CaseCenterOptions"/> class.
/// </remarks>
/// <param name="organisationId">The organisation id.</param>
/// <param name="organisationType">The organisation type.</param>
/// <param name="masterBundleName">The name of the master bundle.</param>
/// <param name="indictmentSectionName">The name of the indictment section.</param>
/// <param name="exhibitsSectionName">The name of the exhibits section.</param>
/// <param name="areaCaseTemplateGroups">A grouping of 1 template to many courts.</param>
/// <param name="areaCaseTemplateIds">A dictionary of the case template ids by their area.</param>
public class CaseCenterOptions(
    string organisationId,
    string organisationType,
    string masterBundleName,
    string indictmentSectionName,
    string exhibitsSectionName,
    List<AreaCaseTemplateGroup> areaCaseTemplateGroups,
    Dictionary<string, string> areaCaseTemplateIds)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CaseCenterOptions"/> class.
    /// </summary>
    public CaseCenterOptions()
        : this(
              organisationId: string.Empty,
              organisationType: string.Empty,
              masterBundleName: string.Empty,
              indictmentSectionName: string.Empty,
              exhibitsSectionName: string.Empty,
              areaCaseTemplateGroups: new List<AreaCaseTemplateGroup>(),
              areaCaseTemplateIds: new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase))
    {
    }

    /// <summary>
    /// Gets the organisationId.
    /// </summary>
    public string OrganisationId { get; init; } = organisationId;

    /// <summary>
    /// Gets the organisationType.
    /// </summary>
    public string OrganisationType { get; init; } = organisationType;

    /// <summary>
    /// Gets the masterBundleName.
    /// </summary>
    public string MasterBundleName { get; init; } = masterBundleName;

    /// <summary>
    /// Gets the indictmentSectionName.
    /// </summary>
    public string IndictmentSectionName { get; init; } = indictmentSectionName;

    /// <summary>
    /// Gets the exhibitsSectionName.
    /// </summary>
    public string ExhibitsSectionName { get; init; } = exhibitsSectionName;

    /// <summary>
    /// Gets the Grouped config: many court codes mapping to a single template id.
    /// </summary>
    public List<AreaCaseTemplateGroup> AreaCaseTemplateGroups { get; init; } = areaCaseTemplateGroups;

    /// <summary>
    /// Gets the Flattened, case-insensitive lookup built at startup: courtCode → templateId.
    /// </summary>
    public IReadOnlyDictionary<string, string> AreaCaseTemplateIds { get; internal set; } = areaCaseTemplateIds;
}
