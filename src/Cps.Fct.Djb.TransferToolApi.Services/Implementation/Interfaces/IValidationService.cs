// <copyright file="IValidationService.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Services.Implementation.Interfaces;

using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;
using FluentValidation.Results;

/// <summary>
/// Interface for validating a class if a validator exists.
/// </summary>
public interface IValidationService
{
    /// <summary>
    /// Validates the specified model asynchronously using FluentValidation.
    /// </summary>
    /// <typeparam name="T">The type to validate.</typeparam>
    /// <param name="model">The model to validate.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    Task<ReturnResultDto<ValidationResult?>> ValidateModelAsync<T>(T model)
        where T : class;
}
