// <copyright file="RegexExpressions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Services.Constants;

/// <summary>
/// Class to store all regex expressions.
/// </summary>
public static class RegexExpressions
{
    /// <summary>
    /// Regex for allowing certain special characters in material name.
    /// </summary>
    public const string RenameMaterialSubjectRegex = @"^[a-zA-Z0-9,\-\)\(_\/:=\'.""\~\s]+$";
}
