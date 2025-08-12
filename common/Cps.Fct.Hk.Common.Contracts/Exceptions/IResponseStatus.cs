namespace Cps.Fct.Hk.Common.Contracts.Exceptions;

using System.Net;

/// <summary>
/// Details around Http Responses.
/// </summary>
public interface IResponseStatus
{
    /// <summary>
    /// The status code for the response.
    /// </summary>
    public HttpStatusCode StatusCode { get; }
}
