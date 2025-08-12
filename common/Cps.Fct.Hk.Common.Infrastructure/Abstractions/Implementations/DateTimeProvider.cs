using Cps.Fct.Hk.Common.Contracts.Abstractions;

namespace Cps.Fct.Hk.Common.Infrastructure.Abstractions.Implementations;

/// <inheritdoc cref="IDateTimeProvider"/>
public class DateTimeProvider : IDateTimeProvider
{
    /// <inheritdoc/>
    public DateTime Now => DateTime.UtcNow;
}
