# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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
