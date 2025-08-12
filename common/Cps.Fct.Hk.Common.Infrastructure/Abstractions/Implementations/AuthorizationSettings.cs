using Cps.Fct.Hk.Common.Contracts.Abstractions;

namespace Cps.Fct.Hk.Common.Infrastructure.Abstractions.Implementations;

/// <inheritdoc cref="IAuthorizationSettings"/>
public class AuthorizationSettings : IAuthorizationSettings
{
    /// <inheritdoc />
    public bool Authorize => true;
}
