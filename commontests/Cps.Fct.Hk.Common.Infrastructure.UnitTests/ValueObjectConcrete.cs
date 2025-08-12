using Cps.Fct.Hk.Common.Contracts;

namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests;

public class ValueObjectConcrete : ValueObject
{
    public ValueObjectConcrete()
    {
    }

    public ValueObjectConcrete(string testString, int testInt)
    {
        TestString = testString;
        TestInt = testInt;
    }

    public string TestString { get; set; } = string.Empty;

    public int TestInt { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return TestString;

        yield return TestInt;
    }
}
