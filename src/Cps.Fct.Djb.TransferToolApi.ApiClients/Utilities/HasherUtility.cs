// <copyright file="IHasher.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.Utilities;

using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Cps.Fct.Djb.TransferToolApi.ApiClients.ConfigOptions;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Utilities.Interfaces;
using Microsoft;
using Microsoft.Extensions.Options;

public class HasherUtility : IHasherUtility
{
    private readonly HashSettingsOptions hashSettingsOptions;

    public HasherUtility(IOptionsMonitor<HashSettingsOptions> hashSettingsOptions)
    {
        this.hashSettingsOptions = hashSettingsOptions.CurrentValue
            ?? throw new InvalidOperationException("Missing Hash Settings options.");
    }

    /// <summary>
    /// Hash and prefix the CMS case ID.
    /// </summary>
    /// <param name="caseId">The case id to hash and prefix.</param>
    /// <returns>The hashed and prefixed case id.</returns>
    public string HashAndPrefixCmsCaseId(int caseId)
    {
        var hashSecretKey = this.hashSettingsOptions?.HashSecretKey ?? string.Empty;
        var casePrefix = this.hashSettingsOptions?.CaseIdHashPrefix ?? string.Empty;

        Requires.NotNullOrEmpty(hashSecretKey);
        Requires.NotNullOrEmpty(casePrefix);
        Requires.NotDefault<int>(caseId);

        byte[] secretKeyBytes = Encoding.ASCII.GetBytes(hashSecretKey);
        using HMACSHA256 hmac = new(secretKeyBytes);

        byte[] idToHashAsBytes = Encoding.ASCII.GetBytes(caseId.ToString(CultureInfo.InvariantCulture));
        byte[] hash = hmac.ComputeHash(idToHashAsBytes);

        return casePrefix + Convert.ToHexString(hash);
    }
}
