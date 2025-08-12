using Cps.Fct.Hk.Common.Contracts.Abstractions;

namespace Cps.Fct.Hk.Common.Infrastructure.Abstractions.Implementations;

/// <inheritdoc cref="IEnumProvider"/>
public class EnumProvider : IEnumProvider
{
    /// <inheritdoc/>
    public T ParseOrThrow<T>(string value)
        where T : struct, Enum
    {
        var enumParseSuccess = Enum.TryParse<T>(value, true, out var @enum);
        if (!enumParseSuccess)
        {
            throw new ArgumentException($"The enum value of {value} could not be parsed to an enum");
        }

        return @enum;
    }
}
