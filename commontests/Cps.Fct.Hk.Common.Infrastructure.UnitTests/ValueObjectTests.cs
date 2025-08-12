namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests;

public class ValueObjectTests
{
    [Fact]
    public void ValueObject_Equals_DifferentType()
    {
        // Arrange
        var test = new ValueObjectConcrete();

        // Act
        var result = test.Equals(VersionInformation.Default());

        // Assert
        result.ShouldBeFalse();
    }

    [Theory]
    [InlineData("test", 10, "test", 10, true)]
    [InlineData("", 0, "", 0, true)]
    [InlineData("test", 10, "other", 10, false)]
    [InlineData("test", 8, "other", 10, false)]
    [InlineData("test", 8, "test", 10, false)]
    public void ValueObject_Equals_Checks(string string1, int int1, string string2, int int2, bool expectedResult)
    {
        // Arrange
        var initialObject = new ValueObjectConcrete(string1, int1);
        var comparedObject = new ValueObjectConcrete(string2, int2);

        // Act
        var result = initialObject.Equals(comparedObject);

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("test", 10, "test", 10, true)]
    [InlineData("", 0, "", 0, true)]
    [InlineData("test", 10, "other", 10, false)]
    [InlineData("test", 8, "other", 10, false)]
    [InlineData("test", 8, "test", 10, false)]
    public void ValueObject_GetHashCode_Checks(string string1, int int1, string string2, int int2, bool shouldBeEqual)
    {
        // Arrange
        var initialObject = new ValueObjectConcrete(string1, int1);
        var comparedObject = new ValueObjectConcrete(string2, int2);

        // Act
        var hashCodeInitial = initialObject.GetHashCode();
        var hashCodeCompared = comparedObject.GetHashCode();
        var result = hashCodeCompared == hashCodeInitial;

        // Assert
        result.ShouldBe(shouldBeEqual);
    }

    [Theory]
    [InlineData("test", 10, "test", 10, false)]
    [InlineData("", 0, "", 0, false)]
    [InlineData("test", 10, "other", 10, false)]
    [InlineData("test", 8, "other", 10, false)]
    [InlineData("test", 8, "test", 10, false)]
    public void ValueObject_EqualOperator_Checks(string string1, int int1, string string2, int int2, bool shouldBeEqual)
    {
        // Arrange
        var initialObject = new ValueObjectConcrete(string1, int1);
        var comparedObject = new ValueObjectConcrete(string2, int2);

        // Act
        var result = initialObject == comparedObject;

        // Assert
        result.ShouldBe(shouldBeEqual);
    }

    [Theory]
    [InlineData(false, false, false)]
    [InlineData(false, true, false)]
    [InlineData(true, false, false)]
    [InlineData(true, true, true)]
    public void ValueObject_NullChecks(bool item1Null, bool item2Null, bool shouldBeEqual)
    {
        // Arrange
        ValueObjectConcrete? item1 = null;
        ValueObjectConcrete? item2 = null;

        if (!item1Null)
        {
            item1 = new ValueObjectConcrete("test", 5);
        }

        if (!item2Null)
        {
            item2 = new ValueObjectConcrete("test", 5);
        }

        // Act
        var result = item1 == item2;

        // Assert
        result.ShouldBe(shouldBeEqual);
    }

    [Theory]
    [InlineData("test", 10, "test", 10, true)]
    [InlineData("", 0, "", 0, true)]
    [InlineData("test", 10, "other", 10, true)]
    [InlineData("test", 8, "other", 10, true)]
    [InlineData("test", 8, "test", 10, true)]
    public void ValueObject_NotEqualOperator_Checks(string string1, int int1, string string2, int int2, bool shouldBeEqual)
    {
        // Arrange
        var initialObject = new ValueObjectConcrete(string1, int1);
        var comparedObject = new ValueObjectConcrete(string2, int2);

        // Act
        var result = initialObject != comparedObject;

        // Assert
        result.ShouldBe(shouldBeEqual);
    }
}
