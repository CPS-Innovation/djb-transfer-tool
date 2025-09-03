// <copyright file="CaseCenterConfigConstants.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.ApiClients.Constants;

/// <summary>
/// Provides constants used for accessing the api client configuration settings.
/// </summary>
public static class CaseCenterConfigConstants
{
    /// <summary>
    /// Name of the configuration section for the Case Center.
    /// </summary>
    public const string CaseCenterConfigurationName = "CaseCenter";

    /// <summary>
    /// Name of the configuration section for the Case Center API client settings.
    /// </summary>
    public const string CaseCenterApiClientConfigurationName = "CaseCenterApiClient";

    /// <summary>
    /// Name of the Case Center Api path to GetToken.
    /// </summary>
    public const string CaseCenterApiGetTokenPathName = "GetToken";

    /// <summary>
    /// Name of the Case Center Api path to GetCaseCenterCaseId.
    /// </summary>
    public const string CaseCenterApiCreateCasePathName = "CreateCase";

    /// <summary>
    /// Name of the Case Center Api path to GetCaseCenterCaseId.
    /// </summary>
    public const string CaseCenterApiGetCaseIdPathName = "GetCaseCenterCaseId";

    /// <summary>
    /// Name of the Case Center Api path to GetTemplateCases.
    /// </summary>
    public const string CaseCenterApiGetTemplateCasesPathName = "GetTemplateCases";

    /// <summary>
    /// Name of the Case Center Api path to GetBundlesInCase.
    /// </summary>
    public const string CaseCenterApiGetBundlesInCasePathName = "GetBundlesInCase";

    /// <summary>
    /// Name of the Case Center Api path to GetSectionsInBundle.
    /// </summary>
    public const string CaseCenterApiGetSectionsInBundlePathName = "GetSectionsInBundle";

    /// <summary>
    /// Name of the Case Center Api path to AddUserToCase.
    /// </summary>
    public const string CaseCenterApiAddUserToCasePathName = "AddUserToCase";

    /// <summary>
    /// Name of the Case Center Api path to UploadDocumentToCase.
    /// </summary>
    public const string CaseCenterApiUploadDocumentToCasePathName = "UploadDocumentToCase";
}
