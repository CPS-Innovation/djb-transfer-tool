// <copyright file="ApplicationOptionsTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Ui.ServiceClient.Ddei.Tests.Configuration.Extensions;

using Xunit;
using FluentAssertions;
using System.Text.Json.Serialization;
using Cps.Fct.Hk.Common.DDEI.Client.Configuration.Extensions;

/// <summary>
/// Unit tests for the <see cref="ApplicationOptions"/> class.
/// </summary>
public class ApplicationOptionsTests
{
    /// <summary>
    /// Tests the default constructor.
    /// </summary>
    [Fact]
    public void DefaultConstructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var options = new ApplicationOptions();

        // Assert
        options.AppName.Should().BeEmpty();
        options.AppDescription.Should().BeEmpty();
    }

    /// <summary>
    /// Tests the parameterized constructor.
    /// </summary>
    [Fact]
    public void ParameterizedConstructor_ShouldInitializeWithGivenValues()
    {
        // Arrange
        string appName = "Test App Name";
        string appDescription = "Test App Description";

        // Act
        var options = new ApplicationOptions(appName, appDescription);

        // Assert
        options.AppName.Should().Be(appName);
        options.AppDescription.Should().Be(appDescription);
    }

    /// <summary>
    /// Tests that the default section name is correct.
    /// </summary>
    [Fact]
    public void DefaultSectionName_ShouldBeCorrect()
    {
        // Assert
        ApplicationOptions.DefaultSectionName.Should().Be("Application");
    }

    /// <summary>
    /// Tests that the default JsonSerializerOptions are configured correctly.
    /// </summary>
    [Fact]
    public void ApplicationSerializerOptions_ShouldBeConfiguredCorrectly()
    {
        // Act
        System.Text.Json.JsonSerializerOptions options = ApplicationOptions.ApplicationSerializerOptions;

        // Assert
        options.DefaultIgnoreCondition.Should().Be(JsonIgnoreCondition.WhenWritingNull);
        options.Converters.Should().ContainSingle(converter => converter is JsonStringEnumConverter);
    }
}
