namespace Cps.Fct.Djb.TransferToolApi.Shared.Dtos.Common;

public class ReturnResultDto<T>
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// A message describing the result of the operation. This may be null if no message is provided.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// The payload data returned from the API.
    /// </summary>
    public T Data { get; set; } = default!;

    /// <summary>
    /// Creates a successful response with data.
    /// </summary>
    public static ReturnResultDto<T> Success(T data, string? message = null) =>
        new() { IsSuccess = true, Message = message ?? string.Empty, Data = data };

    /// <summary>
    /// Creates a failure response.
    /// </summary>
    public static ReturnResultDto<T> Fail(string message) =>
        new() { IsSuccess = false, Message = message, Data = default! };

    /// <summary>
    /// Creates a failure response with data.
    /// </summary>
    public static ReturnResultDto<T> Fail(T data, string message) =>
        new() { IsSuccess = false, Message = message, Data = data };
}

public class ReturnResultDto
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// A message describing the result of the operation. This may be null if no message is provided.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Creates a successful response.
    /// </summary>
    public static ReturnResultDto Success( string? message = null) =>
        new() { IsSuccess = true, Message = message ?? string.Empty };

    /// <summary>
    /// Creates a failure response.
    /// </summary>
    public static ReturnResultDto Fail(string message) =>
        new() { IsSuccess = false, Message = message};
}
