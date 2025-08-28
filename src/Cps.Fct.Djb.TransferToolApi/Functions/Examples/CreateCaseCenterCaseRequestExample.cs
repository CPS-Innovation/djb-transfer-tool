// <copyright file="CreateCaseCenterCaseRequestExample.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Functions.Examples;

using System.Diagnostics.CodeAnalysis;
using Cps.Fct.Djb.TransferToolApi.Models.Requests;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;

/// <summary>
/// OpenAPI example generator for the request body of CreateCaseRequest.
/// </summary>
[ExcludeFromCodeCoverage]
public class CreateCaseCenterCaseRequestExample
    : OpenApiExample<CreateCaseRequest>
{
    /// <inheritdoc/>
    public override IOpenApiExample<CreateCaseRequest> Build(
        NamingStrategy? namingStrategy = null)
    {
        var exampleRequest = new CreateCaseRequest
        {
            CmsCaseId = 123456,
        };

        this.Examples.Add(OpenApiExampleResolver.Resolve(
            "CreateCaseCenterCaseRequestExample",
            exampleRequest,
            namingStrategy));

        return this;
    }
}
