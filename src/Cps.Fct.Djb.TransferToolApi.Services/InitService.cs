// <copyright file="InitService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Services;

using System.Globalization;
using Cps.Fct.Djb.TransferToolApi.Services.Contracts;
using Cps.Fct.Djb.TransferToolApi.Services.Dto;
using Cps.Fct.Hk.Common.Contracts.Logging;
using Cps.Fct.Hk.Common.DDEI.Client.Model;
using Cps.Fct.Hk.Common.DDEI.Client.Model.Requests;
using Cps.Fct.Hk.Common.DDEI.Provider.Contracts;
using Microsoft;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

/// <summary>
/// Provides the implementation for processing initialization requests.
/// </summary>
public class InitService : IInitService
{
    private readonly ILogger<InitService> logger;
    private readonly IConfiguration config;
    private readonly ICmsModernProvider cmsModernProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="InitService"/> class.
    /// </summary>
    /// <param name="logger">The logger instance used for logging information and errors.</param>
    /// <param name="config">The configuration instance used for retrieving settings.</param>
    /// <param name="cmsModernProvider">The API client instance used for communication with external services.</param>
    public InitService(ILogger<InitService> logger, IConfiguration config, ICmsModernProvider cmsModernProvider)
    {
        Requires.NotNull(logger);
        Requires.NotNull(config);
        Requires.NotNull(cmsModernProvider);
        this.logger = logger;
        this.config = config;
        this.cmsModernProvider = cmsModernProvider;
    }

    /// <inheritdoc />
    public async Task<InitResult> ProcessRequest(HttpRequest req, int? caseId, string? cc)
    {
        // Retrieve values from configuration
        string redirectUrlCwa = this.config["RedirectUrl:CaseworkApp"] ?? string.Empty;
        string redirectUrlHkUi = this.config["RedirectUrl:HousekeepingUi"] ?? string.Empty;

        if (string.IsNullOrEmpty(redirectUrlCwa) || string.IsNullOrEmpty(redirectUrlHkUi))
        {
            this.logger.LogError($"{LoggingConstants.HskUiLogPrefix} One or more Housekeeping re-direct URL's are missing.");

            return new InitResult
            {
                Status = InitResultStatus.ServerError,
                Message = "One or more Housekeeping re-direct URL's are missing.",
            };
        }

        // Log a warning when the cc query parameter is missing
        if (string.IsNullOrEmpty(cc))
        {
            this.logger.LogWarning($"{LoggingConstants.HskUiLogPrefix} The 'cc' query parameter is missing from the request for [{caseId}]");
        }

        // Set cookies only if cc is available
        if (caseId.HasValue && !string.IsNullOrEmpty(cc))
        {
            string? ct = null;

            try
            {
                var correspondenceId = Guid.NewGuid();
                var cmsAuthValues = new CmsAuthValues(cc, Guid.NewGuid().ToString(), correspondenceId);
                var request = new GetCmsModernTokenRequest(cmsAuthValues, correspondenceId);
                ct = await this.cmsModernProvider.GetCmsModernTokenAsync(request).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"{LoggingConstants.HskUiLogPrefix} Unable to process request for caseId [{caseId}]: ", ex.Message);
            }

            return new InitResult
            {
                Status = InitResultStatus.Redirect,
                RedirectUrl = redirectUrlHkUi, // HK UI home page
                ShouldSetCookie = true,
                CaseId = caseId.Value.ToString(CultureInfo.InvariantCulture),
                Cc = cc,
                Ct = ct,
            };
        }

        // Redirect to CWA if cc is missing
        string redirectUrl = BuildRedirectUrl(req, caseId, redirectUrlCwa);

        return new InitResult
        {
            Status = InitResultStatus.Redirect,
            RedirectUrl = redirectUrl, // CWA cookie handoff endpoint
        };
    }

    /// <summary>
    /// Builds the full redirect URL for the case initialization process.
    /// </summary>
    /// <param name="req">The HTTP request that contains the scheme and host information.</param>
    /// <param name="caseId">The case ID to be used in the redirect query string.</param>
    /// <param name="redirectUrlCwa">The base redirect URL for the CWA (Casework App).</param>
    /// <returns>A complete redirect URL combining the base URL and the query string with the case ID.</returns>
    internal static string BuildRedirectUrl(HttpRequest req, int? caseId, string redirectUrlCwa)
    {
        if (caseId == null)
        {
            throw new ArgumentNullException(nameof(caseId), "Case ID cannot be null.");
        }

        // Get the host from the request
        string host = $"{req.Scheme}://{req.Host}";

        // Build up redirect query string
        string redirectEndpoint = "/api/init/" + caseId.Value.ToString(CultureInfo.InvariantCulture);
        string redirectQueryParameter = host + redirectEndpoint;

        return redirectUrlCwa + redirectQueryParameter;
    }
}
