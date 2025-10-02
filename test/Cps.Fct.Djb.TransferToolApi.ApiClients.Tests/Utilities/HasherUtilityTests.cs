// <copyright file="HasherUtilityTests.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Tests.Utilities;

using System;
using Cps.Fct.Djb.TransferToolApi.ApiClients.ConfigOptions;
using Cps.Fct.Djb.TransferToolApi.ApiClients.Utilities;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

/// <summary>
/// Unit tests for the <see cref="HasherUtility"/> class.
/// </summary>
public class HasherUtilityTests
{
    private readonly Mock<IOptionsMonitor<HashSettingsOptions>> optionsMock;
    private readonly HasherUtility hasher;

    public HasherUtilityTests()
    {
        // Default valid config for most tests
        var settings = new HashSettingsOptions
        {
            HashSecretKey = "SuperSmashingSecretKey",
            CaseIdHashPrefix = "case:"
        };

        optionsMock = new Mock<IOptionsMonitor<HashSettingsOptions>>();
        optionsMock.Setup(m => m.CurrentValue).Returns(settings);
        optionsMock.Setup(m => m.Get(It.IsAny<string>())).Returns(settings);

        hasher = new HasherUtility(optionsMock.Object);
    }

    /// <summary>
    /// Same caseId should always hash to the same value.
    /// </summary>
    [Fact]
    public void SameCaseId_ShouldAlwaysProduceSameHash()
    {
        string hash1 = hasher.HashAndPrefixCmsCaseId(42);
        string hash2 = hasher.HashAndPrefixCmsCaseId(42);

        Assert.Equal(hash1, hash2);
    }

    /// <summary>
    /// Different caseIds should not produce the same hash.
    /// </summary>
    [Fact]
    public void DifferentCaseIds_ShouldProduceDifferentHashes()
    {
        string hash1 = hasher.HashAndPrefixCmsCaseId(1);
        string hash2 = hasher.HashAndPrefixCmsCaseId(2);

        Assert.NotEqual(hash1, hash2);
    }

    /// <summary>
    /// Hash results should start with the configured prefix.
    /// </summary>
    [Fact]
    public void CaseIdHash_ShouldStartWithConfiguredPrefix()
    {
        string hash = hasher.HashAndPrefixCmsCaseId(12345);

        Assert.StartsWith("case:", hash);
        Assert.True(hash.Length > "case:".Length);
    }

    /// <summary>
    /// Constructor should throw if options are null.
    /// </summary>
    [Fact]
    public void Constructor_WithNullOptions_ShouldThrow()
    {
        var mock = new Mock<IOptionsMonitor<HashSettingsOptions>>();
        mock.Setup(m => m.CurrentValue).Returns((HashSettingsOptions)null!);

        Assert.Throws<InvalidOperationException>(() => new HasherUtility(mock.Object));
    }

    /// <summary>
    /// Should throw if secret key is missing in options.
    /// </summary>
    [Fact]
    public void CaseIdHash_WithMissingSecretKey_ShouldThrow()
    {
        optionsMock.Setup(m => m.CurrentValue).Returns(new HashSettingsOptions
        {
            HashSecretKey = "",
            CaseIdHashPrefix = "case:"
        });

        var localHasher = new HasherUtility(optionsMock.Object);

        Assert.Throws<ArgumentException>(() => localHasher.HashAndPrefixCmsCaseId(123));
    }

    /// <summary>
    /// Should throw if prefix is missing in options.
    /// </summary>
    [Fact]
    public void CaseIdHash_WithMissingPrefix_ShouldThrow()
    {
        optionsMock.Setup(m => m.CurrentValue).Returns(new HashSettingsOptions
        {
            HashSecretKey = "SuperSmashingSecretKey",
            CaseIdHashPrefix = ""
        });

        var localHasher = new HasherUtility(optionsMock.Object);

        Assert.Throws<ArgumentException>(() => localHasher.HashAndPrefixCmsCaseId(123));
    }

    /// <summary>
    /// Should throw if the caseId provided is default (0).
    /// </summary>
    [Fact]
    public void CaseIdHash_WithDefaultCaseId_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() => hasher.HashAndPrefixCmsCaseId(default));
    }

    /// <summary>
    /// Known input should produce a fixed expected hash (golden value test).
    /// </summary>
    [Fact]
    public void CaseIdHash_WithKnownInput_ShouldMatchExpectedHash()
    {
        // Arrange
        int caseId = 12345;
        string expectedHash = "case:CA4ADDA67AF8671CCA39DE988B648F346CD3E659DFCA5289F3264A5F071539AC";

        // Act
        string actualHash = hasher.HashAndPrefixCmsCaseId(caseId);

        // Assert
        Assert.Equal(expectedHash, actualHash);
    }
}
