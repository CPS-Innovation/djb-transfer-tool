// <copyright file="ShouldlyAssertionExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.Tests.Unit.Framework.Extensions;

using Shouldly;

[ShouldlyMethods]
public static class ShouldlyAssertionExtensions
{
    public static void ShouldBeAssignableFrom(this Type type, Type fromType)
    {
        type.IsAssignableFrom(fromType).ShouldBeTrue();
    }

    public static void ShouldThrowWithMessage<T, TException>(this Func<T> act, string message)
        where TException : Exception
    {
        var exception = Should.Throw<TException>(() => act());
        exception.Message.ShouldBe(message);
    }

    public static void ShouldThrowAsyncWithMessage<TException>(this Func<Task> act, string message)
        where TException : Exception
    {
        var exception = Should.Throw<TException>(async () => await act().ConfigureAwait(false));
        exception.Message.ShouldBe(message);
    }

    public static void ShouldThrowAsyncWithException<TException>(this Func<Task> act, TException exception)
        where TException : Exception
    {
        var thrownException = Should.Throw<TException>(async () => await act().ConfigureAwait(false));
        thrownException.ShouldBe(exception);
    }
}
