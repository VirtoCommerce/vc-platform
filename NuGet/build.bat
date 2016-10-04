SET "SOURCE_DIR=%~dp0%.."
SET "TARGET_DIR=%SOURCE_DIR%\NuGet"

IF NOT DEFINED ProgramFiles(x86) SET ProgramFiles(x86)=%ProgramFiles%
IF NOT DEFINED MSBUILD_PATH IF EXIST "%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" SET MSBUILD_PATH=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe
IF NOT DEFINED MSBUILD_PATH IF EXIST "%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe" SET MSBUILD_PATH=%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe
IF NOT DEFINED MSBUILD_PATH SET MSBUILD_PATH=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe

"%MSBUILD_PATH%" "%SOURCE_DIR%\VirtoCommerce.Platform.sln" /nologo /verbosity:m /t:Build /p:Configuration=Release;Platform="Any CPU"

nuget pack "%SOURCE_DIR%\VirtoCommerce.Platform.Core\VirtoCommerce.Platform.Core.csproj" -IncludeReferencedProjects -Symbols -Properties Configuration=Release -o "%TARGET_DIR%"
nuget pack "%SOURCE_DIR%\VirtoCommerce.Platform.Core.Web\VirtoCommerce.Platform.Core.Web.csproj" -IncludeReferencedProjects -Symbols -Properties Configuration=Release -o "%TARGET_DIR%"
nuget pack "%SOURCE_DIR%\VirtoCommerce.Platform.Data\VirtoCommerce.Platform.Data.csproj" -IncludeReferencedProjects -Symbols -Properties Configuration=Release -o "%TARGET_DIR%"
nuget pack "%SOURCE_DIR%\VirtoCommerce.Platform.Data.Azure\VirtoCommerce.Platform.Data.Azure.csproj" -IncludeReferencedProjects -Symbols -Properties Configuration=Release -o "%TARGET_DIR%"
nuget pack "%SOURCE_DIR%\VirtoCommerce.Platform.Data.Notifications\VirtoCommerce.Platform.Data.Notifications.csproj" -IncludeReferencedProjects -Symbols -Properties Configuration=Release -o "%TARGET_DIR%"
nuget pack "%SOURCE_DIR%\VirtoCommerce.Platform.Data.Security\VirtoCommerce.Platform.Data.Security.csproj" -IncludeReferencedProjects -Symbols -Properties Configuration=Release -o "%TARGET_DIR%"
nuget pack "%SOURCE_DIR%\VirtoCommerce.Platform.Data.Serialization\VirtoCommerce.Platform.Data.Serialization.csproj" -IncludeReferencedProjects -Symbols -Properties Configuration=Release -o "%TARGET_DIR%"
nuget pack "%SOURCE_DIR%\VirtoCommerce.Platform.Testing\VirtoCommerce.Platform.Testing.csproj" -IncludeReferencedProjects -Symbols -Properties Configuration=Release -o "%TARGET_DIR%"

@pause
