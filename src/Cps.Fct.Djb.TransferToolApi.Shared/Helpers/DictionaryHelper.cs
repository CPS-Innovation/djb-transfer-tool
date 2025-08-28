// <copyright file="DictionaryHelper.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Shared.Helpers;

/// <summary>
/// A helper for dictionary types.
/// </summary>
public static class DictionaryHelper
{
    /// <summary>
    /// Ensures that the specified dictionary is not <c>null</c> and contains at least one element.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dict">
    /// The <see cref="IReadOnlyDictionary{TKey, TValue}"/> to validate. This parameter may be
    /// <c>null</c>, in which case an <see cref="ArgumentException"/> will be thrown.
    /// </param>
    /// <param name="paramName">
    /// The name of the parameter being validated. This is used in any thrown exceptions to
    /// identify the source of the invalid argument.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="dict"/> is <c>null</c> or contains no elements.
    /// </exception>
    public static void NotNullOrEmpty<TKey, TValue>(
        IReadOnlyDictionary<TKey, TValue>? dict,
        string? paramName = null)
    {
        if (dict is null || dict.Count == 0)
        {
            throw new ArgumentException("Dictionary cannot be null or empty.", paramName);
        }
    }
}
