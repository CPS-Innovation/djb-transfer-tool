namespace Cps.Fct.Hk.Common.Contracts.Models;

/// <summary>
/// ValidationProblemDetails.
/// </summary>
public class ValidationProblemDetails : ProblemDetails
{
    /// <summary>
    /// Errors.
    /// </summary>
    public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();

    /// <summary>
    /// Object initializer.
    /// </summary>
    public ValidationProblemDetails()
    {
        Title = "One or more validation errors occurred.";
        Status = 400; // Bad Request
    }

    /// <summary>
    /// Object initializer.
    /// </summary>
    /// <param name="errors"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public ValidationProblemDetails(IDictionary<string, string[]> errors) : this()
    {
        Errors = errors ?? throw new ArgumentNullException(nameof(errors));
    }
}
