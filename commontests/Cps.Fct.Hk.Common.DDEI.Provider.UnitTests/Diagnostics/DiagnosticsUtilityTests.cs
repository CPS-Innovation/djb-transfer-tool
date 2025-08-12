// <copyright file="DiagnosticsUtilityTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Ui.ServiceClient.Ddei.Tests.Diagnostics;

using Xunit;
using Cps.Fct.Hk.Common.DDEI.Client.Diagnostics;

/// <summary>
/// Unit tests for <see cref="DiagnosticsUtility"/>.
/// </summary>
public class DiagnosticsUtilityTests
{
    /// <summary>
    /// Verifies that the <see cref="DiagnosticsUtility.Warning"/> constant is correctly defined.
    /// </summary>
    [Fact]
    public void WarningConstant_ShouldBeCorrect()
    {
        // Arrange
        const string expectedValue = "Warning: ";

        // Act
        string actualValue = DiagnosticsUtility.Warning;

        // Assert
        Assert.Equal(expectedValue, actualValue);
    }

    /// <summary>
    /// Verifies that the <see cref="DiagnosticsUtility.Error"/> constant is correctly defined.
    /// </summary>
    [Fact]
    public void ErrorConstant_ShouldBeCorrect()
    {
        // Arrange
        const string expectedValue = "Error: ";

        // Act
        string actualValue = DiagnosticsUtility.Error;

        // Assert
        Assert.Equal(expectedValue, actualValue);
    }
}
