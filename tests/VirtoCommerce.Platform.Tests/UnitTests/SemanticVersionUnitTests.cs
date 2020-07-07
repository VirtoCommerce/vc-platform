using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    public class SemanticVersionUnitTests
    {
        [Theory]
        [InlineData("3.3.0", 3, 3, 0, "")]
        [InlineData("3.3.0-alpha.1499", 3, 3, 0, "alpha.1499")]
        [InlineData("3.3.0-alpha001", 3, 3, 0, "alpha001")]
        public void Parse(string version, int majorExpected, int minorExpected, int patchExpected, string prereleaseExpected)
        {
            //Act
            var semVer = SemanticVersion.Parse(version);

            //Assert
            Assert.Equal(majorExpected, semVer.Major);
            Assert.Equal(minorExpected, semVer.Minor);
            Assert.Equal(patchExpected, semVer.Patch);
            Assert.Equal(prereleaseExpected, semVer.Prerelease);
        }
    }
}
