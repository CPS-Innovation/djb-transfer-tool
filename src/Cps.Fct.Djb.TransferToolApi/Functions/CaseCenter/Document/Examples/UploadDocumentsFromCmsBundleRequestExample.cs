// <copyright file="UploadDocumentsFromCmsBundleRequestExample.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Functions.CaseCenter.Document.Examples;

using System.Diagnostics.CodeAnalysis;
using Cps.Fct.Djb.TransferToolApi.Models.Requests.Document;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;

/// <summary>
/// OpenAPI example generator for the request body of UploadDocumentsFromCmsBundleRequest.
/// </summary>
[ExcludeFromCodeCoverage]
public class UploadDocumentsFromCmsBundleRequestExample
    : OpenApiExample<UploadDocumentsFromCmsBundleRequest>
{
    /// <inheritdoc/>
    public override IOpenApiExample<UploadDocumentsFromCmsBundleRequest> Build(
        NamingStrategy? namingStrategy = null)
    {
        var exampleRequest = new UploadDocumentsFromCmsBundleRequest
        {
            CmsCaseId = 123456,
            CmsUsername = "cms.user@cps.gov.uk",
        };

        this.Examples.Add(OpenApiExampleResolver.Resolve(
            "UploadDocumentsFromCmsBundleRequestExample",
            exampleRequest,
            namingStrategy));

        return this;
    }
}
