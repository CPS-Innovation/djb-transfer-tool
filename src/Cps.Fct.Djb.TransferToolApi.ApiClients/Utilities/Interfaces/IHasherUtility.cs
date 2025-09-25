// <copyright file="IHasher.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.Utilities.Interfaces;

/// <summary>
/// Contract for IHasher.
/// </summary>
public interface IHasherUtility
{
    /// <summary>
    /// Hash and prefix the CMS case ID.
    /// </summary>
    /// <param name="caseId">The case id to hash and prefix.</param>
    /// <returns>The hashed and prefixed case id.</returns>
    string HashAndPrefixCmsCaseId(int caseId);
}
