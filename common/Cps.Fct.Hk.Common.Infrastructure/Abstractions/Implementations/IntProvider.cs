using Cps.Fct.Hk.Common.Contracts.Abstractions;

namespace Cps.Fct.Hk.Common.Infrastructure.Abstractions.Implementations;

/// <inheritdoc cref="IIntProvider"/>
public class IntProvider : IIntProvider
{
    /// <inheritdoc/>
    public int ParseOrThrow(string value)
    {
        var intParseSuccess = int.TryParse(value, out var number);
        if (!intParseSuccess)
        {
            throw new ArgumentException($"The integer value of {value} could not be parsed to an integer");
        }

        return number;
    }
}
