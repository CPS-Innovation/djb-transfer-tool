// <copyright file="ReturnResultDto.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;

/// <summary>
/// A generic return result DTO for API responses.
/// </summary>
/// <typeparam name="T">The data type to return.</typeparam>
public class ReturnResultDto<T>
{
    /// <summary>
    /// Gets or sets a value indicating whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets a value describing the result of the operation. This may be null if no message is provided.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value for the payload data returned from the API.
    /// </summary>
    public T Data { get; set; } = default!;

    /// <summary>
    /// Creates a successful response.
    /// </summary>
    /// <param name="data">The data to return.</param>
    /// <param name="message">An optional message.</param>
    /// <returns>A response object.</returns>
    public static ReturnResultDto<T> Success(T data, string? message = null) =>
        new() { IsSuccess = true, Message = message ?? string.Empty, Data = data };

    /// <summary>
    /// Creates a failure response without data.
    /// </summary>
    /// <param name="message">The message for the falure.</param>
    /// <returns>A response object.</returns>
    public static ReturnResultDto<T> Fail(string message) =>
        new() { IsSuccess = false, Message = message, Data = default! };

    /// <summary>
    /// Creates a failure response with data.
    /// </summary>
    /// <param name="data">The data to return.</param>
    /// <param name="message">The message to return.</param>
    /// <returns>A response object.</returns>
    public static ReturnResultDto<T> Fail(T data, string message) =>
        new() { IsSuccess = false, Message = message, Data = data };
}

/// <summary>
/// A non-generic return result DTO for API responses.
/// </summary>
public class ReturnResultDto
{
    /// <summary>
    /// Gets or sets a value indicating whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets a value describing the result of the operation. This may be null if no message is provided.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Creates a successful response.
    /// </summary>
    /// <param name="message">An optional message.</param>
    /// <returns>A response object.</returns>
    public static ReturnResultDto Success(string? message = null) =>
        new() { IsSuccess = true, Message = message ?? string.Empty };

    /// <summary>
    /// Creates a failure response.
    /// </summary>
    /// <param name="message">The message to return.</param>
    /// <returns>A response object.</returns>
    public static ReturnResultDto Fail(string message) =>
        new() { IsSuccess = false, Message = message };
}
