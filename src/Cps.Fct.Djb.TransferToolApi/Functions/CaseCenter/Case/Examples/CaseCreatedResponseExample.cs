// <copyright file="CaseCreatedResponseExample.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Functions.CaseCenter.Case.Examples;

using System.Diagnostics.CodeAnalysis;
using Cps.Fct.Djb.TransferToolApi.Models.Responses.Case;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;

/// <summary>
/// OpenAPI example generator for the response body of CreateCaseCenterCase.
/// </summary>
[ExcludeFromCodeCoverage]
public class CaseCreatedResponseExample
    : OpenApiExample<CaseCreatedResponse>
{
    /// <inheritdoc/>
    public override IOpenApiExample<CaseCreatedResponse> Build(
        NamingStrategy? namingStrategy = null)
    {
        // Construct a sample token response
        var exampleResponse = new CaseCreatedResponse
        {
            CaseCenterCaseId = "471ea6f116f74fdcb4ecc8996339f373",
        };

        this.Examples.Add(OpenApiExampleResolver.Resolve(
            "CaseCreatedResponseExample",
            exampleResponse,
            namingStrategy));

        return this;
    }
}
