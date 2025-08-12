namespace Cps.Fct.Hk.Common.Contracts.Clients;

/// <summary>
/// Message from API containing error details.
/// </summary>
public interface IExternalErrorResponse
{
    /// <summary>
    /// Return the error details from the response.
    /// </summary>
    /// <returns></returns>
    public string GetError();
}
