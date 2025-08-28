// <copyright file="CaseCenterOptionsPostConfigure.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.ConfigOptions;

using System.Collections.ObjectModel;

/// <summary>
/// Builds derived/validated state for <see cref="CaseCenterOptions"/> after binding.
/// </summary>
public static class CaseCenterOptionsPostConfigure
{
    /// <summary>
    /// Builds a lookup dictionary of court codes to template IDs from the provided area case template groups.
    /// </summary>
    /// <param name="options">CaseCenterOptions.</param>
    /// <exception cref="ArgumentNullException">Thrown if no options are supplied.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the template ids are not valid format.</exception>
    public static void BuildLookup(CaseCenterOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var group in options.AreaCaseTemplateGroups ?? Enumerable.Empty<AreaCaseTemplateGroup>())
        {
            if (string.IsNullOrWhiteSpace(group.TemplateId))
            {
                throw new InvalidOperationException("A CaseCenterOptions.AreaCaseTemplateGroups item has an empty TemplateId.");
            }

            if (group.CourtCodes is null || group.CourtCodes.Count == 0)
            {
                throw new InvalidOperationException($"Template '{group.TemplateId}' has no CourtCodes configured.");
            }

            foreach (var raw in group.CourtCodes)
            {
                var code = raw?.Trim();
                if (string.IsNullOrWhiteSpace(code))
                {
                    throw new InvalidOperationException($"Template '{group.TemplateId}' contains an empty CourtCode.");
                }

                if (map.TryGetValue(code, out var existing) &&
                    !string.Equals(existing, group.TemplateId, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException(
                        $"Court code '{code}' is mapped to multiple template IDs: '{existing}' and '{group.TemplateId}'.");
                }

                map[code] = group.TemplateId;
            }
        }

        options.AreaCaseTemplateIds = new ReadOnlyDictionary<string, string>(map);
    }
}
