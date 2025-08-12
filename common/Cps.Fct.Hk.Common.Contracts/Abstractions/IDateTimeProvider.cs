namespace Cps.Fct.Hk.Common.Contracts.Abstractions;

/// <summary>Retrieve DateTime information.</summary>
public interface IDateTimeProvider
{
    /// <summary>Get the DateTime as it is now.</summary>
    DateTime Now { get; }
}
