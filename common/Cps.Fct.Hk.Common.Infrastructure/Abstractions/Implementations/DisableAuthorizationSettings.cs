using System.Diagnostics.CodeAnalysis;
using Cps.Fct.Hk.Common.Contracts.Abstractions;

namespace Cps.Fct.Hk.Common.Infrastructure.Abstractions.Implementations;

/// <summary>
/// Implementation of <see cref="IAuthorizationSettings"/> to disable Authorization.
/// </summary>
[ExcludeFromCodeCoverage]
public class DisableAuthorizationSettings : IAuthorizationSettings
{
    /// <inheritdoc/>
    public bool Authorize => false;
}
