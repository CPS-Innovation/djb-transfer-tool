using System.ComponentModel.DataAnnotations;

namespace Cps.Fct.Hk.Common.Infrastructure.UnitTests.Extensions.TestData;

public enum TestEnumeration
{
    /// <summary>
    /// Big dog.
    /// </summary>
    [Display(Name = "This is a Big Dog!")]
    BigDog,

    /// <summary>
    /// Small cat.
    /// </summary>
    SmallCat,

    /// <summary>
    /// Massive fox.
    /// </summary>
    [Display(Name = "Massive Fox")]
    MassiveFox
}
