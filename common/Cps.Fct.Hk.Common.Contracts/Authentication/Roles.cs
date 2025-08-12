namespace Cps.Fct.Hk.Common.Contracts.Authentication;

/// <summary>
/// Provides roles for use when authorizing allowed claims.
/// </summary>
public static class Roles
{
    /// <summary>
    /// Read all.
    /// </summary>
    public const string ReadAll = "Read.All";

    /// <summary>
    /// Read and write all.
    /// </summary>
    public const string ReadWriteAll = "ReadWrite.All";

    /// <summary>
    /// Gets the string representation of a role for a given resource and operation constraint.
    /// </summary>
    /// <param name="resource"></param>
    /// <param name="operationConstraint"></param>
    /// <returns></returns>
    public static string RoleFor(string resource, string operationConstraint)
    {
        return $"{resource}.{operationConstraint}";
    }
}
