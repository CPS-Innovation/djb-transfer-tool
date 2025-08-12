// <copyright file="TestAssemblyProvider.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.Tests.Unit.Framework.Providers;

/// <inheritdoc cref="IAssemblyProvider" />
public class TestAssemblyProvider : IAssemblyProvider
{
    private readonly Type assemblyClassType;

    public TestAssemblyProvider(Type assemblyClassType)
    {
        this.assemblyClassType = assemblyClassType;
    }

    /// <summary>
    /// Retrieves a Startup Assembly details.
    /// </summary>
    /// <returns>A <see cref="Assembly"/> details for Startup.</returns>
    /// <exception cref="InvalidOperationException">throws an invalid operation if assembly is not retrieved.</exception>
    public Assembly GetEntryAssembly()
    {
        return Assembly.GetAssembly(assemblyClassType)
            ?? throw new InvalidOperationException(
                $"Could not retrieve Entry Assembly for type {assemblyClassType.FullName}");
    }
}
