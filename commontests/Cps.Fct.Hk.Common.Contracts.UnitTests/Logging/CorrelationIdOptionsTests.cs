using Cps.Fct.Hk.Common.Contracts.Logging;

namespace Cps.Fct.Hk.Common.Contracts.UnitTests.Logging;

public class CorrelationIdOptionsTests
{
    [Fact]
    public void CorrelationIdOptions_SectionName_HasDefaultValue()
    {
        // Arrange & Act
        var options = new CorrelationIdOptions();

        // Assert
        options.SectionName.ShouldBe(string.Empty);
    }

    [Fact]
    public void CorrelationIdOptions_Properties_HaveDefaultValues()
    {
        // Arrange & Act
        var options = new CorrelationIdOptions();

        // Assert
        options.Header.ShouldBe("correlationid");
        options.IncludeInResponse.ShouldBeTrue();
    }

    [Fact]
    public void CorrelationIdOptions_Properties_AreSet()
    {
        // Arrange & Act
        var options = new CorrelationIdOptions
        {
            Header = "x-pcp-abc",
            IncludeInResponse = false
        };

        // Assert
        options.Header.ShouldBe("x-pcp-abc");
        options.IncludeInResponse.ShouldBeFalse();
    }
}
