// <copyright file="BaseHttpFunction.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Functions;

using System.Net;
using System.Text.Json;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;
using Cps.Fct.Hk.Common.DDEI.Client.Model;
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
    /// Checks if the incoming HTTP request contains a valid "Cms-Auth-Values" header.
    /// </summary>
    /// <param name="request">HttpRequestData.</param>
    /// <returns>True if valid otherwise false.</returns>
    protected bool HasCmsAuthValuesHeader(HttpRequestData request)
    {
        if (!request.Headers.Contains("Cms-Auth-Values"))
        {
            return false;
        }

        var rawHeader = request.Headers.GetValues("Cms-Auth-Values")?.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(rawHeader))
        {
            return false;
        }

        try
        {
            var temp = JsonSerializer.Deserialize<JsonElement>(
                rawHeader,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (!temp.TryGetProperty("Cookies", out var cookies) || string.IsNullOrWhiteSpace(cookies.GetString()))
            {
                return false;
            }

            if (!temp.TryGetProperty("Token", out var token) || string.IsNullOrWhiteSpace(token.GetString()))
            {
                return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Extracts and deserializes the "Cms-Auth-Values" header from the HTTP request into a CmsAuthValues object.
    /// </summary>
    /// <param name="request">HttpRequestData.</param>
    /// <returns>A nullable CMSAuthValues.</returns>
    protected CmsAuthValues? GetCmsAuthValues(HttpRequestData request)
    {
        if (!request.Headers.Contains("Cms-Auth-Values"))
        {
            return null;
        }

        var rawHeader = request.Headers.GetValues("Cms-Auth-Values")?.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(rawHeader))
        {
            return null;
        }

        try
        {
            var temp = JsonSerializer.Deserialize<JsonElement>(
                rawHeader,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var cookies = temp.GetProperty("Cookies").GetString();
            var token = temp.GetProperty("Token").GetString();

            return new CmsAuthValues(cookies!, token!);
        }
        catch
        {
            return null;
        }
    }

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

    /// <summary>
    /// Gets a function's qualified name by extracting the namespace and class name
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>The qualified name.</returns>
    protected string GetFunctionQualifiedName(Type type)
    {
        const string marker = "Functions.";

        var fullName = type.FullName ?? type.Name;

        var index = fullName.IndexOf(marker, StringComparison.Ordinal);
        if (index < 0)
        {
            return fullName;
        }

        return fullName.Substring(index);
    }
}
