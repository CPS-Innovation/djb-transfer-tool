// <copyright file="MapperTestHelpers.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.Tests.Unit.Framework.Extensions;

public static class MapperTestHelpers
{
    public static void TestMapToNewObject<TSource, TDestination>(
        this IModelMapper<TSource, TDestination> mapper,
        Action<TSource, TDestination> assert,
        IEnumerable<string>? ignoreDestinationProperties = null,
        IEnumerable<string>? ignoreSourceProperties = null)
        where TSource : IExampleProvider<TSource>, new()
    {
        // Arrange
        var original = new TSource().Generate();
        var source = CreateCopy(original);

        // Act
        var result = mapper.Map(source);

        // Assert
        assert.Invoke(original, result);
        TestMappings<TSource, TDestination>(ignoreDestinationProperties, ignoreSourceProperties);
    }

    public static void TestMapToExistingObject<TSource, TDestination>(
        this IExistingModelMapper<TSource, TDestination> mapper,
        TDestination destination,
        Action<TSource, TDestination> assert)
        where TSource : IExampleProvider<TSource>, new()
    {
        // Arrange
        var original = new TSource().Generate();
        var source = CreateCopy(original);

        // Act
        var result = mapper.Map(source, destination);

        // Assert
        assert.Invoke(original, result);
    }

    public static bool TestReverseMapToNewObject<TSource, TDestination>(
        this IModelMapper<TSource, TDestination> mapper,
        IModelMapper<TDestination, TSource> reverseMapper,
        Action<TSource, TSource>? preEquivalenceAction = null,
        IEnumerable<string>? ignoreDestinationProperties = null,
        IEnumerable<string>? ignoreSourceProperties = null,
        IEnumerable<string>? ignoreReverseDestinationProperties = null,
        IEnumerable<string>? ignoreReverseSourceProperties = null)
        where TSource : IExampleProvider<TSource>, new()
    {
        // Arrange
        var original = new TSource().Generate();
        var source = CreateCopy(original);

        // Act
        var dto = mapper.Map(source);
        var bo = reverseMapper.Map(dto);

        // Assert
        preEquivalenceAction?.Invoke(original, bo);
        bo.ShouldBeEquivalentTo(original);
        TestMappings<TSource, TDestination>(ignoreDestinationProperties, ignoreSourceProperties);
        TestMappings<TDestination, TSource>(ignoreReverseDestinationProperties, ignoreReverseSourceProperties);
        return true;
    }

    public static T CreateCopy<T>(T original)
    {
        var json = JsonSerializer.Serialize(original) ?? throw new Exception("Cant parse json");
        var source = JsonSerializer.Deserialize<T>(json);

        source.ShouldBeEquivalentTo(original);
        return source!;
    }

    public static void TestMappings<TSource, TDestination>(
        IEnumerable<string>? ignoreDestinationProperties = null,
        IEnumerable<string>? ignoreSourceProperties = null)
    {
        // Arrange
        var source = typeof(TSource).GetProperties().Select(x => x.Name).ToArray();
        var destination = typeof(TDestination).GetProperties().Select(x => x.Name).ToArray();
        ignoreDestinationProperties ??= Enumerable.Empty<string>();
        ignoreSourceProperties ??= Enumerable.Empty<string>();
        var builder = new StringBuilder();

        // Act
        var commonProperties = source.Intersect(destination).ToArray();

        // Testing that any new properties added to the Destination have been mapped from Source
        AssertUnmappedProperties<TSource, TDestination>(
            builder,
            destination,
            commonProperties,
            ignoreDestinationProperties);

        // Testing that any new properties added to the Source have been mapped to Destination
        AssertUnmappedProperties<TDestination, TSource>(
            builder,
            source,
            commonProperties,
            ignoreSourceProperties);

        // Assert
        if (builder.Length != 0)
        {
            var exceptionBuilder = new StringBuilder();
            exceptionBuilder
                .AppendLine("Did you forget to map?")
                .AppendLine("In mapping:")
                .Append("from: ").AppendLine(typeof(TSource).FullName)
                .Append("to  : ").AppendLine(typeof(TDestination).FullName)
                .AppendLine("the following properties were found unmapped or configured to be ignored in this running test")
                .Append(builder);

            throw new Exception(exceptionBuilder.ToString());
        }
    }

    private static void AssertUnmappedProperties<TSource, TDestination>(
        StringBuilder builder,
        string[] destination,
        string[] commonProperties,
        IEnumerable<string> ignoreProperties)
    {
        var allCommonProperties = commonProperties.Union(ignoreProperties).ToArray();

        var unmappedProperties = destination
            .Except(allCommonProperties)
            .OrderBy(x => x)
            .ToArray();

        if (unmappedProperties.Any())
        {
            builder
                .AppendLine()
                .Append(typeof(TSource).Name).AppendLine(" is missing")
                .AppendLine("=============================================");

            foreach (var property in unmappedProperties)
            {
                builder.Append("nameof(").Append(typeof(TDestination).Name).Append('.').Append(property).AppendLine("),");
            }

            builder.AppendLine();
        }
    }
}
