// <copyright file="HttpReturnResultDto.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;

using System.Net;

/// <summary>
/// HttpReturnResultDto.
/// </summary>
/// <typeparam name="T">The data type to return.</typeparam>
public class HttpReturnResultDto<T>
{
    /// <summary>
    /// Gets or sets a message describing the result of the operation.
    /// Defaults to an empty string if not provided.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the payload data returned from the API.
    /// Non-nullable in the signature; can be null at runtime if not set.
    /// </summary>
    public T Data { get; set; } = default!;

    /// <summary>
    /// Gets or sets the HTTP status code returned from the API.
    /// If default (0), we treat it as no status code.
    /// </summary>
    public HttpStatusCode StatusCode { get; set; } = default!;

    /// <summary>
    /// Gets a value indicating whether the operation was successful, derived from <see cref="StatusCode"/>.
    /// True if <see cref="StatusCode"/> is a 2xx code; false otherwise.
    /// </summary>
    public bool IsSuccess
    {
        get
        {
            if (this.StatusCode == default)
            {
                // No status code at all => fail
                return false;
            }

            int codeInt = (int)this.StatusCode;
            return codeInt >= 200 && codeInt < 300;
        }
    }

    /// <summary>
    /// Creates a failure result with no data.
    /// If <paramref name="message"/> is null or whitespace, defaults to "Failed".
    /// </summary>
    /// <param name="statusCode">The status code to return.</param>
    /// <param name="message">The message to return.</param>
    /// <returns>Returns a failed HttpResponse with an optional message.</returns>
    public static HttpReturnResultDto<T> Fail(HttpStatusCode statusCode, string? message = null)
        => new()
        {
            StatusCode = statusCode,
            Message = string.IsNullOrWhiteSpace(message) ? "Failed" : message,
            Data = default!,
        };

    /// <summary>
    /// Creates a success result with data.
    /// </summary>
    /// <param name="statusCode">The status code to return.</param>
    /// <param name="data">The data to return.</param>
    /// <returns>Returns a success HttpResponse with a data payload.</returns>
    public static HttpReturnResultDto<T> Success(HttpStatusCode statusCode, T data)
        => new()
        {
            StatusCode = statusCode,
            Data = data,
        };
}
