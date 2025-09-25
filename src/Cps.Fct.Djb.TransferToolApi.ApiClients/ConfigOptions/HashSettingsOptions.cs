// <copyright file="HashSettingsOptions.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.ConfigOptions;

/// <summary>
/// The options for the hash settings.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="HashSettingsOptions"/> class.
/// </remarks>
/// <param name="hashSecretKey">The key being used for hashing.</param>
/// <param name="caseIdHashPrefix">The prefix to use with hashed case id.</param>
/// <param name="documentIdHashPrefix">The prefix to use with hashed document id.</param>
/// <param name="materialIdHashPrefix">The prefix to use with hashed material id.</param>
public class HashSettingsOptions(
    string hashSecretKey,
    string caseIdHashPrefix,
    string documentIdHashPrefix,
    string materialIdHashPrefix)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HashSettingsOptions"/> class.
    /// </summary>
    public HashSettingsOptions()
        : this(
              hashSecretKey: string.Empty,
              caseIdHashPrefix: string.Empty,
              documentIdHashPrefix: string.Empty,
              materialIdHashPrefix: string.Empty)
    {
    }

    /// <summary>
    /// Gets the key being used for hashing.
    /// </summary>
    public string HashSecretKey { get; init; } = hashSecretKey;

    /// <summary>
    /// Gets the prefix to use with hashed case id.
    /// </summary>
    public string CaseIdHashPrefix { get; init; } = caseIdHashPrefix;

    /// <summary>
    /// Gets the prefix to use with hashed document id.
    /// </summary>
    public string DocumentIdHashPrefix { get; init; } = documentIdHashPrefix;

    /// <summary>
    /// Gets the prefix to use with hashed material id.
    /// </summary>
    public string MaterialIdHashPrefix { get; init; } = materialIdHashPrefix;
}
