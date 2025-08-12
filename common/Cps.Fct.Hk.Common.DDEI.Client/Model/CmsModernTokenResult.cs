using System.Text.Json.Serialization;

namespace Cps.Fct.Hk.Common.DDEI.Client.Model;

/// <summary>
/// Represents the result containing the CMS modern token.
/// </summary>
public record CmsModernTokenResult(
    [property: JsonPropertyName("cmsModernToken")] string CmsModernToken)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CmsModernTokenResult"/> class.
    /// </summary>
    public CmsModernTokenResult()
        : this(string.Empty)
    {
    }
}
