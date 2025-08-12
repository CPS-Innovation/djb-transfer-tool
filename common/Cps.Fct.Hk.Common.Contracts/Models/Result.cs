using System.Net;

namespace Cps.Fct.Hk.Common.Contracts.Models;

/// <inheritdoc cref="IResult"/>
public class Result : IResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    public Result()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    /// <param name="status">Http status code for this result.</param>
    public Result(HttpStatusCode status)
    {
        Status = status;
    }

    /// <inheritdoc/>
    public object? CustomState { get; set; }

    /// <summary>
    /// Validation errors.
    /// </summary>
    public ValidationProblemDetails? ValidationErrors => CustomState as ValidationProblemDetails;

    /// <inheritdoc/>
    public HttpStatusCode Status { get; set; }

    /// <summary>
    /// Method for generating a bad request result.
    /// </summary>
    /// <typeparam name="T">type of result.</typeparam>
    /// <param name="customState">Error message.</param>
    /// <returns></returns>
    public static Result<T> BadRequest<T>(string customState)
    {
        return new(HttpStatusCode.BadRequest) { CustomState = new ValidationProblemDetails { Title = customState } };
    }

    /// <summary>
    /// Method for generating a bad request result with validation errors.
    /// </summary>
    /// <typeparam name="T">type of result.</typeparam>
    /// <param name="customState"></param>
    /// <returns></returns>
    public static Result<T> BadRequest<T>(ValidationProblemDetails? customState = null)
    {
        return new(HttpStatusCode.BadRequest) { CustomState = customState };
    }

    /// <summary>
    /// Method for generating a created result.
    /// </summary>
    /// <typeparam name="T">type of result.</typeparam>
    /// <param name="value">The created resource.</param>
    /// <param name="location">The location of the created resource.</param>
    /// <returns></returns>
    public static Result<T> Created<T>(T value, Uri? location = null)
    {
        return new(HttpStatusCode.Created) { Value = value, CustomState = location };
    }

    /// <summary>
    /// Method for generating a updated result.
    /// </summary>
    /// <typeparam name="T">type of result.</typeparam>
    /// <param name="value">The updated resource.</param>
    /// <param name="location">The location of the updated resource.</param>
    /// <returns></returns>
    public static Result<T> Updated<T>(T value, Uri? location = null)
    {
        return new(HttpStatusCode.OK) { Value = value, CustomState = location };
    }

    /// <summary>
    /// Method for generating an Internal Server Error result.
    /// </summary>
    /// <typeparam name="T">type of result.</typeparam>
    /// <param name="customState"></param>
    /// <returns></returns>
    public static Result<T> InternalServerError<T>(object? customState = null)
    {
        return new(HttpStatusCode.InternalServerError) { CustomState = customState };
    }

    /// <summary>
    /// Method for generating a no content result.
    /// </summary>
    /// <typeparam name="T">type of result.</typeparam>
    /// <returns></returns>
    public static Result<T> NoContent<T>()
    {
        return new(HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Method for generating a not found result.
    /// </summary>
    /// <typeparam name="T">type of result.</typeparam>
    /// <param name="customState"></param>
    /// <returns></returns>
    public static Result<T> NotFound<T>(object? customState = null)
    {
        return new(HttpStatusCode.NotFound) { CustomState = customState };
    }

    /// <summary>
    /// Method for generating an ok result.
    /// </summary>
    /// <typeparam name="T">type of result.</typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Result<T> Ok<T>(T value)
    {
        return new(HttpStatusCode.OK) { Value = value };
    }

    /// <summary>
    /// Method for generating a status code result.
    /// </summary>
    /// <typeparam name="T">type of result.</typeparam>
    /// <param name="status"></param>
    /// <param name="customState"></param>
    /// <returns></returns>
    public static Result<T> StatusCode<T>(HttpStatusCode status, object? customState = null)
    {
        return new(status) { CustomState = customState };
    }

    /// <summary>
    /// Method for generating an unauthorized result.
    /// </summary>
    /// <typeparam name="T">type of result.</typeparam>
    /// <param name="customState"></param>
    /// <returns></returns>
    public static Result<T> Unauthorized<T>(object? customState = null)
    {
        return new(HttpStatusCode.Unauthorized) { CustomState = customState };
    }

    /// <summary>
    /// Indicates whether the result status code is a success code.
    /// </summary>
    /// <returns></returns>
    public bool IsSuccess()
    {
        return (int)Status is >= 200 and <= 299;
    }
}
