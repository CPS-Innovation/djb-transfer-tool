using System.Net;
using System.Runtime.Serialization;

namespace Cps.Fct.Hk.Common.Contracts.Exceptions;

/// <summary>
/// Bad request exception class.
/// </summary>
[Serializable]
public class BadRequestException : Exception, IResponseStatus
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class.
    /// </summary>
    public BadRequestException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class.
    /// </summary>
    /// <param name="message"></param>
    public BadRequestException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public BadRequestException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class.
    /// with info and context.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected BadRequestException(SerializationInfo info, StreamingContext context)
    {
    }

    /// <inheritdoc/>
    public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
