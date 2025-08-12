namespace Cps.Fct.Hk.Common.Contracts.Abstractions;

// TODO: [AUTH] hack until we sort out authorization in test

/// <summary>
/// Settings to configure the usage of authorization.
/// </summary>
public interface IAuthorizationSettings
{
    /// <summary>Should the system use authorization.</summary>
    bool Authorize { get; }
}
