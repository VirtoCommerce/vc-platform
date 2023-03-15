using System;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Core.Tests.Common;

[Trait("Category", "Unit")]
public class EnumUtilityTests
{
    [Flags]
    public enum TestEnum
    {
        None = 0,
        One = 1,
        Two = 2,
        Four = 4,
        Full = One | Two | Four,
    }

    [Theory]
    [InlineData("0", ",", TestEnum.None)]
    [InlineData("1", ",", TestEnum.One)]
    [InlineData("1,2", ",", TestEnum.One | TestEnum.Two)]
    [InlineData("1,2,4", ",", TestEnum.Full)]

    [InlineData("None", ",", TestEnum.None)]
    [InlineData("One", ",", TestEnum.One)]
    [InlineData("One,Two", ",", TestEnum.One | TestEnum.Two)]
    [InlineData("One,Two,Four", ",", TestEnum.Full)]
    [InlineData("Full", ",", TestEnum.Full)]

    [InlineData("One;Two", ";", TestEnum.One | TestEnum.Two)]
    [InlineData("One;Two;Four", ";", TestEnum.Full)]
    public void SafeParseFlagsTest(string value, string separator, TestEnum expectedResult)
    {
        var result = EnumUtility.SafeParseFlags(value, TestEnum.None, separator);
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("1", TestEnum.Two, ',', "1")]
    [InlineData("2", TestEnum.Two, ',', "0")]
    [InlineData("3", TestEnum.Two, ',', "1")]

    [InlineData("One", TestEnum.Two, ',', "One")]
    [InlineData("Two", TestEnum.Two, ',', "None")]
    [InlineData("One,Two", TestEnum.Two, ',', "One")]
    [InlineData("One,Two,Four", TestEnum.Two, ',', "One,Four")]
    [InlineData("Full", TestEnum.Two, ',', "One,Four")]

    [InlineData("One;Two;Four", TestEnum.Two, ';', "One;Four")]
    [InlineData("Full", TestEnum.Two, ';', "One;Four")]
    public void SafeRemoveFlagFromEnumStringTest(string value, TestEnum flag, char separator, string expectedResult)
    {
        var result = EnumUtility.SafeRemoveFlagFromEnumString(value, flag, separator);
        Assert.Equal(expectedResult, result);
    }
}
