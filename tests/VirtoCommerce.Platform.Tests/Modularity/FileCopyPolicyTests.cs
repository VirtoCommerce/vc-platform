using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules.Local;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Modularity;

public class FileCopyPolicyTests
{
    private readonly Mock<IFileMetadataProvider> _metadataProvider = new();

    public static TheoryData<string, string> FilePathTestData => new()
    {
        // Source path                                          Expected target path
        { "Module.pdb",                                         "Module.pdb"                                     },
        { "Module.xml",                                         "Module.xml"                                     },
        { "Module.deps.json",                                   "Module.deps.json"                               },
        { "Module.dll",                                         "Module.dll"                                     },
        { "de/Module.resources.dll",                            "de/Module.resources.dll"                        },
        { "Debug/net8.0/Module.pdb",                            "Module.pdb"                                     },
        { "Debug/net8.0/Module.xml",                            "Module.xml"                                     },
        { "Debug/net8.0/Module.deps.json",                      "Module.deps.json"                               },
        { "Debug/net8.0/Module.dll",                            "Module.dll"                                     },
        { "Debug/net8.0/de/Module.resources.dll",               "de/Module.resources.dll"                        },
        { "runtimes/linux-x64/native/native.so",                "runtimes/linux-x64/native/native.so"            },
        { "runtimes/win-x64/native/native.dll",                 "runtimes/win-x64/native/native.dll"             },
        { "runtimes/osx-x64/native/native.dylib",               "runtimes/osx-x64/native/native.dylib"           },
        { "runtimes/win/lib/net7.0/System.Management.dll",      "runtimes/win/lib/net7.0/System.Management.dll"  },
        { "runtimes/win/lib/net6.0/System.Drawing.Common.dll",  null                                             },
        { "Hangfire.Core.dll",                                  null                                             },
        { "de/Hangfire.Core.resources.dll",                     null                                             },
        { "ref/assembly.dll",                                   null                                             },
        { "Localizations/de.Module.json",                       null                                             },
        { "System.Runtime.dll",                                 null                                             },
    };

    [Theory]
    [MemberData(nameof(FilePathTestData))]
    public void FilePathTests(string sourcePath, string expectedTargetPath)
    {
        // Arrange
        var copyFilePolicy = GetFileCopyPolicy();

        // Make tests independent of DirectorySeparatorChar
        sourcePath = sourcePath.Replace('/', Path.DirectorySeparatorChar);

        // Act
        var actualTargetPath = copyFilePolicy.GetTargetRelativePath(sourcePath);

        // Make tests independent of DirectorySeparatorChar
        actualTargetPath = actualTargetPath?.Replace(Path.DirectorySeparatorChar, '/');

        // Assert
        Assert.Equal(expectedTargetPath, actualTargetPath);
    }

    [Theory]
    [InlineData(false, null, true)]
    [InlineData(true, false, false)]
    [InlineData(true, true, true)]
    public void NonExecutableFilesCopyTest(
        bool targetExists,
        bool? isTargetOlder,
        bool expectedCopyRequired)
    {
        // Arrange
        const string sourcePath = @"c:\source\non_executable.xml";
        const string targetPath = @"c:\target\non_executable.xml";

        var sourceDate = DateTime.UtcNow;
        AddFile(sourcePath, sourceDate, version: null, architecture: null);

        if (targetExists)
        {
            var targetDate = isTargetOlder == true ? sourceDate.AddDays(-1) : sourceDate;
            AddFile(targetPath, targetDate, version: null, architecture: null);
        }

        var copyFilePolicy = GetFileCopyPolicy();

        // Act
        var actualCopyRequired = copyFilePolicy.IsCopyRequired(Architecture.X64, sourcePath, targetPath, out _);

        // Assert
        Assert.Equal(expectedCopyRequired, actualCopyRequired);
    }

    [Theory]
    [InlineData(null, false, null, null, true)]
    [InlineData(null, true, "1.0.0.0", true, false)]
    [InlineData(null, true, "1.0.0.0", false, false)]
    [InlineData("1.0.0.0", false, null, null, true)]
    [InlineData("1.0.0.0", true, "1.0.0.0", false, false)]
    [InlineData("1.0.0.0", true, "1.0.0.0", true, true)]
    [InlineData("1.0.0.0", true, "1.0.0.1", false, false)]
    [InlineData("1.0.0.0", true, "1.0.0.1", true, false)]
    [InlineData("1.0.0.2", true, "1.0.0.1", false, true)]
    [InlineData("1.0.0.2", true, "1.0.0.1", true, true)]
    public void ExecutableFilesWithDifferentVersionsCopyTest(
        string sourceVersion,
        bool targetExists,
        string targetVersion,
        bool? isTargetOlder,
        bool expectedCopyRequired)
    {
        // Arrange
        const string sourcePath = @"c:\source\assembly.dll";
        const string targetPath = @"c:\target\assembly.dll";

        var sourceDate = DateTime.UtcNow;
        AddFile(sourcePath, sourceDate, sourceVersion, Architecture.X64);

        if (targetExists)
        {
            var targetDate = isTargetOlder == true ? sourceDate.AddDays(-1) : sourceDate;
            AddFile(targetPath, targetDate, targetVersion, Architecture.X64);
        }

        var copyFilePolicy = GetFileCopyPolicy();

        // Act
        var actualCopyRequired = copyFilePolicy.IsCopyRequired(Architecture.X64, sourcePath, targetPath, out _);

        // Assert
        Assert.Equal(expectedCopyRequired, actualCopyRequired);
    }

