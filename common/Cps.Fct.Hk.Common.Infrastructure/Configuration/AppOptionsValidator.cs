using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Cps.Fct.Hk.Common.Infrastructure.Configuration;

/// <summary>
/// Validate App options are configured correctly.
/// </summary>
public class AppOptionsValidator : IValidateOptions<AppOptions>
{
    private readonly ConnectionStringsOptions? _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppOptionsValidator"/> class.
    /// </summary>
    /// <param name="config"></param>
    public AppOptionsValidator(IConfiguration config)
    {
        _options = config.GetSection(new ConnectionStringsOptions().SectionName)
            .Get<ConnectionStringsOptions?>();
    }

    /// <inheritdoc/>
    public ValidateOptionsResult Validate(string? name, AppOptions options)
    {
        return ValidateOptionsResult.Success;
    }
}
