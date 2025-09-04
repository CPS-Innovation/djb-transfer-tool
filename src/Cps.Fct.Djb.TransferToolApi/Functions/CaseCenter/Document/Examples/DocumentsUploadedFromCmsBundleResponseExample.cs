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
            UploadedDocumentCaseCenterIds = new List<string>
            {
                "d1f5c3e2b4a14f8e9c6e7a8b9c0d1e2f",
                "a2b3c4d5e6f708192837465564738291",
                "b1c2d3e4f5a607182736455667788990",
            },
        };

        this.Examples.Add(OpenApiExampleResolver.Resolve(
            "DocumentsUploadedFromCmsBundleResponseExample",
            exampleResponse,
            namingStrategy));

        return this;
    }
}
