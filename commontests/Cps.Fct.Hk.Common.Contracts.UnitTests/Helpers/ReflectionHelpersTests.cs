namespace Cps.Fct.Hk.Common.Contracts.UnitTests.Helpers;

public class ReflectionHelpersTests
{
    [Fact]
    public void GetClassAttributesGenericClassName_GivenClassWithNoTestAttribute_ReturnsEmptyArray()
    {
        // Arrange & Act
        var result = ReflectionHelpers.GetClassAttributes<ClassWithNoTestAttributeDeclared, TestAttributeAttribute>();

        // Assert
        result.ShouldBeEmpty();
    }

    [Fact]
    public void GetClassAttributesGenericClassName_GivenClassWithTestAttribute_ReturnsCorrectAttributeTypes()
    {
        // Arrange & Act
        var result = ReflectionHelpers.GetClassAttributes<ClassWithTestAttributeDeclared, TestAttributeAttribute>();

        // Assert
        result.ShouldHaveSingleItem();
        result[0].ShouldBeOfType<TestAttributeAttribute>();
    }

    [Fact]
    public void GetClassAttributesGenericClassName_GivenClassWithMultipleTestAttributes_ReturnsCorrectAttributeTypes()
    {
        // Arrange & Act
        var result = ReflectionHelpers.GetClassAttributes<ClassWithTestMultipleAttributesDeclared, TestAttributeAttribute>();

        // Assert
        result.Length.ShouldBe(2);
        result[0].ShouldBeOfType<TestAttributeAttribute>();
        result[1].ShouldBeOfType<TestAttributeAttribute>();
    }

    [Fact]
    public void GetClassAttributeGenericClassName_GivenClassWithMultipleAttributeTypes_ReturnsCorrectAttribute()
    {
        // Arrange & Act
        var result = ReflectionHelpers.GetClassAttributes<ClassWithMultipleTestAttributesDeclared, TestAttributeAttribute>();

        // Assert
        result.ShouldHaveSingleItem();
        result[0].ShouldBeOfType<TestAttributeAttribute>();
    }

    [Fact]
    public void GetClassAttributes_GivenClassWithNoTestAttribute_ReturnsEmptyArray()
    {
        // Arrange & Act
        var result = ReflectionHelpers.GetClassAttributes<TestAttributeAttribute>(typeof(ClassWithNoTestAttributeDeclared));

        // Assert
        result.ShouldBeEmpty();
    }

    [Fact]
    public void GetClassAttributes_GivenClassWithTestAttribute_ReturnsCorrectAttributeTypes()
    {
        // Arrange & Act
        var result = ReflectionHelpers.GetClassAttributes<TestAttributeAttribute>(typeof(ClassWithTestAttributeDeclared));

        // Assert
        result.ShouldHaveSingleItem();
        result[0].ShouldBeOfType<TestAttributeAttribute>();
    }

    [Fact]
    public void GetClassAttributes_GivenClassWithMultipleTestAttributes_ReturnsCorrectAttributeTypes()
    {
        // Arrange & Act
        var result = ReflectionHelpers.GetClassAttributes<TestAttributeAttribute>(typeof(ClassWithTestMultipleAttributesDeclared));

        // Assert
        result.Length.ShouldBe(2);
        result[0].ShouldBeOfType<TestAttributeAttribute>();
        result[1].ShouldBeOfType<TestAttributeAttribute>();
    }

    [Fact]
    public void GetClassAttribute_GivenClassWithMultipleAttributeTypes_ReturnsCorrectAttribute()
    {
        // Arrange & Act
        var result = ReflectionHelpers.GetClassAttributes<TestAttributeAttribute>(typeof(ClassWithMultipleTestAttributesDeclared));

        // Assert
        result.ShouldHaveSingleItem();
        result[0].ShouldBeOfType<TestAttributeAttribute>();
    }

    [Fact]
    public void GetMethodAttributesGenericClassName_GivenMethodWithNoTestAttribute_ReturnsEmptyArray()
    {
        // Arrange & Act
        var result = ReflectionHelpers.GetMethodAttributes<ClassWithNoTestAttributeDeclared, TestAttributeAttribute>(
            nameof(ClassWithNoTestAttributeDeclared.MethodWithNoAttributesDeclared));

        // Assert
        result.ShouldBeEmpty();
    }

    [Fact]
    public void GetMethodAttributesGenericClassName_GivenMethodWithTestAttribute_ReturnsCorrectAttributeTypes()
    {
        // Arrange & Act
        var result = ReflectionHelpers.GetMethodAttributes<ClassWithNoTestAttributeDeclared, TestAttributeAttribute>(
            nameof(ClassWithNoTestAttributeDeclared.MethodWithTestAttributeDeclared));

        // Assert
        result.ShouldHaveSingleItem();
        result[0].ShouldBeOfType<TestAttributeAttribute>();
    }

    [Fact]
    public void GetMethodAttributesGenericClassName_GivenMethodWithMultipleTestAttributes_ReturnsCorrectAttributeTypes()
    {
        // Arrange & Act
        var result = ReflectionHelpers.GetMethodAttributes<ClassWithNoTestAttributeDeclared, TestAttributeAttribute>(
            nameof(ClassWithNoTestAttributeDeclared.MethodWithMultipleTestAttributesDeclared));

        // Assert
        result.Length.ShouldBe(2);
        result[0].ShouldBeOfType<TestAttributeAttribute>();
        result[1].ShouldBeOfType<TestAttributeAttribute>();
    }

