namespace Cps.Fct.Hk.Common.Contracts.Models;

using System.Net;

/// <inheritdoc />
public class Result<T> : Result
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class.
    /// </summary>
    public Result()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class.
    /// </summary>
    /// <param name="status">Http status code for this result.</param>
    public Result(HttpStatusCode status)
        : base(status)
    {
        Status = status;
    }

    /// <summary>
    /// Value.
    /// </summary>
    public T? Value { get; set; }
}
