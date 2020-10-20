using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.CloudFoundry;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Git;
using Nuke.Common.Tools.Npm;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
partial class Build : NukeBuild
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
        if (!nukeFile.Any())
        {
            Logger.Info("No .nuke file found!");
            var solutions = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.sln");
            if (solutions.Length == 1)
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

    [Parameter] static string GlobalModuleIgnoreFileUrl = @"https://raw.githubusercontent.com/VirtoCommerce/vc-platform/dev/module.ignore";

    [Parameter] readonly string SonarAuthToken = "";
    [Parameter] readonly string SonarUrl = "https://sonarcloud.io";
    [Parameter] readonly AbsolutePath CoverageReportPath = RootDirectory / ".tmp" / "coverage.xml";
    [Parameter] readonly string TestsFilter = "Category!=IntegrationTest";

    [Parameter("Url to Swagger Validation Api")] readonly string SwaggerValidatorUri = "https://validator.swagger.io/validator/debug";

    [Parameter("GitHub user for release creation")] readonly string GitHubUser;
    [Parameter("GitHub user security token for release creation")] readonly string GitHubToken;
    [Parameter("True - prerelease, False - release")] readonly bool PreRelease;
    [Parameter("True - Pull Request")] readonly bool PullRequest;

    [Parameter("Path to folder with  git clones of modules repositories")] readonly AbsolutePath ModulesFolderPath;
    [Parameter("Repo Organization/User")] readonly string RepoOrg = "VirtoCommerce";
    [Parameter("Repo Name")] string RepoName;

    [Parameter("Sonar Organization (\"virto-commerce\" by default)")] readonly string SonarOrg = "virto-commerce";

    [Parameter("Path to nuget config")] readonly AbsolutePath NugetConfig;

    [Parameter("Swagger schema path")] readonly AbsolutePath SwaggerSchemaPath;

    [Parameter("Path to modules.json")] readonly string ModulesJsonName = "modules_v3.json";
    [Parameter("Full uri to module artifact")] readonly string CustomModulePackageUri;

    [Parameter("Path to Release Notes File")] readonly AbsolutePath ReleaseNotes;

    [Parameter("VersionTag for module.manifest and Directory.Build.props")] string CustomVersionPrefix;
    [Parameter("VersionSuffix for module.manifest and Directory.Build.props")] string CustomVersionSuffix;

    [Parameter("Release branch")] readonly string ReleaseBranch;

    [Parameter("Branch Name for SonarQube")] readonly string SonarBranchName;

    [Parameter("PR Base for SonarQube")] readonly string SonarPRBase;
    [Parameter("PR Branch for SonarQube")] readonly string SonarPRBranch;
    [Parameter("PR Number for SonarQube")] readonly string SonarPRNumber;
    [Parameter("Github Repository for SonarQube")] readonly string SonarGithubRepo;
    [Parameter("PR Provider for SonarQube")] readonly string SonarPRProvider;

    [Parameter("Modules.json repo url")] readonly string ModulesJsonRepoUrl = "https://github.com/VirtoCommerce/vc-modules.git";

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    [Parameter("Path to Artifacts Directory")] AbsolutePath ArtifactsDirectory = RootDirectory / "artifacts";

    [Parameter("Directory containing modules.json")] string ModulesJsonDirectoryName = "vc-modules";
    AbsolutePath ModulesLocalDirectory => ArtifactsDirectory / ModulesJsonDirectoryName;
    Project WebProject => Solution.AllProjects.FirstOrDefault(x => (x.Name.EndsWith(".Web") && !x.Path.ToString().Contains("samples")) || x.Name.EndsWith("VirtoCommerce.Storefront"));
    AbsolutePath ModuleManifestFile => WebProject.Directory / "module.manifest";
    AbsolutePath ModuleIgnoreFile => RootDirectory / "module.ignore";

    Microsoft.Build.Evaluation.Project MSBuildProject => WebProject.GetMSBuildProject();
    string VersionPrefix => MSBuildProject.GetProperty("VersionPrefix")?.EvaluatedValue;
    string VersionSuffix => MSBuildProject.GetProperty("VersionSuffix")?.EvaluatedValue;
    string ReleaseVersion => MSBuildProject.GetProperty("PackageVersion")?.EvaluatedValue ?? WebProject.GetProperty("Version");

    ModuleManifest ModuleManifest => ManifestReader.Read(ModuleManifestFile);

    AbsolutePath ModuleOutputDirectory => ArtifactsDirectory / ModuleManifest.Id;

    AbsolutePath DirectoryBuildPropsPath => Solution.Directory / "Directory.Build.props";

    string ZipFileName => IsModule ? $"{ModuleManifest.Id}_{ReleaseVersion}.zip" : $"{WebProject.Solution.Name}.{ReleaseVersion}.zip";
    string ZipFilePath => ArtifactsDirectory / ZipFileName;
    string GitRepositoryName => GitRepository.Identifier.Split('/')[1];

    string ModulePackageUrl => CustomModulePackageUri.IsNullOrEmpty() ?
        $"https://github.com/VirtoCommerce/{GitRepositoryName}/releases/download/{ReleaseVersion}/{ModuleManifest.Id}_{ReleaseVersion}.zip" : CustomModulePackageUri;
    GitRepository ModulesRepository => GitRepository.FromUrl(ModulesJsonRepoUrl);

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
          //For platform take nuget package description from Directory.Build.props
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
           //
           var OutPath = RootDirectory / ".tmp";
           testProjects.ForEach((testProject, index) =>
           {
               var testSetting = new DotNetTestSettings()
                .SetProjectFile(testProject.Path)
                .SetConfiguration(Configuration)
                .SetLogger("trx")
                .SetFilter(TestsFilter)
                .SetNoBuild(true)
                .SetCollectCoverage(true)
                .SetLogOutput(true)
                .SetResultsDirectory(OutPath);
               var testProjectBinDir = testProject.Directory / "bin";
               var testAssemblies = testProjectBinDir.GlobFiles($"**/{testProject.Name}.dll");
               if(testAssemblies.Count < 1)
               {
                   ControlFlow.Fail("Tests Assemblies not found!");
               }
               CoverletTasks.Coverlet(s => s
                .SetTargetSettings(testSetting)
                .SetAssembly(testAssemblies.First())
                .SetTarget(dotnetPath)
                .When(index == 0, ss => ss.SetOutput(CoverageReportPath))
                .When(index > 0 && index < testProjects.Count() - 1, ss => ss.SetMergeWith(CoverageReportPath))
                .When(index == testProjects.Count() - 1, ss => ss.SetOutput(CoverageReportPath).SetFormat(CoverletOutputFormat.opencover))
                );

           });
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
                    .SetSkipDuplicate(true)
                    .CombineWith(
                        packages, (cs, v) => cs
                            .SetTargetPath(v)),
                degreeOfParallelism: 5,
                completeOnFailure: true);
        });

    public class Utf8StringWriter : StringWriter
    {
        // Use UTF8 encoding but write no BOM to the wire
        public override Encoding Encoding => new UTF8Encoding(false);
    }

    public void ChangeProjectVersion(string prefix = null, string suffix = null)
    {
        //module.manifest
        if (IsModule)
        {
            var manifest = ModuleManifest.Clone();
            if (!String.IsNullOrEmpty(prefix))
                manifest.Version = prefix;
            if (!String.IsNullOrEmpty(suffix))
                manifest.VersionTag = suffix;
            using (var writer = new Utf8StringWriter())
            {
                XmlSerializer xml = new XmlSerializer(typeof(ModuleManifest));
                xml.Serialize(writer, manifest);
                File.WriteAllText(ModuleManifestFile, writer.ToString(), Encoding.UTF8);
            }
        }

        //Directory.Build.props
        var xmlDoc = new XmlDocument()
        {
            PreserveWhitespace = true
        };
        xmlDoc.LoadXml(File.ReadAllText(DirectoryBuildPropsPath));
        if (!String.IsNullOrEmpty(prefix))
        {
            var prefixNodex = xmlDoc.GetElementsByTagName("VersionPrefix");
            prefixNodex[0].InnerText = prefix;
        }
        if (String.IsNullOrEmpty(VersionSuffix) && !String.IsNullOrEmpty(suffix))
        {
            var suffixNodes = xmlDoc.GetElementsByTagName("VersionSuffix");
            suffixNodes[0].InnerText = suffix;
        }
        using (var writer = new Utf8StringWriter())
        {
            xmlDoc.Save(writer);
            File.WriteAllText(DirectoryBuildPropsPath, writer.ToString());
        }
    }

    Target ChangeVersion => _ => _
        .Requires(() => !CustomVersionPrefix.IsNullOrEmpty() || !CustomVersionSuffix.IsNullOrEmpty())
        .Executes(() =>
        {
            if ((String.IsNullOrEmpty(VersionSuffix) && !CustomVersionSuffix.IsNullOrEmpty()) || !CustomVersionPrefix.IsNullOrEmpty())
            {
                ChangeProjectVersion(prefix: CustomVersionPrefix, suffix: CustomVersionSuffix);
            }
        });

    Target StartRelease => _ => _
        .Executes(() =>
        {
            var currentDir = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(Solution.Path.Parent);
            GitTasks.Git("checkout dev");
            GitTasks.Git("pull");
            var releaseBranchName = $"release/{ReleaseVersion}";
            Logger.Info(Directory.GetCurrentDirectory());
            GitTasks.Git($"checkout -b {releaseBranchName}");
            GitTasks.Git($"push -u origin {releaseBranchName}");
            Directory.SetCurrentDirectory(currentDir);
        });

    Target CompleteRelease => _ => _
        .After(StartRelease)
        .Executes(() =>
        {
            //workaround for run from sources
            var currentDir = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(Solution.Path.Parent);
            var currentBranch = GitTasks.GitCurrentBranch();
            //Master
            GitTasks.Git("checkout master");
            GitTasks.Git("pull");
            GitTasks.Git($"merge {currentBranch}");
            GitTasks.Git("push origin master");
            //Dev
            GitTasks.Git("checkout dev");
            GitTasks.Git($"merge {currentBranch}");
            IncrementVersionMinor();
            ChangeProjectVersion(prefix: CustomVersionPrefix);
            var manifestArg = IsModule ? RootDirectory.GetRelativePathTo(ModuleManifestFile) : "";
            GitTasks.Git($"add Directory.Build.props {manifestArg}");
            GitTasks.Git($"commit -m \"{CustomVersionPrefix}\"");
            GitTasks.Git($"push origin dev");
            //remove release branch
            GitTasks.Git($"branch -d {currentBranch}");
            GitTasks.Git($"push origin --delete {currentBranch}");
            Directory.SetCurrentDirectory(currentDir);
        });

    Target QuickRelease => _ => _
        .DependsOn(StartRelease, CompleteRelease);

    Target StartHotfix => _ => _
        .Executes(() =>
        {
            var currentDir = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(Solution.Path.Parent);
            GitTasks.Git("checkout master");
            GitTasks.Git("pull");
            IncrementVersionPatch();
            var hotfixBranchName = $"hotfix/{CustomVersionPrefix}";
            Logger.Info(Directory.GetCurrentDirectory());
            GitTasks.Git($"checkout -b {hotfixBranchName}");
            ChangeProjectVersion(prefix: CustomVersionPrefix);
            var manifestArg = IsModule ? RootDirectory.GetRelativePathTo(ModuleManifestFile) : "";
            GitTasks.Git($"add Directory.Build.props {manifestArg}");
            GitTasks.Git($"commit -m \"{CustomVersionPrefix}\"");
            GitTasks.Git($"push -u origin {hotfixBranchName}");
            Directory.SetCurrentDirectory(currentDir);
        });

    Target CompleteHotfix => _ => _
        .After(StartHotfix)
        .Executes(() =>
        {
            //workaround for run from sources
            var currentDir = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(Solution.Path.Parent);
            var currentBranch = GitTasks.GitCurrentBranch();
            //Master
            GitTasks.Git("checkout master");
            GitTasks.Git($"merge {currentBranch}");
            GitTasks.Git($"tag {VersionPrefix}");
            GitTasks.Git("push origin master");
            //remove hotfix branch
            GitTasks.Git($"branch -d {currentBranch}");
            GitTasks.Git($"push origin --delete {currentBranch}");
            Directory.SetCurrentDirectory(currentDir);
        });

    public void IncrementVersionMinor()
    {
        Version v = new Version(VersionPrefix);
        var newPrefix = $"{v.Major}.{v.Minor + 1}.{v.Build}";
        CustomVersionPrefix = newPrefix;
    }

    public void IncrementVersionPatch()
    {
        Version v = new Version(VersionPrefix);
        var newPrefix = $"{v.Major}.{v.Minor}.{v.Build + 1}";
        CustomVersionPrefix = newPrefix;
    }

    Target IncrementMinor => _ => _
        .Triggers(ChangeVersion)
        .Executes(() =>
        {
            IncrementVersionMinor();
        });

    Target IncremenPatch => _ => _
        .Triggers(ChangeVersion)
        .Executes(() =>
        {
            IncrementVersionPatch();
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
               .SetConfiguration(Configuration));

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

    Target GetManifestGit => _ => _
        .Before(UpdateManifest)
        .Executes(() =>
        {
            GitTasks.GitLogger = GitLogger;
            var modulesJsonFile = ModulesLocalDirectory / ModulesJsonName;
            if (!DirectoryExists(ModulesLocalDirectory))
            {
                GitTasks.Git($"clone {ModulesRepository.HttpsUrl} {ModulesLocalDirectory}");
            }
            else
            {
                GitTasks.Git($"pull", ModulesLocalDirectory);
            }
        });

    Target UpdateManifest => _ => _
        .Before(PublishManifestGit)
        .After(GetManifestGit)
        .Executes(() =>
        {
            var modulesJsonFile = ModulesLocalDirectory / ModulesJsonName;
            var manifest = ModuleManifest;

            var modulesExternalManifests = JsonConvert.DeserializeObject<List<ExternalModuleManifest>>(TextTasks.ReadAllText(modulesJsonFile));
            manifest.PackageUrl = ModulePackageUrl;
            var existExternalManifest = modulesExternalManifests.FirstOrDefault(x => x.Id == manifest.Id);
            if (existExternalManifest != null)
            {
                if (!manifest.VersionTag.IsNullOrEmpty() || !CustomVersionSuffix.IsNullOrEmpty())
                {
                    var tag = manifest.VersionTag.IsNullOrEmpty() ? CustomVersionSuffix : manifest.VersionTag;
                    manifest.VersionTag = tag;
                    var existPrereleaseVersions = existExternalManifest.Versions.Where(v => !v.VersionTag.IsNullOrEmpty());
                    if (existPrereleaseVersions.Any())
                    {
                        var prereleaseVersion = existPrereleaseVersions.First();
                        prereleaseVersion.Dependencies = manifest.Dependencies;
                        prereleaseVersion.Incompatibilities = manifest.Incompatibilities;
                        prereleaseVersion.PlatformVersion = manifest.PlatformVersion;
                        prereleaseVersion.ReleaseNotes = manifest.ReleaseNotes;
                        prereleaseVersion.Version = manifest.Version;
                        prereleaseVersion.VersionTag = manifest.VersionTag;
                        prereleaseVersion.PackageUrl = manifest.PackageUrl;
                    }
                    else
                    {
                        existExternalManifest.Versions.Add(ExternalModuleManifestVersion.FromManifest(manifest));
                    }
                }
                else
                {
                    existExternalManifest.PublishNewVersion(manifest);
                }
                existExternalManifest.Title = manifest.Title;
                existExternalManifest.Description = manifest.Description;
                existExternalManifest.Authors = manifest.Authors;
                existExternalManifest.Copyright = manifest.Copyright;
                existExternalManifest.Groups = manifest.Groups;
                existExternalManifest.IconUrl = manifest.IconUrl;
                existExternalManifest.Id = manifest.Id;
                existExternalManifest.LicenseUrl = manifest.LicenseUrl;
                existExternalManifest.Owners = manifest.Owners;
                existExternalManifest.ProjectUrl = manifest.ProjectUrl;
                existExternalManifest.RequireLicenseAcceptance = manifest.RequireLicenseAcceptance;
                existExternalManifest.Tags = manifest.Tags;
            }
            else
            {
                modulesExternalManifests.Add(ExternalModuleManifest.FromManifest(manifest));
            }
            TextTasks.WriteAllText(modulesJsonFile, JsonConvert.SerializeObject(modulesExternalManifests, Newtonsoft.Json.Formatting.Indented));
        });

    Target PublishManifestGit => _ => _
        .After(UpdateManifest)
        .Executes(() =>
        {
            GitTasks.GitLogger = GitLogger;
            GitTasks.Git($"commit -am \"{ModuleManifest.Id} {ReleaseVersion}\"", ModulesLocalDirectory);
            GitTasks.Git($"push origin HEAD:master -f", ModulesLocalDirectory);
        });

    Target PublishModuleManifest => _ => _
        .DependsOn(GetManifestGit, UpdateManifest, PublishManifestGit);

    Target SwaggerValidation => _ => _
          .DependsOn(Publish)
          .Requires(() => !IsModule)
          .Executes(async () =>
          {
              var swashbuckle = ToolResolver.GetPackageTool("Swashbuckle.AspNetCore.Cli", "dotnet-swagger.dll", framework:"netcoreapp3.0");
              var projectPublishPath = ArtifactsDirectory / "publish" / $"{WebProject.Name}.dll";
              var swaggerJson = ArtifactsDirectory / "swagger.json";
              var currentDir = Directory.GetCurrentDirectory();
              Directory.SetCurrentDirectory(RootDirectory / "src" / "VirtoCommerce.Platform.Web");
              swashbuckle.Invoke($"tofile --output {swaggerJson} {projectPublishPath} VirtoCommerce.Platform");
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
            var branchName = String.IsNullOrEmpty(SonarBranchName) ? GitRepository.Branch : SonarBranchName;
            Logger.Info($"BRANCH_NAME = {branchName}");
            var projectName = Solution.Name;
            var prBaseParam = "";
            var prBranchParam = "";
            var prKeyParam = "";
            var ghRepoArg = "";
            var prProviderArg = "";
            var prBase = "";
            if (PullRequest)
            {
                prBase = String.IsNullOrEmpty(SonarPRBase) ? Environment.GetEnvironmentVariable("CHANGE_TARGET") : SonarPRBase;
                prBaseParam = $"/d:sonar.pullrequest.base=\"{prBase}\"";

                var changeTitle = String.IsNullOrEmpty(SonarPRBranch) ? Environment.GetEnvironmentVariable("CHANGE_TITLE") : SonarPRBranch;
                prBranchParam = $"/d:sonar.pullrequest.branch=\"{changeTitle}\"";

                var prNumber = String.IsNullOrEmpty(SonarPRNumber) ? Environment.GetEnvironmentVariable("CHANGE_ID") : SonarPRNumber;
                prKeyParam = $"/d:sonar.pullrequest.key={prNumber}";

                ghRepoArg = String.IsNullOrEmpty(SonarGithubRepo) ? "" : $"/d:sonar.pullrequest.github.repository={SonarGithubRepo}";

                prProviderArg = String.IsNullOrEmpty(SonarPRProvider) ? "" : $"/d:sonar.pullrequest.provider={SonarPRProvider}";

            }
            var baseBranch = PullRequest ? prBase : branchName;
            var branchParam = $"/d:\"sonar.branch={baseBranch}\"";

            var projectNameParam = $"/n:\"{RepoName}\"";
            var projectKeyParam = $"/k:\"{RepoOrg}_{RepoName}\"";
            var projectVersionParam = $"/v:\"{ReleaseVersion}\"";
            var hostParam = $"/d:sonar.host.url={SonarUrl}";
            var tokenParam = $"/d:sonar.login={SonarAuthToken}";
            var sonarReportPathParam = $"/d:sonar.cs.opencover.reportsPaths={CoverageReportPath}";
            var orgParam = $"/o:{SonarOrg}";

            var startCmd = $"sonarscanner begin {orgParam} {branchParam} {projectKeyParam} {projectNameParam} {projectVersionParam} {hostParam} {tokenParam} {sonarReportPathParam} {prBaseParam} {prBranchParam} {prKeyParam} {ghRepoArg} {prProviderArg}";

            Logger.Normal($"Execute: {startCmd.Replace(SonarAuthToken, "{IS HIDDEN}")}");

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
        if (text.Contains("github returned 422 Unprocessable Entity") && text.Contains("already_exists"))
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
             string tag = ReleaseVersion;
             //FinishReleaseOrHotfix(tag);

             void RunGitHubRelease(string args)
             {
                 ProcessTasks.StartProcess("github-release", args, RootDirectory, customLogger: GitLogger).AssertZeroExitCode();
             }
             var prereleaseArg = PreRelease ? "--pre-release" : "";
             var targetBranchArg = ReleaseBranch.IsNullOrEmpty() ? "" : $"--target \"{ReleaseBranch}\"";
             var descriptionArg = File.Exists(ReleaseNotes) ? $"--description \"{File.ReadAllText(ReleaseNotes)}\"" : "";
             RunGitHubRelease($@"release --user {GitHubUser} -s {GitHubToken} --repo {GitRepositoryName} {targetBranchArg} --tag {tag} {descriptionArg} {prereleaseArg}"); //-c branch -d description
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
