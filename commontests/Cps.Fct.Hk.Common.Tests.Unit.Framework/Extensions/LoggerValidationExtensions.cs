// <copyright file="LoggerValidationExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.Tests.Unit.Framework.Extensions;

using Moq;

public static class LoggerValidationExtensions
{
    public static void VerifyLoggedWarning<T>(this Mock<ILogger<T>> logger, string messageContained)
    {
        logger.VerifyLogged(messageContained, LogLevel.Warning);
    }

    public static void VerifyLogged<T>(this Mock<ILogger<T>> logger, string messageContained, LogLevel logLevel)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        logger.Verify(
            l => l.Log(
                It.Is<LogLevel>(level => level == logLevel),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((@object, type) => @object.ToString().Contains(messageContained)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    public static void VerifyLoggedWithException<T>(this Mock<ILogger<T>> logger, string messageContained, LogLevel logLevel, Exception exception)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        logger.Verify(
            l => l.Log(
                It.Is<LogLevel>(level => level == logLevel),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((@object, type) => @object.ToString().Contains(messageContained)),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    public static void VerifyLoggedWithExceptionMessage<T>(this Mock<ILogger<T>> logger, string messageContained, LogLevel logLevel, string exceptionMessage)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        logger.Verify(
            l => l.Log(
                It.Is<LogLevel>(level => level == logLevel),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((@object, type) => @object.ToString().Contains(messageContained)),
                It.Is<Exception>(e => e.Message == exceptionMessage),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    public static void VerifyNoWarningsLogged<T>(this Mock<ILogger<T>> logger)
    {
        logger.Verify(
            l => l.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Never);
    }

    public static void VerifyNoCriticalsLogged<T>(this Mock<ILogger<T>> logger)
    {
        logger.Verify(
            l => l.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Critical),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Never);
    }

    public static void VerifyNothingLogged<T>(this Mock<ILogger<T>> logger)
    {
        logger.Verify(
            l => l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Never);
    }
}
