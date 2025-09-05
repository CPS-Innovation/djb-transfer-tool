// <copyright file="DocumentsUploadedFromCmsBundleResponseExample.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Functions.CaseCenter.Document.Examples;

using System.Diagnostics.CodeAnalysis;
using Cps.Fct.Djb.TransferToolApi.Models.Responses.Document;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;

/// <summary>
/// OpenAPI example generator for the response body of CreateCaseCenterCase.
/// </summary>
[ExcludeFromCodeCoverage]
public class DocumentsUploadedFromCmsBundleResponseExample
    : OpenApiExample<DocumentsUploadedFromCmsBundleResponse>
{
    /// <inheritdoc/>
    public override IOpenApiExample<DocumentsUploadedFromCmsBundleResponse> Build(
        NamingStrategy? namingStrategy = null)
    {
        // Construct a sample token response
        var exampleResponse = new DocumentsUploadedFromCmsBundleResponse
        {
            DocumentResponses = new List<DocumentUploadResponse>()
            {
                new DocumentUploadResponse()
                {
                    IsUploaded = true,
                    CmsDocumentId = 100001,
                    CaseCenterDocumentId = "d1f5c3e2b4a14f8e9c6e7a8b9c0d1e2f",
                    ErrorMessage = string.Empty,
                },
                new DocumentUploadResponse()
                {
                    IsUploaded = false,
                    CmsDocumentId = 100002,
                    CaseCenterDocumentId = string.Empty,
                    ErrorMessage = "Something went wrong",
                },
            },
        };

        this.Examples.Add(OpenApiExampleResolver.Resolve(
            "DocumentsUploadedFromCmsBundleResponseExample",
            exampleResponse,
            namingStrategy));

        return this;
    }
}
