namespace Cps.Fct.Hk.Common.Contracts.Models;

using System.Text.Json.Serialization;

/// <summary>
/// ProblemDetails.
/// </summary>
public class ProblemDetails
{
    /// <summary>
    /// type.
    /// </summary>
    public string? Type { get; set; } = "about:blank"; // Default type

    /// <summary>
    /// Title.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Status.
    /// </summary>
    public int? Status { get; set; }

    /// <summary>
    /// Detail.
    /// </summary>
    public string? Detail { get; set; }

    /// <summary>
    /// Instance.
    /// </summary>
    public string? Instance { get; set; }

    /// <summary>
    /// Extensions.
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, object> Extensions { get; set; } = new Dictionary<string, object>();
}
