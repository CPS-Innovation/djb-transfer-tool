// <copyright file="ObjectExtentions.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Shared.Extensions;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

/// <summary>
/// Provides extension methods for JSON serialization.
/// </summary>
public static class ObjectExtentions
{
    /// <summary>
    /// Converts an object to a JSON string suitable for use as a payload in HTTP requests.
    /// </summary>
    /// <param name="obj">object.</param>
    /// <returns>Serialized object.</returns>
    public static string ToJsonPayload(this object obj)
    {
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
        };

        return JsonConvert.SerializeObject(obj, Formatting.None, settings);
    }
}

