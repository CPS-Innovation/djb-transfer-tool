// <copyright file="ValidationService.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Services;

using Cps.Fct.Djb.TransferTool.Services.Services.Interfaces;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;
using FluentValidation;
using FluentValidation.Results;

/// <summary>
/// Class for validating a class if a validator exists.
/// </summary>
public class ValidationService : IValidationService
{
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationService"/> class.
    /// </summary>
    /// <param name="serviceProvider">IServiceProvider.</param>
    public ValidationService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Validates the specified model asynchronously using FluentValidation.
    /// </summary>
    /// <typeparam name="T">The type to validate.</typeparam>
    /// <param name="model">The model to validate.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    public async Task<ReturnResultDto<ValidationResult?>> ValidateModelAsync<T>(T model)
        where T : class
    {
        // Use the non-generic GetService(Type) and cast
        var validator = this.serviceProvider.GetService(typeof(IValidator<T>)) as IValidator<T>;
        if (validator == null)
        {
            return ReturnResultDto<ValidationResult?>.Success(
                data: null,
                message: $"No validator registered for '{typeof(T).Name}'. Skipping validation.");
        }

        ValidationResult validationResult = await validator.ValidateAsync(model).ConfigureAwait(false);
        if (!validationResult.IsValid)
        {
            // Aggregate error messages
            string errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));

            // Use the Fail(data, message) overload to return the validation details too
            return ReturnResultDto<ValidationResult?>.Fail(
                data: validationResult,
                message: $"Validation failed for '{typeof(T).Name}': {errors}");
        }

        return ReturnResultDto<ValidationResult?>.Success(
            validationResult,
            $"Validation succeeded for '{typeof(T).Name}'.");
    }

    /// <summary>
    /// Converts a FluentValidation <see cref="ValidationResult"/> into a
    /// <see cref="ValidationErrorResponseDto"/> with top-level message and property-level errors.
    /// </summary>
    /// <param name="validationResult">The FluentValidation result to convert.</param>
    /// <param name="topLevelMessage">
    /// (Optional) A top-level message describing the errors. Defaults if not provided.
    /// </param>
    /// <returns>A structured <see cref="ValidationErrorResponseDto"/>.</returns>
    public ValidationErrorResponseDto BuildValidationFailureResponse(
        ValidationResult? validationResult,
        string? topLevelMessage = null)
    {
        // Safely handle no validation result or empty errors
        var errors = validationResult?.Errors
            .Select(e => new ValidationErrorDto
            {
                PropertyName = e.PropertyName,
                ErrorMessage = e.ErrorMessage,
            })
            .ToArray() ?? Array.Empty<ValidationErrorDto>();

        return new ValidationErrorResponseDto
        {
            Message = !string.IsNullOrWhiteSpace(topLevelMessage)
                ? topLevelMessage
                : "Validation errors occurred.",
            Errors = errors,
        };
    }
}