    [Theory]
    [InlineData(Architecture.X64, null, false, null, null, true)]
    [InlineData(Architecture.X64, null, true, null, true, true)]
    [InlineData(Architecture.X64, null, true, null, false, false)]
    [InlineData(Architecture.X64, null, true, Architecture.X64, true, false)]
    [InlineData(Architecture.X64, null, true, Architecture.X86, true, false)]
    [InlineData(Architecture.X64, Architecture.X64, false, null, null, true)]
    [InlineData(Architecture.X64, Architecture.X86, false, null, null, true)]
    [InlineData(Architecture.X86, Architecture.X86, false, null, null, true)]
    [InlineData(Architecture.X86, Architecture.X64, false, null, null, false)]
    [InlineData(Architecture.X64, Architecture.X64, true, null, true, true)]
    [InlineData(Architecture.X64, Architecture.X64, true, Architecture.X64, false, false)]
    [InlineData(Architecture.X64, Architecture.X64, true, Architecture.X86, false, true)]
    [InlineData(Architecture.X64, Architecture.X86, true, Architecture.X64, false, false)]
    [InlineData(Architecture.X64, Architecture.X86, true, Architecture.X86, false, false)]
    [InlineData(Architecture.X64, Architecture.X64, true, Architecture.X64, true, true)]
    [InlineData(Architecture.X64, Architecture.X64, true, Architecture.X86, true, true)]
    [InlineData(Architecture.X64, Architecture.X86, true, Architecture.X64, true, false)]
    [InlineData(Architecture.X64, Architecture.X86, true, Architecture.X86, true, true)]
    [InlineData(Architecture.X86, Architecture.X64, true, Architecture.X64, false, false)]
    [InlineData(Architecture.X86, Architecture.X64, true, Architecture.X86, false, false)]
    [InlineData(Architecture.X86, Architecture.X86, true, Architecture.X64, false, true)]
    [InlineData(Architecture.X86, Architecture.X86, true, Architecture.X86, false, false)]
    [InlineData(Architecture.X86, Architecture.X64, true, Architecture.X64, true, true)]
    [InlineData(Architecture.X86, Architecture.X64, true, Architecture.X86, true, false)]
    [InlineData(Architecture.X86, Architecture.X86, true, Architecture.X64, true, true)]
    [InlineData(Architecture.X86, Architecture.X86, true, Architecture.X86, true, true)]
    public void ExecutableFilesWithDifferentArchitectureCopyTest(
        Architecture environment,
        Architecture? sourceArchitecture,
        bool targetExists,
        Architecture? targetArchitecture,
        bool? isTargetOlder,
        bool expectedCopyRequired)
    {
        // Arrange
        const string sourcePath = @"c:\source\assembly.dll";
        const string targetPath = @"c:\target\assembly.dll";

        var sourceDate = DateTime.UtcNow;
        AddFile(sourcePath, sourceDate, "1.0.0.0", sourceArchitecture);

        if (targetExists)
        {
            var targetDate = isTargetOlder == true ? sourceDate.AddDays(-1) : sourceDate;
            AddFile(targetPath, targetDate, "1.0.0.0", targetArchitecture);
        }

        var copyFilePolicy = GetFileCopyPolicy();

        // Act
        var actualCopyRequired = copyFilePolicy.IsCopyRequired(environment, sourcePath, targetPath, out _);

        // Assert
        Assert.Equal(expectedCopyRequired, actualCopyRequired);
    }


    private FileCopyPolicy GetFileCopyPolicy()
    {
        return new FileCopyPolicy(_metadataProvider.Object, Options.Create(new LocalStorageModuleCatalogOptions()));
    }

    private void AddFile(string path, DateTime date, string version, Architecture? architecture)
    {
        _metadataProvider.Setup(x => x.Exists(path)).Returns(true);
        _metadataProvider.Setup(x => x.GetDate(path)).Returns(date);
        _metadataProvider.Setup(x => x.GetVersion(path)).Returns(version is null ? null : new Version(version));
        _metadataProvider.Setup(x => x.GetArchitecture(path)).Returns(architecture);
    }
}
