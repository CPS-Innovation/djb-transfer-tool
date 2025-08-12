namespace Cps.Fct.Hk.Common.Contracts;
/// <summary>
/// Abstract base class for basic utility methods like equality.
/// </summary>
// Learn more: https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/implement-value-objects
public abstract class ValueObject
{
    /// <summary>
    /// Check if instance is equal to a compared object.
    /// </summary>
    /// <param name="obj">Object being compared to.</param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }

    /// <summary>
    /// Check if two instances have the same values as one another.
    /// </summary> 
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>True if objects are equal, false otherwise.</returns>
    protected static bool EqualOperator(ValueObject? left, ValueObject? right)
    {
        if (left is null ^ right is null)
        {
            return false;
        }

        return left?.Equals(right) != false;
    }

    /// <summary>
    /// Check if two instances are not the same as one another.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>True if objects are not equal, false otherwise.</returns>
    protected static bool NotEqualOperator(ValueObject? left, ValueObject? right)
    {
        return !EqualOperator(left, right);
    }

    /// <summary>
    /// Method to be implemented by derived classes to provide equality components.
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerable<object?> GetEqualityComponents();
}
