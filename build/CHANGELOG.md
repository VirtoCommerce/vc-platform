# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).
## [1.4.16] - 2020-10-12
### Added
- Targets: GetManifestGit, UpdateManifest, PublishManifestGit

## [1.4.15] - 2020-10-06
### Added
- PushChanges parameter
- ModulesJsonRepoUrl parameter
### Fixed
- Search of WebProject
- Git Log filter

## [1.4.7] - 2020-09-17
### Added
- Parameters for Sonar PullRequests Analysis
### Fixed
- Default value for SwaggerValidatorUri
- Parameters for Sonar PullRequests Decoration

## [1.4.3] - 2020-09-07
### Added
- SonarBranchName parameter
### Fixed
- Search of Web-projects
- sonar.branch.name parameter changed to sonar.branch
- NukeSpecificationFiles exclusions

## [1.4.0] - 2020-08-31
### Added
 - GrabMigrator Target

## [1.3.7] - 2020-08-25
### Fixed
- Directory.Build.props search
- Search of Web-projects
- Search of Tests Assemblies and Projects
- Swashbuckle.AspNetCore.Cli dependency resolving
- SonarQubeStart target for using it with SonarCloud

## [1.3.1] - 2020-07-31
### Fixed
- Repository index updating
- Parameters for SonarScanner to use it with new versions of SonaQube

## [1.3.0] - 2020-07-20
### Added
- Targets: StartRelease, CompleteRelease, QuickRelease, StartHotfix, CompleteHotfix, IncrementMinor, IncrementPatch, ChangeVersion
### Changed
- OnTargetStart event handler was replaced with ChangeVersion Target
### Fixed
- Hash and Sources of github releases (VP-3628)

## [1.2.0] - 2020-07-07
### Changed
- Opencover is replaced with Coverlet

## [1.1.1] - 2020-06-26
### Fixed
- An issue when there is no Directory.Build.Props file

## [1.1.0] - 2020-06-25
### Changed
- Parameters VersionTag and CustomTagSuffix were replaced with CustomVersionSuffix

## [1.0.2] - 2020-06-18
### Added
- Support for prereleases in modules manifest
### Fixed
- Updating of modules properties in the manifest

## [1.0.1] - 2020-06-11
### Fixed
- Artifact name of Storefront
- Name of artifacts directory of Modules
### Added
- .nuke file will be created if it doesn't exist and there is solution file in current directory
- ArtifactsDirectory Parameter to customize artifact directory

## [1.0.0] - 2020-06-05
### Fixed
- An issue with Storefront's project search
- Modules version is getting from Project properties now
### Added
- CustomTagSuffix parameter
### Changed
- Updated dependencies

## [3.0.0-beta0010] - 2020-04-24
### Fixed
- An issue with opencover that fails when it runs not on build server.
### Changed
- NUKE Execution Engine updated to 0.24.10
- Removed GitVersion dependency
- The Version is going to be got from Project properties now instead of GitVersion

## [3.0.0-beta0009] - 2020-04-01
### Added
- Custom logger for DotnetTasks
- ValidateSwaggerSchema Target
- Support for Pull Request in SonarQubeStart Target
### Fixed
- An issue with dependencies filter in Compress Target
- An issue with packaging vc-build with 3rd party tools https://github.com/nuke-build/nuke/issues/437
### Changed
- NUKE Execution Engine updated to 0.24.7
- GitVersion updated to 5.2.4
- Virtocommerce.Platform dependency changed from ProjectReference to PackageReference

## [3.0.0-beta0008] - 2020-01-28
### Added
- Code Coverage
### Changed
- Updated dependencies
### Fixed
- Fixed an issues with GitVersion and Nuke

## [3.0.0-beta0007] - 2020-01-13
### Added
- SwaggerValidationUrl parameter
- changelog
### Changed
- Target Framework updated to 3.1
- SwaggerValidation now uses validator.swagger.io
- Nuke.Common dependency updated to 0.23.6
