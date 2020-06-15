using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nuke.Common;
using Nuke.Common.CI.Jenkins;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Git;
using Nuke.Common.Tools.GitReleaseManager;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.Npm;
using Nuke.Common.Tools.OpenCover;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Git.GitTasks;

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

    public static int Main()
    {
        var nukeFile = Directory.GetFiles(Directory.GetCurrentDirectory(), ".nuke");
        if(!nukeFile.Any())
        {
            Logger.Info("No .nuke file found!");
            var solutions = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.sln");
            if(solutions.Length == 1)
            {
                var solutionFileName = Path.GetFileName(solutions.First());
                Logger.Info($"Solution found: {solutionFileName}");
                File.WriteAllText(".nuke", solutionFileName);
            }
        }
        var exitCode = Execute<Build>(x => x.Compile);
        return ExitCode ?? exitCode;
    }

    private static int? ExitCode = null;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    private static string[] ModuleContentFolders = new[] { "dist", "Localizations", "Scripts", "Content" };

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;

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

    [Parameter("Url to Swagger Validation Api")] readonly string SwaggerValidatorUri = "http://validator.swagger.io/validator/debug";

    [Parameter("GitHub user for release creation")] readonly string GitHubUser;
    [Parameter("GitHub user security token for release creation")] readonly string GitHubToken;
    [Parameter("True - prerelease, False - release")] readonly bool PreRelease;
    [Parameter("True - Pull Request")] readonly bool PullRequest;

    [Parameter("Path to folder with  git clones of modules repositories")] readonly AbsolutePath ModulesFolderPath;
    [Parameter("Repo Organization/User")] readonly string RepoOrg = "VirtoCommerce";
    [Parameter("Repo Name")] string RepoName;

    [Parameter("Path to nuget config")] readonly AbsolutePath NugetConfig;

    [Parameter("Swagger schema path")] readonly AbsolutePath SwaggerSchemaPath;

    [Parameter("Path to modules.json")] readonly string ModulesJsonName = "modules_v3.json";
    [Parameter("Full uri to module artifact")] readonly string CustomModulePackageUri;

    [Parameter("Custom Version Suffix")] readonly string CustomTagSuffix = "";

    [Parameter("Path to Release Notes File")] readonly AbsolutePath ReleaseNotes;
   
    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    [Parameter("Path to Artifacts Directory")] AbsolutePath ArtifactsDirectory = RootDirectory / "artifacts";
    Project WebProject => Solution.AllProjects.FirstOrDefault(x => (x.SolutionFolder?.Name == "src" && x.Name.EndsWith("Web")) || x.Name.EndsWith("VirtoCommerce.Storefront"));
    AbsolutePath ModuleManifestFile => WebProject.Directory / "module.manifest";
    AbsolutePath ModuleIgnoreFile => RootDirectory / "module.ignore";

    Microsoft.Build.Evaluation.Project MSBuildProject => WebProject.GetMSBuildProject();
    string VersionPrefix => MSBuildProject.GetProperty("VersionPrefix").EvaluatedValue;
    string VersionSuffix => MSBuildProject.GetProperty("VersionSuffix").EvaluatedValue;
    string PackageVersion => MSBuildProject.GetProperty("PackageVersion").EvaluatedValue;
    string ReleaseVersion => PackageVersion;

    ModuleManifest ModuleManifest => ManifestReader.Read(ModuleManifestFile);

    AbsolutePath ModuleOutputDirectory => ArtifactsDirectory / ModuleManifest.Id;
    
    string ZipFileName => IsModule ? $"{ModuleManifest.Id}_{ReleaseVersion}{CustomTagSuffix}.zip" : $"{WebProject.Solution.Name}.{ReleaseVersion}{CustomTagSuffix}.zip";
    string ZipFilePath => ArtifactsDirectory / ZipFileName;
    string GitRepositoryName => GitRepository.Identifier.Split('/')[1];

    string ModulePackageUrl => CustomModulePackageUri.IsNullOrEmpty() ?
        $"https://github.com/VirtoCommerce/{GitRepositoryName}/releases/download/{ReleaseVersion}/{ModuleManifest.Id}_{ReleaseVersion}.zip" : CustomModulePackageUri;
    GitRepository ModulesRepository => GitRepository.FromUrl("https://github.com/VirtoCommerce/vc-modules.git");

    bool IsModule => FileExists(ModuleManifestFile);

    void SonarLogger(OutputType type, string text)
    {
        switch (type)
        {
            case OutputType.Err:
                Logger.Error(text);
                break;
            case OutputType.Std:
                Logger.Info(text);
                break;
        }
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
                .SetProjectFile(Solution)
                .When(NugetConfig != null, c => c
                    .SetConfigFile(NugetConfig))
                );
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
                  .SetVersion(ReleaseVersion);

          if (IsModule)
          {
              //For module take nuget package description from module manifest
              settings.SetAuthors(ModuleManifest.Authors)
                  .SetPackageLicenseUrl(ModuleManifest.LicenseUrl)
                  .SetPackageProjectUrl(ModuleManifest.ProjectUrl)
                  .SetPackageIconUrl(ModuleManifest.IconUrl)
                  .SetPackageRequireLicenseAcceptance(false)
                  .SetDescription(ModuleManifest.Description)
                  .SetCopyright(ModuleManifest.Copyright);
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
               var registerArg = IsServerBuild ? "-register" : "-register:user";
               OpenCoverTasks.OpenCover($"-target:\"{dotnetPath}\" -targetargs:\"test {testArgs}\" {registerArg} -output:\"{CoverageReportPath}\" -returntargetcode");
           }
       });

    public void CustomDotnetLogger(OutputType type, string text)
    {
        Logger.Info(text);
        if (text.Contains("error: Response status code does not indicate success: 409"))
        {
            ExitCode = 409;
        }
    }

    Target PublishPackages => _ => _
        .DependsOn(Pack)
        .Requires(() => ApiKey)
        .Executes(() =>
        {
            var packages = ArtifactsDirectory.GlobFiles("*.nupkg");

            DotNetLogger = CustomDotnetLogger;

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
               .When(IsModule, ss => ss
                   .SetAssemblyVersion(ModuleManifest.Version)
                   .SetFileVersion(ModuleManifest.Version)
                   .SetInformationalVersion(ReleaseVersion)));

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
                .EnableNoRestore()
                .When(IsModule, ss => ss
                    .SetAssemblyVersion(ModuleManifest.Version)
                    .SetFileVersion(ModuleManifest.Version)
                    .SetInformationalVersion(ReleaseVersion)));
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
            GitTasks.GitLogger = GitLogger;
            var modulesLocalDirectory = ArtifactsDirectory / "vc-modules";
            var modulesJsonFile = modulesLocalDirectory / ModulesJsonName;
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
            manifest.VersionTag = manifest.VersionTag.Replace("$", "");
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
            GitTasks.Git($"commit -am \"{manifest.Id} {ReleaseVersion}\"", modulesLocalDirectory);
            GitTasks.Git($"push origin HEAD:master -f", modulesLocalDirectory);
        });

    Target SwaggerValidation => _ => _
          .DependsOn(Publish)
          .Requires(() => !IsModule)
          .Executes(async () =>
          {
              var swashbucklePackage = NuGetPackageResolver.GetGlobalInstalledPackage("swashbuckle.aspnetcore.cli", "5.2.1", ToolPathResolver.NuGetPackagesConfigFile);

              var swashbucklePath = swashbucklePackage.Directory.GlobFiles("**/dotnet-swagger.dll").Last();
              var projectPublishPath = ArtifactsDirectory / "publish" / $"{WebProject.Name}.dll";
              var swaggerJson = ArtifactsDirectory / "swagger.json";
              var currentDir = Directory.GetCurrentDirectory();
              Directory.SetCurrentDirectory(RootDirectory / "src" / "VirtoCommerce.Platform.Web");
              DotNet($"{swashbucklePath} tofile --output {swaggerJson} {projectPublishPath} VirtoCommerce.Platform");
              Directory.SetCurrentDirectory(currentDir);

              var responseContent = await SendSwaggerSchemaToValidator(httpClient, swaggerJson, SwaggerValidatorUri);
              var jsonObj = JObject.Parse(responseContent);
              foreach (var msg in jsonObj["schemaValidationMessages"])
              {
                  Logger.Normal(msg);
              }
              if (jsonObj["schemaValidationMessages"].Where(t => (string)t["level"] == "error").Any())
                  ControlFlow.Fail("Schema Validation Messages contains error");
          });

    Target ValidateSwaggerSchema => _ => _
        .Requires(() => SwaggerSchemaPath != null)
        .Executes(async () =>
        {
            var responseContent = await SendSwaggerSchemaToValidator(httpClient, SwaggerSchemaPath, SwaggerValidatorUri);
            var jsonObj = JObject.Parse(responseContent);
            foreach (var msg in jsonObj["schemaValidationMessages"])
            {
                Logger.Normal(msg);
            }
            if (jsonObj["schemaValidationMessages"].Where(t => (string)t["level"] == "error").Any())
                ControlFlow.Fail("Schema Validation Messages contains error");
        });

    private async Task<string> SendSwaggerSchemaToValidator(HttpClient httpClient, string schemaPath, string validatorUri)
    {
        var swaggerScheme = File.ReadAllText(schemaPath);
        var requestContent = new StringContent(swaggerScheme, Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, validatorUri);
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        request.Content = requestContent;
        var response = await httpClient.SendAsync(request);
        return await response.Content.ReadAsStringAsync();
    }

    Target SonarQubeStart => _ => _
        .Executes(() =>
        {
            var dotNetPath = ToolPathResolver.TryGetEnvironmentExecutable("DOTNET_EXE") ?? ToolPathResolver.GetPathExecutable("dotnet");
            Logger.Normal($"IsServerBuild = {IsServerBuild}");
            var branchName = GitRepository.Branch;
            Logger.Info($"BRANCH_NAME = {branchName}");
            var projectName = Solution.Name;
            var previewModeParam = "";
            var prParam = "";
            var repositoryParam = "";
            var githubAuthParam = "";
            if (PullRequest)
            {
                var prNumber = Environment.GetEnvironmentVariable("CHANGE_ID");
                prParam = $"/d:sonar.github.pullRequest={prNumber}";
                previewModeParam = "/d:sonar.analysis.mode=preview";
                if (RepoName.IsNullOrEmpty())
                {
                    RepoName = Jenkins.Instance.JobName.Split("/")[1];
                }
                repositoryParam = $"/d:sonar.github.repository={RepoOrg}/{RepoName}";
                githubAuthParam = $"/d:sonar.github.oauth={GitHubToken}";
            }
            var branchParam = $"/d:\"sonar.branch={branchName}\"";
            var projectNameParam = $"/n:\"{projectName}\"";
            var projectKeyParam = $"/k:\"{projectName}\"";
            var hostParam = $"/d:sonar.host.url={SonarUrl}";
            var tokenParam = $"/d:sonar.login={SonarAuthToken}";
            var sonarReportPathParam = $"/d:sonar.cs.opencover.reportsPaths={CoverageReportPath}";

            var startCmd = $"sonarscanner begin {branchParam} {projectNameParam} {projectKeyParam} {hostParam} {tokenParam} {sonarReportPathParam} {previewModeParam} {prParam} {repositoryParam} {githubAuthParam}";

            Logger.Normal($"Execute: {startCmd.Replace(SonarAuthToken, "{IS HIDDEN}").Replace(GitHubToken, "{IS HIDDEN}")}");

            var processStart = ProcessTasks.StartProcess(dotNetPath, startCmd, customLogger: SonarLogger, logInvocation: false)
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

            var processEnd = ProcessTasks.StartProcess(dotNetPath, endCmd, customLogger: SonarLogger, logInvocation: false)
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

    void GitLogger(OutputType type, string text)
    {
        if (text.Contains("github returned 422 Unprocessable Entity"))
        {
            ExitCode = 422;
        }
        if (text.Contains("nothing to commit, working tree clean"))
        {
            ExitCode = 423;
        }
        switch (type)
        {
            case OutputType.Err:
                Logger.Error(text);
                break;
            case OutputType.Std:
                Logger.Info(text);
                break;
        }
    }

    Target Release => _ => _
         .DependsOn(Clean, Compress)
         .Requires(() => GitHubUser, () => GitHubToken)
         /*.Requires(() =>   GitRepository.IsOnReleaseBranch() && GitTasks.GitHasCleanWorkingCopy()) */
         .Executes(() =>
         {
             string tag = $"{ReleaseVersion}{CustomTagSuffix}";
             //FinishReleaseOrHotfix(tag);

             void RunGitHubRelease(string args)
             {
                 ProcessTasks.StartProcess("github-release", args, RootDirectory, customLogger: GitLogger).AssertZeroExitCode();
             }
             var prereleaseArg = PreRelease ? "--pre-release" : "";
             var descriptionArg = File.Exists(ReleaseNotes) ? $"--description \"{File.ReadAllText(ReleaseNotes)}\"" : "";
             RunGitHubRelease($@"release --user {GitHubUser} -s {GitHubToken} --repo {GitRepositoryName} --tag {tag} {descriptionArg} {prereleaseArg}"); //-c branch -d description
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