    [Fact]
    public void GetClassAttributeGenericClassName_GivenMethodWithMultipleAttributeTypes_ReturnsCorrectAttributes()
    {
        // Arrange & Act
        var result = ReflectionHelpers.GetMethodAttributes<ClassWithNoTestAttributeDeclared, TestAttributeAttribute>(
            nameof(ClassWithNoTestAttributeDeclared.MethodWithOtherTestAttributeAndTestAttributeDeclared));

        // Assert
        result.ShouldHaveSingleItem();
        result[0].ShouldBeOfType<TestAttributeAttribute>();
    }

    [Fact]
    public void GetMethodAttributes_GivenMethodWithNoTestAttribute_ReturnsEmptyArray()
    {
        // Arrange & Act
        var result = ReflectionHelpers.GetMethodAttributes<ClassWithNoTestAttributeDeclared>(
            nameof(ClassWithNoTestAttributeDeclared.MethodWithNoAttributesDeclared),
            typeof(TestAttributeAttribute));

        // Assert
        result.ShouldBeEmpty();
    }

    [Fact]
    public void GetMethodAttributes_GivenMethodWithTestAttribute_ReturnsCorrectAttributeTypes()
    {
        // Arrange & Act
        var result = ReflectionHelpers.GetMethodAttributes<ClassWithNoTestAttributeDeclared>(
            nameof(ClassWithNoTestAttributeDeclared.MethodWithTestAttributeDeclared),
            typeof(TestAttributeAttribute));

        // Assert
        result.ShouldHaveSingleItem();
        result[0].ShouldBeOfType<TestAttributeAttribute>();
    }

    [Fact]
    public void GetMethodAttributes_GivenMethodWithMultipleTestAttributes_ReturnsCorrectAttributeTypes()
    {
        // Arrange & Act
        var result = ReflectionHelpers.GetMethodAttributes<ClassWithNoTestAttributeDeclared>(
            nameof(ClassWithNoTestAttributeDeclared.MethodWithMultipleTestAttributesDeclared),
            typeof(TestAttributeAttribute));

        // Assert
        result.Length.ShouldBe(2);
        result[0].ShouldBeOfType<TestAttributeAttribute>();
        result[1].ShouldBeOfType<TestAttributeAttribute>();
    }

    [Fact]
    public void GetClassAttribute_GivenMethodWithMultipleAttributeTypes_ReturnsCorrectAttributes()
    {
        // Arrange & Act
        var result = ReflectionHelpers.GetMethodAttributes<ClassWithNoTestAttributeDeclared>(
            nameof(ClassWithNoTestAttributeDeclared.MethodWithOtherTestAttributeAndTestAttributeDeclared),
            typeof(TestAttributeAttribute));

        // Assert
        result.ShouldHaveSingleItem();
        result[0].ShouldBeOfType<TestAttributeAttribute>();
    }

    [Fact]
    public void GetDeclaringTypePublicMethodsInfo_GivenMethodWithoutTestAttribute_ReturnsEmptyArray()
    {
        // Arrange & Act
        var result = ReflectionHelpers.GetDeclaringTypePublicMethodsInfo<ClassWithTestAttributeDeclared>();

        // Assert
        result.ShouldBeEmpty();
    }

    [Theory]
    [InlineData(nameof(ClassWithNoTestAttributeDeclared.MethodWithNoAttributesDeclared))]
    [InlineData(nameof(ClassWithNoTestAttributeDeclared.MethodWithTestAttributeDeclared))]
    [InlineData(nameof(ClassWithNoTestAttributeDeclared.MethodWithMultipleTestAttributesDeclared))]
    public void GetMethodInfo_ReturnsExpectedResult(string methodName)
    {
        ReflectionHelpers.GetMethodInfo<ClassWithTestAttributeDeclared>(methodName)
            .ShouldBeEquivalentTo(typeof(ClassWithTestAttributeDeclared).GetMethod(methodName));
    }

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    private sealed class TestAttributeAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    private sealed class OtherTestAttributeAttribute : Attribute
    {
    }

    [TestAttribute]
    [OtherTestAttribute]
    private class ClassWithMultipleTestAttributesDeclared
    {
    }

    private class ClassWithNoTestAttributeDeclared
    {
        public static void MethodWithNoAttributesDeclared()
        {
            // Empty test method
        }

        [TestAttribute]
        public static void MethodWithTestAttributeDeclared()
        {
            // Empty test method
        }

        [TestAttribute]
        [TestAttribute]
        public static void MethodWithMultipleTestAttributesDeclared()
        {
            // Empty test method
        }

        [TestAttribute]
        [OtherTestAttribute]
        public static void MethodWithOtherTestAttributeAndTestAttributeDeclared()
        {
            // Empty test method
        }
    }

    [TestAttribute]
    private class ClassWithTestAttributeDeclared
    {
    }

    [TestAttribute]
    [TestAttribute]
    private class ClassWithTestMultipleAttributesDeclared
    {
    }
}
