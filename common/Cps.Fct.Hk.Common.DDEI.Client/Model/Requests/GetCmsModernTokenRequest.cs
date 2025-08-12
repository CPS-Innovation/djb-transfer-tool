namespace Cps.Fct.Hk.Common.DDEI.Client.Model.Requests;

/// <summary>
/// The request to get the CMS Modern token.
/// </summary>
/// <param name="CmsAuthValues">The user cookie.</param>
/// <param name="CorrespondenceId">The correspondence ID.</param>
public record GetCmsModernTokenRequest(CmsAuthValues CmsAuthValues, Guid CorrespondenceId)
        : BaseRequest(CorrespondenceId: CorrespondenceId)
{
}

