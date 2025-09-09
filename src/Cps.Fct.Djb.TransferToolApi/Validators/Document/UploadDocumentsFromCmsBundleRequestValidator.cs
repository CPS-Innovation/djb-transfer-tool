// <copyright file="UploadDocumentsFromCmsBundleRequestValidator.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Validators.Document;

using Cps.Fct.Djb.TransferToolApi.Models.Requests.Document;
using Cps.Fct.Djb.TransferToolApi.Shared.Interfaces;
using FluentValidation;

/// <summary>
/// Validator for UploadDocumentsFromCmsBundleRequest.
/// </summary>
public class UploadDocumentsFromCmsBundleRequestValidator : AbstractValidator<UploadDocumentsFromCmsBundleRequest>, IValidatorDependencyScanner
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UploadDocumentsFromCmsBundleRequestValidator"/> class.
    /// </summary>
    public UploadDocumentsFromCmsBundleRequestValidator()
    {
        this.RuleFor(x => x.CmsCaseId)
            .GreaterThan(0).WithMessage("CmsCaseId is required.");

        this.RuleFor(x => x.CmsBundleId)
            .GreaterThan(0).WithMessage("CmsBundleId is required.");

        this.RuleFor(x => x.DocumentUploader)
            .NotEmpty().WithMessage("CmsUsername is required.");
    }
}
