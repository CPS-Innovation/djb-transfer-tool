// <copyright file="ExceptionExtensionsTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Ui.ServiceClient.Ddei.Tests.Diagnostics;

using System;
using Xunit;
using Cps.Fct.Hk.Common.DDEI.Client.Diagnostics;

/// <summary>
/// Unit tests for <see cref="ExceptionExtensions"/>.
/// </summary>
public class ExceptionExtensionsTests
{
    /// <summary>
    /// Verifies that <see cref="ExceptionExtensions.ToAggregatedMessage"/> returns the correct aggregated message
    /// for a nested exception.
    /// </summary>
    [Fact]
    public void ToAggregatedMessage_ShouldReturnAggregatedMessage_ForNestedExceptions()
    {
        // Arrange
        var innerException = new InvalidOperationException("Inner exception message.");
        var outerException = new Exception("Outer exception message.", innerException);

        // Act
        string result = outerException.ToAggregatedMessage();

        // Assert
        // Note: The order of messages should be inner first, then outer.
        string expected = $"Inner exception message.{Environment.NewLine}Outer exception message.";
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Verifies that <see cref="ExceptionExtensions.ToAggregatedMessage"/> returns the correct message
    /// for a single exception.
    /// </summary>
    [Fact]
    public void ToAggregatedMessage_ShouldReturnMessage_ForSingleException()
    {
        // Arrange
        var exception = new Exception("Single exception message.");

        // Act
        string result = exception.ToAggregatedMessage();

        // Assert
        Assert.Equal("Single exception message.", result);
    }

    /// <summary>
    /// Verifies that <see cref="ExceptionExtensions.ToAggregatedMessage"/> returns an empty string for null exception.
    /// </summary>
    [Fact]
    public void ToAggregatedMessage_ShouldReturnEmptyString_ForNullException()
    {
        // Arrange
        Exception? exception = null;

        // Act
        string result = exception.ToAggregatedMessage();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    /// <summary>
    /// Verifies that <see cref="ExceptionExtensions.ToAggregatedMessage"/> handles an exception with null inner exception.
    /// </summary>
    [Fact]
    public void ToAggregatedMessage_ShouldHandleNullInnerException()
    {
        // Arrange
        var exception = new Exception("Exception with null inner exception.");

        // Act
        string result = exception.ToAggregatedMessage();

        // Assert
        Assert.Equal("Exception with null inner exception.", result);
    }
}
