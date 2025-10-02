// <copyright file="CmsAreaToCaseCenterDataMappingViaOptionsResolverTests.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Tests.Resolvers;

using System;
using Cps.Fct.Djb.TransferToolApi.ApiClients.ConfigOptions;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Resolvers;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

/// <summary>
/// Unit tests for the <see cref="CmsAreaToCaseCenterDataMappingViaOptionsResolver"/> class.
/// </summary>
public class CmsAreaToCaseCenterDataMappingViaOptionsResolverTests
{
    private readonly CmsAreaToCaseCenterDataMappingsOptions mappings;
    private readonly Mock<IOptionsMonitor<CmsAreaToCaseCenterDataMappingsOptions>> optionsMock;
    private readonly CmsAreaToCaseCenterDataMappingViaOptionsResolver resolver;

    public CmsAreaToCaseCenterDataMappingViaOptionsResolverTests()
    {
        // Arrange default test mappings (using made-up IDs)
        mappings = new CmsAreaToCaseCenterDataMappingsOptions
        {
            ["Swansea_CJU"] = new CmsAreaMapping
            {
                TemplateId = "template-swansea-123",
                DepartmentId = "dept-swansea-456"
            },
            ["Westminster_CJU"] = new CmsAreaMapping
            {
                TemplateId = "template-westminster-789",
                DepartmentId = "dept-westminster-012"
            }
        };

        optionsMock = new Mock<IOptionsMonitor<CmsAreaToCaseCenterDataMappingsOptions>>();
        optionsMock.Setup(m => m.CurrentValue).Returns(mappings);
        optionsMock.Setup(m => m.Get(It.IsAny<string>())).Returns(mappings);

        resolver = new CmsAreaToCaseCenterDataMappingViaOptionsResolver(optionsMock.Object);
    }

    /// <summary>
    /// Should resolve a known area to its configured mapping.
    /// </summary>
    [Fact]
    public void Resolve_WithKnownArea_ShouldReturnMapping()
    {
        var result = resolver.Resolve("Swansea_CJU");

        Assert.NotNull(result);
        Assert.Equal("template-swansea-123", result.TemplateId);
        Assert.Equal("dept-swansea-456", result.DepartmentId);
    }

    /// <summary>
    /// Should resolve a known area regardless of case sensitivity.
    /// </summary>
    [Fact]
    public void Resolve_WithDifferentCase_ShouldReturnMapping()
    {
        var result = resolver.Resolve("swansea_cju");

        Assert.NotNull(result);
        Assert.Equal("template-swansea-123", result.TemplateId);
        Assert.Equal("dept-swansea-456", result.DepartmentId);
    }

    /// <summary>
    /// Should return null if area name is not in configuration.
    /// </summary>
    [Fact]
    public void Resolve_WithUnknownArea_ShouldReturnNull()
    {
        var result = resolver.Resolve("Leeds_CJU");

        Assert.Null(result);
    }

    /// <summary>
    /// Should return null if area name is null or whitespace.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Resolve_WithNullOrWhitespace_ShouldReturnNull(string areaName)
    {
        var result = resolver.Resolve(areaName);

        Assert.Null(result);
    }

    /// <summary>
    /// Constructor should throw if options are null.
    /// </summary>
    [Fact]
    public void Constructor_WithNullOptions_ShouldThrow()
    {
        var mock = new Mock<IOptionsMonitor<CmsAreaToCaseCenterDataMappingsOptions>>();
        mock.Setup(m => m.CurrentValue).Returns((CmsAreaToCaseCenterDataMappingsOptions)null!);

        Assert.Throws<InvalidOperationException>(() => new CmsAreaToCaseCenterDataMappingViaOptionsResolver(mock.Object));
    }
}
