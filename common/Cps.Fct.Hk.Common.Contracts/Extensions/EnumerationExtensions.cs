using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Cps.Fct.Hk.Common.Contracts.Extensions;

/// <summary>
/// Extensions for <see cref="Enum"/>.
/// </summary>
public static class EnumerationExtensions
{
    /// <summary>
    /// Gets the display name of the Enum based on its Display attribute.
    /// </summary>
    /// <param name="value">The Enum to get the display name of.</param>
    /// <returns>
    /// The display name of the Enum based on its Display attribute
    /// or if that does not exist, the string representation of the Enum.
    /// </returns>
    public static string GetDisplayName(this Enum value)
    {
        var displayName = value.GetType()
            .GetMember(value.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()?
            .Name;

        if (string.IsNullOrEmpty(displayName))
        {
            displayName = value.ToString();
        }

        return displayName;
    }
}
