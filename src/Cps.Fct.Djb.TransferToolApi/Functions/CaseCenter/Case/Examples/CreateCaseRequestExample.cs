// <copyright file="CreateCaseRequestExample.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Functions.CaseCenter.Case.Examples;

using System.Diagnostics.CodeAnalysis;
using Cps.Fct.Djb.TransferToolApi.Models.Requests.Case;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;

/// <summary>
/// OpenAPI example generator for the request body of CreateCaseRequest.
/// </summary>
[ExcludeFromCodeCoverage]
public class CreateCaseRequestExample
    : OpenApiExample<CreateCaseRequest>
{
    /// <inheritdoc/>
    public override IOpenApiExample<CreateCaseRequest> Build(
        NamingStrategy? namingStrategy = null)
    {
        var exampleRequest = new CreateCaseRequest
        {
            CmsCaseId = 123456,
            CmsUsername = "cms.user@cps.gov.uk",
        };

        this.Examples.Add(OpenApiExampleResolver.Resolve(
            "CreateCaseRequestExample",
            exampleRequest,
            namingStrategy));

        return this;
    }
}
