namespace Cps.Fct.Hk.Common.Contracts.Exceptions;
/// <summary>
/// Unauthorized exception class.
/// </summary>
public class UnauthorizedException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnauthorizedException"/> class.
    /// </summary>
    public UnauthorizedException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnauthorizedException"/> class.
    /// </summary>
    /// <param name="message"></param>
    public UnauthorizedException(string message)
        : base(message)
    {
    }
}
