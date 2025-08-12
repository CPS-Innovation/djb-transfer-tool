using Cps.Fct.Hk.Common.Contracts;

namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests;

public class DefaultsTests
{
    [Fact]
    public void FakerLocale_ShouldBe_GB()
    {
        // AAA
        Defaults.FakerLocale.ShouldBe("en_GB");
    }

    [Fact]
    public void Culture_ShouldBe_GB()
    {
        // AAA
        Defaults.Culture.Name.ShouldBe("en-GB");
    }

    [Fact]
    public void TimestampFormat_ShouldBe_yyyyMMdd_HHmmssfff()
    {
        // AAA
        Defaults.TimestampFormat.ShouldBe("yyyyMMdd_HHmmssfff");
    }

    [Fact]
    public void TimestampFormatNoUnderscores_ShouldBe_yyyyMMddHHmmssfff()
    {
        // AAA
        Defaults.TimestampFormatNoUnderscores.ShouldBe("yyyyMMddHHmmssfff");
    }
}
