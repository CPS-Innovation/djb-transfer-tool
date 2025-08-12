namespace Cps.Fct.Hk.Common.Contracts.Helpers;

using System.Reflection;

/// <summary>
/// Provides static extension methods for reflection.
/// </summary>
public static class ReflectionHelpers
{
    /// <summary>
    /// Gets all the decorated attributes for a class.
    /// </summary>
    /// <typeparam name="TClass">Class type.</typeparam>
    /// <typeparam name="TAttribute">Attribute type.</typeparam>
    /// <returns></returns>
    public static TAttribute[] GetClassAttributes<TClass, TAttribute>()
        where TClass : class
        where TAttribute : Attribute
    {
        return typeof(TClass)
            .GetCustomAttributes<TAttribute>()
            .Select(x => x).ToArray();
    }

    /// <summary>
    /// Gets only the decorated attributes with type TAttribute.
    /// </summary>
    /// <typeparam name="TAttribute">Attribute type.</typeparam>
    /// <param name="controllerType"></param>
    /// <returns></returns>
    public static TAttribute[] GetClassAttributes<TAttribute>(Type controllerType)
        where TAttribute : Attribute
    {
        return controllerType
            .GetCustomAttributes<TAttribute>()
            .Select(x => x).ToArray();
    }

    /// <summary>
    /// Gets only the decorated attributes with type TAttribute.
    /// </summary>
    /// <typeparam name="TClass">Class type.</typeparam>
    /// <typeparam name="TAttribute">Attribute type.</typeparam>
    /// <param name="methodName">Name of the decorated method.</param>
    /// <returns></returns>
    public static TAttribute[] GetMethodAttributes<TClass, TAttribute>(string methodName)
        where TClass : class
        where TAttribute : Attribute
    {
        var methodInfo = GetMethodInfo<TClass>(methodName);

        return methodInfo!
            .GetCustomAttributes<TAttribute>(false)
            .Select(x => x).ToArray();
    }

    /// <summary>
    /// Gets all the decorated attributes for a method.
    /// </summary>
    /// <typeparam name="TClass">Class type.</typeparam>
    /// <param name="methodName"></param>
    /// <param name="attributeType"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">When the method cannot be found.</exception>
    public static object[] GetMethodAttributes<TClass>(string methodName, Type attributeType)
        where TClass : class
    {
        var methodInfo = GetMethodInfo<TClass>(methodName)
            ?? throw new InvalidOperationException($"Could not find method named: {methodName}");

        return methodInfo.GetCustomAttributes(attributeType, false);
    }

    /// <summary>
    /// Gets <see cref="MethodInfo"/> for a given method in a class.
    /// </summary>
    /// <typeparam name="TClass">Class with method.</typeparam>
    /// <param name="methodName">Name of the method.</param>
    /// <returns></returns>
    public static MethodInfo? GetMethodInfo<TClass>(string methodName)
    {
        return typeof(TClass).GetMethod(methodName);
    }

    /// <summary>
    /// Gets a list of <see cref="MethodInfo"/> for all the pubic methods in a class.
    /// </summary>
    /// <typeparam name="TClass">Class with method.</typeparam>
    /// <returns></returns>
    public static MethodInfo[] GetDeclaringTypePublicMethodsInfo<TClass>()
    {
        return typeof(TClass)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => x.DeclaringType == typeof(TClass))
            .ToArray();
    }
}
