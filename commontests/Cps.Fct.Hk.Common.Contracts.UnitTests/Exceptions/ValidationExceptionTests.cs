using FluentValidation.Results;
using ValidationException = Cps.Fct.Hk.Common.Contracts.Exceptions.ValidationException;

namespace Cps.Fct.Hk.Common.Contracts.UnitTests.Exceptions;

public class ValidationExceptionTests
{
    [Fact]
    public void ValidationException_Construct_NoParameters()
    {
        // Act
        var result = new ValidationException();

        // Assert
        result.Message.ShouldBe("One or more validation failures have occurred.");
        result.Errors.ShouldNotBeNull();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public void ValidationException_Construct_WithNoFailures()
    {
        // Arrange & Act
        var result = new ValidationException(new List<ValidationFailure>());

        // Assert
        result.Message.ShouldBe("One or more validation failures have occurred.");
        result.Errors.ShouldNotBeNull();
        result.Errors.ShouldBeEmpty();
    }

    [Fact]
    public void ValidationException_Construct_WithSingleFailures()
    {
        // Arrange
        var failures = new List<ValidationFailure>
        {
            new("Property1", "Failure1")
        };

        // Act
        var result = new ValidationException(failures);

        // Assert
        result.Message.ShouldBe("One or more validation failures have occurred.");
        result.Errors.ShouldNotBeNull();
        result.Errors.ShouldHaveSingleItem();
        result.Errors.ShouldContainKey("Property1");

        var hasValue = result.Errors.TryGetValue("Property1", out var foundValue);
        hasValue.ShouldBeTrue();
        foundValue.ShouldHaveSingleItem();
        foundValue[0].ShouldBe("Failure1");
    }

    [Fact]
    public void ValidationException_Construct_WithGroupedFailures()
    {
        // Arrange
        var failures = new List<ValidationFailure>
        {
            new("Property1", "Failure1"),
            new("Property1", "Failure1"),
            new("Property2", "Failure1"),
            new("Property2", "Failure2"),
            new("Property2", "Failure3"),
            new("Property2", "Failure3"),
            new("Property3", "Failure4")
        };

        // Act
        var result = new ValidationException(failures);

        // Assert
        result.Message.ShouldBe("One or more validation failures have occurred.");
        result.Errors.ShouldNotBeNull();
        result.Errors.Count.ShouldBe(3);

        var hasValue = result.Errors.TryGetValue("Property1", out var foundValue);
        hasValue.ShouldBeTrue();
        foundValue!.Length.ShouldBe(2);
        foundValue.ShouldContain(x => x == "Failure1", 2);

        hasValue = result.Errors.TryGetValue("Property2", out foundValue);
        hasValue.ShouldBeTrue();
        foundValue!.Length.ShouldBe(4);
        foundValue.ShouldContain("Failure1");
        foundValue.ShouldContain("Failure2");
        foundValue.ShouldContain(x => x == "Failure3", 2);

        hasValue = result.Errors.TryGetValue("Property3", out foundValue);
        hasValue.ShouldBeTrue();
        foundValue.ShouldHaveSingleItem();
        foundValue[0].ShouldBe("Failure4");
    }
}
