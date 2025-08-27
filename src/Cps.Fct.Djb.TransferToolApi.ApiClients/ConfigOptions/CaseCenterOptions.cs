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
/// <param name="juryBundleName">The name of the jury bundle.</param>
/// <param name="indictmentSectionName">The name of the indictment section.</param>
/// <param name="exhibitsSectionName">The name of the exhibits section.</param>
/// <param name="templateIds">A dictionary of the template ids by their area.</param>

public class CaseCenterOptions(
    string organisationId,
    string organisationType,
    string juryBundleName,
    string indictmentSectionName,
    string exhibitsSectionName,
    IReadOnlyDictionary<string, string> templateIds)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CaseCenterOptions"/> class.
    /// </summary>
    public CaseCenterOptions()
        : this(
              organisationId: string.Empty,
              organisationType: string.Empty,
              juryBundleName: string.Empty,
              indictmentSectionName: string.Empty,
              exhibitsSectionName: string.Empty,
              templateIds: new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase))
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
    /// Gets the juryBundleName.
    /// </summary>
    public string JuryBundleName { get; init; } = juryBundleName;

    /// <summary>
    /// Gets the indictmentSectionName.
    /// </summary>
    public string IndictmentSectionName { get; init; } = indictmentSectionName;

    /// <summary>
    /// Gets the exhibitsSectionName.
    /// </summary>
    public string ExhibitsSectionName { get; init; } = exhibitsSectionName;

    /// <summary>
    /// Gets a list of the template ids by their area.
    /// </summary>
    public IReadOnlyDictionary<string, string> TemplateIds { get; init; } = templateIds;
}
