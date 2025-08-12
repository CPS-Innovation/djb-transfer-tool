// <copyright file="ExampleProviderHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.Tests.Unit.Framework.Extensions;

using System.Reflection;
using System.Text;
using Shouldly;

public static class ExampleProviderHelper
{
    private const string PahNamespacePrefix = "PAH";
    private const string PahTestAssemblyName = "Tests";

    /// <summary>
    /// Checks all implementations of Generate() by inheritors of IExampleProvider
    /// result in all properties being instantiated.
    /// </summary>
    /// <returns>Whether the tests passed.</returns>
    public static bool EnsureAllPropertiesAreInstantiated()
    {
        return EnsureAllPropertiesAreInstantiated(new List<string>());
    }

    /// <summary>
    /// Checks all implementations of Generate() by inheritors of IExampleProvider
    /// result in all properties being instantiated.
    /// </summary>
    /// <param name="propertiesToIgnore">List of property names that do not need to be generated.</param>
    /// <returns>Whether the tests passed.</returns>
    public static bool EnsureAllPropertiesAreInstantiated(List<string> propertiesToIgnore)
    {
        var errors = new StringBuilder();

        foreach (var assembly in GetAllReferencedNonTestAssemblies())
        {
            foreach (var type in GetExampleProviderTypesFromAssembly(assembly))
            {
                var generateMethod = type
                    .GetMethods()
                    .Where(method => method.Name == nameof(IExampleProvider<object>.Generate))
                    .ToList();

                TestGenerateMethodFunctionality(generateMethod.First(), type, errors, propertiesToIgnore);
            }
        }

        errors.Length.ShouldBe(0, errors.ToString());

        return errors.Length == 0;
    }

    private static IEnumerable<Assembly> GetAllReferencedNonTestAssemblies()
    {
        return AssemblyTestHelper.GetPahAssemblies()
            .Where(x => x.FullName!.StartsWith(PahNamespacePrefix, StringComparison.InvariantCulture) &&
                        !x.FullName.Contains(PahTestAssemblyName, StringComparison.InvariantCulture));
    }

    private static List<Type> GetExampleProviderTypesFromAssembly(Assembly assembly)
    {
        return assembly
            .GetTypes()
            .Where(type => type
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IExampleProvider<>))
                .Select(i => i.GetGenericArguments().First())
                .Any())
            .ToList();
    }

    private static void TestGenerateMethodFunctionality(
        MethodBase generateMethod,
        Type type,
        StringBuilder errors,
        List<string> propertiesToIgnore)
    {
        if (type.GetConstructor(Type.EmptyTypes) == null)
        {
            return;
        }

        var instance = Activator.CreateInstance(type);
        var result = generateMethod.Invoke(instance, Array.Empty<object>());

        if (result == null)
        {
            return;
        }

        var errorsForType = AssertAllPropertiesGenerated(result, propertiesToIgnore);

        if (errorsForType.Length != 0)
        {
            errors
                .AppendLine("Some properties were not assigned by the ")
                .Append(nameof(IExampleProvider<object>.Generate))
                .Append(" method for type '")
                .Append(type.Name).Append('\'')
                .Append(type.Name)
                .AppendLine("':")
                .AppendLine(errorsForType);
        }
    }

    private static string AssertAllPropertiesGenerated(
        object instance,
        List<string> propertiesToIgnore)
    {
        var builder = new StringBuilder();
        AssertAllPropertiesGenerated(instance, builder, propertiesToIgnore);
        return builder.ToString();
    }

    private static void AssertAllPropertiesGenerated(
        object instance,
        StringBuilder builder,
        List<string> propertiesToIgnore)
    {
        var properties = instance
            .GetType()
            .GetProperties();

        foreach (var property in properties)
        {
            if (propertiesToIgnore.Contains(property.Name))
            {
                continue;
            }

            var value = property.GetValue(instance);

            if (value is null || (value is string s && string.IsNullOrEmpty(s)))
            {
                builder
                    .Append("\t- '")
                    .Append(property.Name)
                    .AppendLine("'");

                continue;
            }

            if (property.PropertyType.Namespace!.StartsWith(PahNamespacePrefix, StringComparison.InvariantCulture))
            {
                AssertAllPropertiesGenerated(value, builder, propertiesToIgnore);
            }
        }
    }
}
