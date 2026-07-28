using System;
using FluentAssertions;
using Xunit;

namespace VirtoCommerce.Platform.MigrationScripter.Tests
{
    public class MigrationScriptOptionsTests
    {
        [Fact]
        public void Parse_WithLongOptions_ReadsValues()
        {
            // Arrange
            var args = new[] { "--platform-path", @"C:\vc\platform", "--output", @"C:\vc\out" };

            // Act
            var options = MigrationScriptOptions.Parse(args);

            // Assert
            options.PlatformPath.Should().Be(@"C:\vc\platform");
            options.OutputPath.Should().Be(@"C:\vc\out");
            options.ShowHelp.Should().BeFalse();
        }

        [Fact]
        public void Parse_WithShortOptions_ReadsValues()
        {
            // Arrange
            var args = new[] { "-p", "platform", "-o", "out" };

            // Act
            var options = MigrationScriptOptions.Parse(args);

            // Assert
            options.PlatformPath.Should().Be("platform");
            options.OutputPath.Should().Be("out");
        }

        [Fact]
        public void Parse_WithHelp_SetsShowHelp()
        {
            // Arrange & Act
            var options = MigrationScriptOptions.Parse(new[] { "--help" });

            // Assert
            options.ShowHelp.Should().BeTrue();
        }

        [Fact]
        public void Parse_WithNoArgs_ReturnsDefaults()
        {
            // Arrange & Act
            var options = MigrationScriptOptions.Parse(Array.Empty<string>());

            // Assert
            options.PlatformPath.Should().BeNull();
            options.OutputPath.Should().BeNull();
            options.ShowHelp.Should().BeFalse();
        }

        [Fact]
        public void Parse_WithMissingValue_Throws()
        {
            // Arrange & Act
            var act = () => MigrationScriptOptions.Parse(new[] { "--output" });

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Parse_IgnoresUnknownArguments()
        {
            // Arrange & Act
            var options = MigrationScriptOptions.Parse(new[] { "--unknown", "value", "-p", "here" });

            // Assert
            options.PlatformPath.Should().Be("here");
        }
    }
}
