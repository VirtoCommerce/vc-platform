using System;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Common
{
    [CLSCompliant(false)]
    public class EnumUtilityTests
    {
        [Flags]
        private enum TestFlagType
        {
            None = 0,
            E0 = 1,
            E1 = 1 << 1,
            E2 = 1 << 2,
            E3 = 1 << 3,
            All = None | E0 | E1 | E2 | E3,
        }

        private enum TestEnumType
        {
            None = 0,
            E0 = 1,
            E1 = 1 << 1,
            E2 = 1 << 2,
            E3 = 1 << 3,
            All = None | E0 | E1 | E2 | E3,
        }

        [Theory]
        // Flags
        // Fully valid combinations
        [InlineData("None", TestFlagType.None, TestFlagType.E1)]
        [InlineData("E1", TestFlagType.E1, TestFlagType.None)]
        [InlineData("E2", TestFlagType.E2, TestFlagType.None)]
        [InlineData("E0,E3", TestFlagType.E0 | TestFlagType.E3, TestFlagType.None)]
        [InlineData("None, E0, E1, E2", TestFlagType.E0 | TestFlagType.E1 | TestFlagType.E2, TestFlagType.None)]
        [InlineData("None, E0, E1, E2, E3, ", TestFlagType.All, TestFlagType.None)]
        [InlineData("None, E0, E1, E2, E3, All", TestFlagType.All, TestFlagType.None)]
        // Partially valid combinations
        [InlineData("None, E0, E1, E2, skljdhdf", TestFlagType.E0 | TestFlagType.E1 | TestFlagType.E2, TestFlagType.None)]
        [InlineData("sdf, sdf, E0, dff, E1, E2df", TestFlagType.E0 | TestFlagType.E1, TestFlagType.None)]
        // Invalid combinations
        [InlineData("", TestFlagType.None, TestFlagType.None)]
        [InlineData("sjkdhdfjkshdkfj", TestFlagType.None, TestFlagType.None)]
        [InlineData("sjkdhdfjkshdkfj, klf,", TestFlagType.None, TestFlagType.None)]
        [InlineData("100", (TestFlagType)100, TestFlagType.None)]
        [InlineData("200", (TestFlagType)200, TestFlagType.None)]
        //Default tests
        [InlineData("", TestFlagType.E3, TestFlagType.E3)]
        [InlineData("2ds", TestFlagType.E2, TestFlagType.E2)]
        [InlineData("Non", TestFlagType.None, TestFlagType.None)]

        //Custom separator tests
        [InlineData("E0;E3", TestFlagType.E0 | TestFlagType.E3, TestFlagType.None, ";")]
        [InlineData("None\tE0\tE1\tE2\t", TestFlagType.E0 | TestFlagType.E1 | TestFlagType.E2, TestFlagType.None, "\t")]
        [InlineData("E1Complex_sparatorE2", TestFlagType.E1 | TestFlagType.E2, TestFlagType.None, "Complex_sparator")]
        public void TestEnumParsing<T>(string parsedString, T expectedResult, T defaultResult, string separator = ",")
            where T : struct
        {
            var result = EnumUtility.SafeParseFlags(parsedString, defaultResult, separator);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        // Oridnary enum
        [InlineData("None", TestEnumType.None)]
        [InlineData("None", TestEnumType.None, "\t")]
        public void TestEnumParsingThrowsForNonFlags<T>(string parsedString, T defaultResult,  string separator = ",")
            where T : struct
        {
            Assert.Throws<ArgumentException>(() => EnumUtility.SafeParseFlags(parsedString, defaultResult, separator));
        }
    }
}
