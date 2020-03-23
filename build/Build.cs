using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Git;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.Npm;
using Nuke.Common.Tools.OpenCover;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
using VirtoCommerce.Platform.Core.Modularity;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public Build()
    {
        ToolPathResolver.ExecutingAssemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    private static string[] ModuleContentFolders = new[] { "dist", "Localizations", "Scripts", "Content" };

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion] readonly GitVersion GitVersion;

    readonly Tool Git;

    readonly string MasterBranch = "master";
    readonly string DevelopBranch = "develop";
    readonly string ReleaseBranchPrefix = "release";
    readonly string HotfixBranchPrefix = "hotfix";

    private static readonly HttpClient httpClient = new HttpClient();

    [Parameter("ApiKey for the specified source")] readonly string ApiKey;
    [Parameter] readonly string Source = @"https://api.nuget.org/v3/index.json";

    [Parameter] static string GlobalModuleIgnoreFileUrl = @"https://raw.githubusercontent.com/VirtoCommerce/vc-platform/release/3.0.0/module.ignore";

    [Parameter] readonly string SonarAuthToken = "";
    [Parameter] readonly string SonarUrl = "https://sonar.virtocommerce.com";
    [Parameter] readonly AbsolutePath CoverageReportPath = RootDirectory / ".tmp" / "coverage.xml";
    [Parameter] readonly string TestsFilter = "Category!=IntegrationTest";

    [Parameter("Url to Swagger Validation Api")] readonly string SwaggerValidationUrl = "http://validator.swagger.io/validator/debug";

    [Parameter("GitHub user for release creation")] readonly string GitHubUser;
    [Parameter("GitHub user security token for release creation")] readonly string GitHubToken;
    [Parameter("True - prerelease, False - release")] readonly bool PreRelease;

    [Parameter("Path to folder with  git clones of modules repositories")] readonly AbsolutePath ModulesFolderPath;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    Project WebProject => Solution.AllProjects.FirstOrDefault(x => x.SolutionFolder?.Name == "src" && x.Name.EndsWith("Web"));
    AbsolutePath ModuleManifestFile => WebProject.Directory / "module.manifest";
    AbsolutePath ModuleIgnoreFile => RootDirectory / "module.ignore";

    ModuleManifest ModuleManifest => ManifestReader.Read(ModuleManifestFile);
    string ModuleSemVersion => string.Join("-", ModuleManifest.Version, ModuleManifest.VersionTag);

    AbsolutePath ModuleOutputDirectory => ArtifactsDirectory / (ModuleManifest.Id + ModuleSemVersion);

    string ZipFileName => IsModule ? ModuleManifest.Id + "_" + string.Join("-", ModuleManifest.Version, ModuleManifest.VersionTag) + ".zip" : "VirtoCommerce.Platform." + GitVersion.SemVer + ".zip";
    string ZipFilePath => ArtifactsDirectory / ZipFileName;
    string GitRepositoryName => GitRepository.Identifier.Split('/')[1];

    string ModulePackageUrl => $"https://github.com/VirtoCommerce/{GitRepositoryName}/releases/download/{ModuleSemVersion}/{ModuleManifest.Id}_{ModuleSemVersion}.zip";
    GitRepository ModulesRepository => GitRepository.FromUrl("https://github.com/VirtoCommerce/vc-modules.git");

    bool IsModule => FileExists(ModuleManifestFile);

    void ErrorLogger(OutputType type, string text)
    {
        if (type == OutputType.Err) Logger.Error(text);
    }

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            if (DirectoryExists(TestsDirectory))
            {
                TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            }
            //if (DirectoryExists(TestsDirectory))
            //{
            //    WebProject.Directory.GlobDirectories("**/node_modules").ForEach(DeleteDirectory);
            //}
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Pack => _ => _
      .DependsOn(Test)
      .Executes(() =>
      {
          //For platform take nuget package description from Directory.Build.Props 
          var settings = new DotNetPackSettings()
               .SetProject(Solution)
                  .EnableNoBuild()
                  .SetConfiguration(Configuration)
                  .EnableIncludeSymbols()
                  .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
                  .SetOutputDirectory(ArtifactsDirectory)
                  .SetVersion(IsModule ? ModuleSemVersion : GitVersion.NuGetVersionV2);

          if (IsModule)
          {
              //For platform take nuget package description from module manifest
              settings.SetAuthors(ModuleManifest.Authors)
                  .SetPackageLicenseUrl(ModuleManifest.LicenseUrl)
                  .SetPackageProjectUrl(ModuleManifest.ProjectUrl)
                  .SetPackageIconUrl(ModuleManifest.IconUrl)
                  .SetPackageRequireLicenseAcceptance(false)
                  .SetDescription(ModuleManifest.Description)
                  .SetCopyright(ModuleManifest.Copyright);

              //Temporary disable GitVersionTask for module. Because version is taken from module.manifest.
              settings = settings.SetProperty("DisableGitVersionTask", false);
          }
          DotNetPack(settings);
      });

    Target Test => _ => _
       .DependsOn(Compile)
       .Executes(() =>
       {
           var dotnetPath = ToolPathResolver.GetPathExecutable("dotnet");
           var testProjects = Solution.GetProjects("*.Tests");
           if (testProjects.Count() > 0)
           {
               var testProjectPath = testProjects.First().Path;
               var testArgs = $"{testProjectPath} --logger trx --filter {TestsFilter}";
               OpenCoverTasks.OpenCover($"-target:\"{dotnetPath}\" -targetargs:\"test {testArgs}\" -register -output:\"{CoverageReportPath}\"");
           }
       });

    Target PublishPackages => _ => _
        .DependsOn(Pack)
        .Requires(() => ApiKey)
        .Executes(() =>
        {
            var packages = ArtifactsDirectory.GlobFiles("*.nupkg");

            DotNetNuGetPush(s => s
                    .SetSource(Source)
                    .SetApiKey(ApiKey)
                    .CombineWith(
                        packages, (cs, v) => cs
                            .SetTargetPath(v)),
                degreeOfParallelism: 5,
                completeOnFailure: true);
        });

    Target Publish => _ => _
       .DependsOn(Compile)
       .After(WebPackBuild, Test)
       .Executes(() =>
       {
           DotNetPublish(s => s
               .SetWorkingDirectory(WebProject.Directory)
               .EnableNoRestore()
               .SetOutput(IsModule ? ModuleOutputDirectory / "bin" : ArtifactsDirectory / "publish")
               .SetConfiguration(Configuration)
               .SetAssemblyVersion(IsModule ? ModuleManifest.Version : GitVersion.AssemblySemVer)
               .SetFileVersion(IsModule ? ModuleManifest.Version : GitVersion.AssemblySemFileVer)
               .SetInformationalVersion(IsModule ? ModuleSemVersion : GitVersion.InformationalVersion));

       });

    Target WebPackBuild => _ => _
     .Executes(() =>
     {
         if (FileExists(WebProject.Directory / "package.json"))
         {
             NpmTasks.Npm("ci", WebProject.Directory);
             NpmTasks.NpmRun(s => s.SetWorkingDirectory(WebProject.Directory).SetCommand("webpack:build"));
         }
         else
         {
             Logger.Info("Nothing to build.");
         }
     });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(IsModule ? ModuleManifest.Version : GitVersion.AssemblySemVer)
                .SetFileVersion(IsModule ? ModuleManifest.Version : GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(IsModule ? ModuleSemVersion : GitVersion.InformationalVersion)
                .EnableNoRestore());
        });

    Target Compress => _ => _
     .DependsOn(Clean, WebPackBuild, Test, Publish)
     .Executes(() =>

     {
         if (IsModule)
         {
             //Copy module.manifest and all content directories into a module output folder
             CopyFileToDirectory(ModuleManifestFile, ModuleOutputDirectory, FileExistsPolicy.Overwrite);
             foreach (var moduleFolder in ModuleContentFolders)
             {
                 var srcModuleFolder = WebProject.Directory / moduleFolder;
                 if (DirectoryExists(srcModuleFolder))
                 {
                     CopyDirectoryRecursively(srcModuleFolder, ModuleOutputDirectory / moduleFolder, DirectoryExistsPolicy.Merge, FileExistsPolicy.Overwrite);
                 }
             }

             var ignoredFiles = HttpTasks.HttpDownloadString(GlobalModuleIgnoreFileUrl).SplitLineBreaks();
             if (FileExists(ModuleIgnoreFile))
             {
                 ignoredFiles = ignoredFiles.Concat(TextTasks.ReadAllLines(ModuleIgnoreFile)).ToArray();
             }
             ignoredFiles = ignoredFiles.Select(x => x.Trim()).Distinct().ToArray();

             DeleteFile(ZipFilePath);
             //TODO: Exclude all dependencies of dependent modules
             CompressionTasks.CompressZip(ModuleOutputDirectory, ZipFilePath, (x) => !ignoredFiles.Contains(x.Name, StringComparer.OrdinalIgnoreCase)
                                                                                     && !(ModuleManifest.Dependencies ?? Array.Empty<ManifestDependency>()).Any(md => x.Name.StartsWith($"{md.Id}Module", StringComparison.OrdinalIgnoreCase)));
         }
         else
         {
             DeleteFile(ZipFilePath);
             CompressionTasks.CompressZip(ArtifactsDirectory / "publish", ZipFilePath);
         }
     });

    Target PublishModuleManifest => _ => _
        .Executes(() =>
        {
            var modulesLocalDirectory = ArtifactsDirectory / "vc-modules";
            var modulesJsonFile = modulesLocalDirectory / "modules_v3.json";
            if (!DirectoryExists(modulesLocalDirectory))
            {
                GitTasks.Git($"clone {ModulesRepository.HttpsUrl} {modulesLocalDirectory}");
            }
            else
            {
                GitTasks.Git($"pull", modulesLocalDirectory);
            }
            var manifest = ModuleManifest;

            var modulesExternalManifests = JsonConvert.DeserializeObject<List<ExternalModuleManifest>>(TextTasks.ReadAllText(modulesJsonFile));
            manifest.PackageUrl = ModulePackageUrl;
            var existExternalManifest = modulesExternalManifests.FirstOrDefault(x => x.Id == manifest.Id);
            if (existExternalManifest != null)
            {
                existExternalManifest.PublishNewVersion(manifest);
            }
            else
            {
                modulesExternalManifests.Add(ExternalModuleManifest.FromManifest(manifest));
            }
            TextTasks.WriteAllText(modulesJsonFile, JsonConvert.SerializeObject(modulesExternalManifests, Formatting.Indented));
            GitTasks.Git($"commit -am \"{manifest.Id} {ModuleSemVersion}\"", modulesLocalDirectory);

            GitTasks.Git($"push origin HEAD:master -f", modulesLocalDirectory);
        });

    Target SwaggerValidation => _ => _
          .DependsOn(Publish)
          .Requires(() => !IsModule)
          .Executes(() =>
          {
              var swashbucklePackage = NuGetPackageResolver.GetGlobalInstalledPackage("swashbuckle.aspnetcore.cli", "5.0.0", "dotnet-swagger.dll");
              var swashbucklePath = swashbucklePackage.Directory.GlobFiles("**/dotnet-swagger.dll").Last();
              var projectPublishPath = ArtifactsDirectory / "publish" / $"{WebProject.Name}.dll";
              var swaggerJson = ArtifactsDirectory / "swagger.json";
              var currentDir = Directory.GetCurrentDirectory();
              Directory.SetCurrentDirectory(RootDirectory / "src" / "VirtoCommerce.Platform.Web");
              DotNet($"{swashbucklePath} tofile --output {swaggerJson} {projectPublishPath} VirtoCommerce.Platform");
              Directory.SetCurrentDirectory(currentDir);

              var swaggerScheme = File.ReadAllText(swaggerJson);
              var requestContent = new StringContent(swaggerScheme, Encoding.UTF8, "application/json");
              var request = new HttpRequestMessage(HttpMethod.Post, SwaggerValidationUrl);
              request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
              request.Content = requestContent;
              var response = httpClient.SendAsync(request).GetAwaiter().GetResult();
              var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
              var jsonObj = JObject.Parse(responseContent);
              bool err = false;
              foreach (var msg in jsonObj["schemaValidationMessages"])
              {
                  Logger.Normal(msg);
                  if ((string)msg["level"] == "error")
                  {
                      err = true;
                  }
              }
              if (err)
                  ControlFlow.Fail("Schema Validation Messages contains error");
          });

    Target SonarQubeStart => _ => _
        .Executes(() =>
        {
            var dotNetPath = ToolPathResolver.TryGetEnvironmentExecutable("DOTNET_EXE") ?? ToolPathResolver.GetPathExecutable("dotnet");
            var branchName = GitRepository.Branch;
            var projectName = Solution.Name;

            var branchParam = $"/d:\"sonar.branch={branchName}\"";
            var projectNameParam = $"/n:\"{projectName}\"";
            var projectKeyParam = $"/k:\"{projectName}\"";
            var hostParam = $"/d:sonar.host.url={SonarUrl}";
            var tokenParam = $"/d:sonar.login={SonarAuthToken}";
            var sonarReportPathParam = $"/d:sonar.cs.opencover.reportsPaths={CoverageReportPath}";

            var startCmd = $"sonarscanner begin {branchParam} {projectNameParam} {projectKeyParam} {hostParam} {tokenParam} {sonarReportPathParam}";

            Logger.Normal($"Execute: {startCmd.Replace(SonarAuthToken, "{IS HIDDEN}")}");

            var processStart = ProcessTasks.StartProcess(dotNetPath, startCmd, customLogger: ErrorLogger, logInvocation: false)
                .AssertWaitForExit().AssertZeroExitCode();
            processStart.Output.EnsureOnlyStd();
        });

    Target SonarQubeEnd => _ => _
        .After(SonarQubeStart)
        .DependsOn(Compile)
        .Executes(() =>
        {
            var dotNetPath = ToolPathResolver.TryGetEnvironmentExecutable("DOTNET_EXE") ?? ToolPathResolver.GetPathExecutable("dotnet");
            var tokenParam = $"/d:sonar.login={SonarAuthToken}";
            var endCmd = $"sonarscanner end {tokenParam}";

            Logger.Normal($"Execute: {endCmd.Replace(SonarAuthToken, "{IS HIDDEN}")}");

            var processEnd = ProcessTasks.StartProcess(dotNetPath, endCmd, customLogger: ErrorLogger, logInvocation: false)
                .AssertWaitForExit().AssertZeroExitCode();
            processEnd.Output.EnsureOnlyStd();
        });

    Target StartAnalyzer => _ => _
        .DependsOn(SonarQubeStart, SonarQubeEnd)
        .Executes(() =>
        {
            Logger.Normal("Sonar validation done.");
        });


    Target MassPullAndBuild => _ => _
        .Requires(() => ModulesFolderPath)
        .Executes(() =>
        {
            if (DirectoryExists(ModulesFolderPath))
            {
                foreach (var moduleDirectory in Directory.GetDirectories(ModulesFolderPath))
                {
                    var isGitRepository = FileSystemTasks.FindParentDirectory(moduleDirectory, x => x.GetDirectories(".git").Any()) != null;
                    if (isGitRepository)
                    {
                        GitTasks.Git($"pull", moduleDirectory);
                        ProcessTasks.StartProcess("nuke", "Compile", moduleDirectory).AssertWaitForExit();
                        ProcessTasks.StartProcess("nuke", "WebPackBuild", moduleDirectory).AssertWaitForExit();
                    }
                }
            }
        });

    Target Release => _ => _
         .DependsOn(Clean, Compress)
         .Requires(() => GitHubUser, () => GitHubToken)
         /*.Requires(() =>   GitRepository.IsOnReleaseBranch() && GitTasks.GitHasCleanWorkingCopy()) */
         .Executes(() =>
         {
             var tag = "v" + (IsModule ? ModuleSemVersion : GitVersion.SemVer);
             //FinishReleaseOrHotfix(tag);

             void RunGitHubRelease(string args)
             {
                 ProcessTasks.StartProcess("github-release", args, RootDirectory).AssertZeroExitCode();
             }
             var prereleaseArg = PreRelease ? "--pre-release" : "";
             RunGitHubRelease($@"release --user {GitHubUser} -s {GitHubToken} --repo {GitRepositoryName} --tag {tag} {prereleaseArg}"); //-c branch -d description
             RunGitHubRelease($@"upload --user {GitHubUser} -s {GitHubToken} --repo {GitRepositoryName} --tag {tag} --name {ZipFileName} --file ""{ZipFilePath}""");
         });

    void FinishReleaseOrHotfix(string tag)
    {
        Git($"checkout {MasterBranch}");
        Git($"merge --no-ff --no-edit {GitRepository.Branch}");
        Git($"tag {tag}");

        Git($"checkout {DevelopBranch}");
        Git($"merge --no-ff --no-edit {GitRepository.Branch}");

        //Uncomment to switch on armed mode 
        //Git($"branch -D {GitRepository.Branch}");
        //Git($"push origin {MasterBranch} {DevelopBranch} {tag}");
    }
}
