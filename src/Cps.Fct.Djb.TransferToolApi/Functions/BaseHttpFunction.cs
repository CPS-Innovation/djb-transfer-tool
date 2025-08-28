// <copyright file="BaseHttpFunction.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Functions;

using System.Net;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;
using FluentValidation.Results;
using Microsoft.Azure.Functions.Worker.Http;

/// <summary>
/// An abstract base class for Azure HTTP-triggered functions.
/// Includes a helper method to build a 400 Bad Request response
/// from a ReturnResult containing a FluentValidation ValidationResult.
/// </summary>
public abstract class BaseHttpFunction
{
    /// <summary>
    /// Safely attempts to read and deserialise the request body as JSON of type <typeparamref name="T"/>.
    /// If the body is null/empty or if deserialisation fails, it returns null.
    /// Otherwise, it returns the deserialised model.
    /// </summary>
    /// <typeparam name="T">The type to deserialise into.</typeparam>
    /// <param name="httpRequestData">The incoming HTTP request.</param>
    /// <returns>The deserialised model if successful; otherwise null.</returns>
    protected async Task<T?> TryReadRequestBodyAsync<T>(HttpRequestData httpRequestData)
        where T : class
    {
        try
        {
            return await httpRequestData.ReadFromJsonAsync<T>().ConfigureAwait(false);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Builds an HTTP 400 response by inspecting a ValidationResult,
    /// converting it into a ValidationErrorResponseDto, and serializing to JSON.
    /// </summary>
    /// <param name="httpRequestData">
    /// The Function's incoming request data (for creating a response).
    /// </param>
    /// <param name="validationResult">
    /// The FluenValidation ValidationResult.
    /// </param>
    /// <returns>The a 400 response based on validation errors.</returns>
    protected async Task<HttpResponseData> BuildBadRequestResponseFromValidationAsync(
        HttpRequestData httpRequestData,
        ValidationResult? validationResult)
    {
        var errorDtos = validationResult is not null
            ? validationResult.Errors
                .Select(e => new ValidationErrorDto
                {
                    PropertyName = e.PropertyName,
                    ErrorMessage = e.ErrorMessage,
                })
                .ToArray() ?? Array.Empty<ValidationErrorDto>()
            : Array.Empty<ValidationErrorDto>();

        var errorResponse = new ValidationErrorResponseDto
        {
            Message = "Validation errors occurred.",
            Errors = errorDtos,
        };

        var badRequest = httpRequestData.CreateResponse(HttpStatusCode.BadRequest);
        await badRequest.WriteAsJsonAsync(errorResponse).ConfigureAwait(false);
        return badRequest;
    }
}
