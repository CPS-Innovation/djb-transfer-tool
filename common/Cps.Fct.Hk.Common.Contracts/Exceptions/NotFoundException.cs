namespace Cps.Fct.Hk.Common.Contracts.Exceptions;

using System.Net;

/// <summary>
/// Not found exception class.
/// </summary>
public class NotFoundException : Exception, IResponseStatus
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class.
    /// </summary>
    public NotFoundException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class.
    /// </summary>
    /// <param name="message"></param>
    public NotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="key"></param>
    public NotFoundException(string name, object key)
        : base($"{name} ({key}) was not found.")
    {
    }

    /// <inheritdoc/>
    public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

    HttpStatusCode IResponseStatus.StatusCode => throw new NotImplementedException();
}
