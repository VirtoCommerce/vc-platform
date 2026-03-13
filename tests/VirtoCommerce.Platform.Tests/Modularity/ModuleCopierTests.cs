using System.IO;
using VirtoCommerce.Platform.Modules;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Modularity;

public class ModuleCopierTests
{
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
    };

    public static TheoryData<string> SkippedFilePathData => new()
    {
        { "ref/assembly.dll" },
        { "Localizations/de.Module.json" },
    };

    [Theory]
    [MemberData(nameof(FilePathTestData))]
    public void GetTargetRelativePath_MapsCorrectly(string sourcePath, string expectedTargetPath)
    {
        // Make tests independent of DirectorySeparatorChar
        sourcePath = sourcePath.Replace('/', Path.DirectorySeparatorChar);

        var actualTargetPath = ModuleCopier.GetTargetRelativePath(sourcePath);

        // Make tests independent of DirectorySeparatorChar
        actualTargetPath = actualTargetPath?.Replace(Path.DirectorySeparatorChar, '/');

        Assert.Equal(expectedTargetPath, actualTargetPath);
    }

    [Theory]
    [MemberData(nameof(SkippedFilePathData))]
    public void GetTargetRelativePath_SkipsExcludedFiles(string sourcePath)
    {
        sourcePath = sourcePath.Replace('/', Path.DirectorySeparatorChar);

        var result = ModuleCopier.GetTargetRelativePath(sourcePath);

        Assert.Null(result);
    }

    [Fact]
    public void GetTargetRelativePath_PreservesRuntimesDirectory()
    {
        var sourcePath = Path.Combine("runtimes", "win-x64", "native", "test.dll");

        var result = ModuleCopier.GetTargetRelativePath(sourcePath);

        Assert.Equal(sourcePath, result);
    }

    [Fact]
    public void GetTargetRelativePath_FlattensNestedAssemblyFiles()
    {
        var sourcePath = Path.Combine("Debug", "net10.0", "MyModule.dll");

        var result = ModuleCopier.GetTargetRelativePath(sourcePath);

        Assert.Equal("MyModule.dll", result);
    }

    [Fact]
    public void GetTargetRelativePath_PreservesLocalizationFolder()
    {
        var sourcePath = Path.Combine("de", "MyModule.resources.dll");

        var result = ModuleCopier.GetTargetRelativePath(sourcePath);

        Assert.Equal(Path.Combine("de", "MyModule.resources.dll"), result);
    }

    [Fact]
    public void GetTargetRelativePath_ReturnsNullForUnknownFiles()
    {
        var result = ModuleCopier.GetTargetRelativePath("readme.txt");

        Assert.Null(result);
    }
}
