using System.Net;
using System.Runtime.Serialization;

namespace Cps.Fct.Hk.Common.Contracts.Exceptions;

/// <summary>
/// API exception class.
/// </summary>
[Serializable]
public class ApiException : Exception, IResponseStatus
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class.
    /// </summary>
    public ApiException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class.
    /// </summary>
    /// <param name="message"></param>
    public ApiException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public ApiException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class.
    /// with info and context.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected ApiException(SerializationInfo info, StreamingContext context)
    {
    }

    /// <summary>
    /// Status code for exception.
    /// </summary>
    public HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;
}
