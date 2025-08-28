// <copyright file="CreateCaseRequestValidator.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Validators;

using Cps.Fct.Djb.TransferToolApi.Models.Requests;
using Cps.Fct.Djb.TransferToolApi.Shared.Interfaces;
using FluentValidation;

/// <summary>
/// Validator for Create Case Request.
/// </summary>
public class CreateCaseRequestValidator : AbstractValidator<CreateCaseRequest>, IValidatorDependencyScanner
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCaseRequestValidator"/> class.
    /// </summary>
    public CreateCaseRequestValidator()
    {
        this.RuleFor(x => x.CmsCaseId)
            .GreaterThan(0).WithMessage("CmsCaseId is required.");

        this.RuleFor(x => x.CmsUsername)
            .NotEmpty().WithMessage("CmsUsername is required.");
    }
}
