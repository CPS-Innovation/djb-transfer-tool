// <copyright file="AssemblyTestHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.Tests.Unit.Framework.Extensions;

using System.Reflection;

/// <summary>
/// AssemblyTestHelper.
/// </summary>
public class AssemblyTestHelper
{
    public static IEnumerable<Assembly> GetPahAssemblies()
    {
        // This was updated due to Resharper reflection returning fewer assemblies. For more information,
        // please look at this https://youtrack.jetbrains.com/issue/RSRP-483799
        // See also https://dotnetcoretutorials.com/2020/07/03/getting-assemblies-is-harder-than-you-think-in-c/
        var returnAssemblies = new List<Assembly>();
        var loadedAssemblyNames = new HashSet<string>();

        var assembliesToCheck = new Queue<Assembly>(FilterForPahAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

        while (assembliesToCheck.Any())
        {
            var assemblyToCheck = assembliesToCheck.Dequeue();

            foreach (var reference in assemblyToCheck.GetReferencedAssemblies())
            {
                if (loadedAssemblyNames.Add(reference.FullName))
                {
                    var assembly = Assembly.Load(reference);
                    assembliesToCheck.Enqueue(assembly);
                    returnAssemblies.Add(assembly);
                }
            }

            if (loadedAssemblyNames.Add(assemblyToCheck.FullName!))
            {
                returnAssemblies.Add(assemblyToCheck);
            }
        }

        return FilterForPahAssemblies(returnAssemblies);
    }

    private static IEnumerable<Assembly> FilterForPahAssemblies(IEnumerable<Assembly> assemblies)
    {
        return assemblies
            .Where(assembly => AssembliesExtensions.AllowedAssemblyStartNames.Any(name =>
                assembly.FullName!.StartsWith(name, StringComparison.InvariantCultureIgnoreCase)));
    }
}
