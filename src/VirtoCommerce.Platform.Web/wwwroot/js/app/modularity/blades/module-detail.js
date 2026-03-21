angular.module('platformWebApp')
.controller('platformWebApp.moduleDetailController', ['$scope', '$timeout', 'platformWebApp.bladeNavigationService', 'platformWebApp.moduleHelper', 'platformWebApp.modulesApi', 'FileUploader', 'platformWebApp.settings', function ($scope, $timeout, bladeNavigationService, moduleHelper, modulesApi, FileUploader, settings) {
    var blade = $scope.blade;

    blade.headIcon = 'fa fa-cubes';
    blade.fallbackIconUrl = '/images/module-logo.png';

    // Custom version state
    $scope.useCustomVersion = false;
    $scope.customVersionData = { version: '' };
    $scope.customVersionState = null; // null | 'checking' | 'valid' | 'invalid'
    var validateTimer = null;
    var originalEntity = null;
    // Semantic version regex: major.minor.patch with optional pre-release tag (e.g. 3.1.0, 3.1.0-alpha.1, 3.1.0-beta, 3.1.0-rc.2)
    var semverRegex = /^\d+\.\d+\.\d+(-[a-zA-Z0-9]+(\.[a-zA-Z0-9]+)*)?$/;

    function initializeBlade() {
        if (blade.currentEntity.isInstalled) {
            var canUpdate = $scope.allowInstallModules && _.any(moduleHelper.modules, function (x) {
                return x.id === blade.currentEntity.id && !x.isInstalled;
            });

            blade.toolbarCommands = [
                {
                    name: "platform.commands.update", icon: 'fa fa-upload',
                    executeMethod: function () {
                        blade.currentEntity = _.last(_.where(moduleHelper.modules, { id: blade.currentEntity.id, isInstalled: false }));
                        initializeBlade();
                    },
                    canExecuteMethod: function () { return canUpdate; },
                    permission: 'platform:module:manage'
                },
                {
                    name: "platform.commands.uninstall", icon: 'fas fa-trash-alt',
                    executeMethod: function () {
                        $scope.confirmActionInDialog('uninstall');
                    },
                    canExecuteMethod: function () { return $scope.allowInstallModules; },
                    permission: 'platform:module:manage'
                }
            ];

            // hide settings toolbar button when there are no settings available #523
            settings.getSettings({ id: blade.currentEntity.id }, function (results) {
                if (_.any(results)) {
                    blade.toolbarCommands.push({
                        name: "platform.commands.settings", icon: 'fa fa-wrench',
                        executeMethod: function () {
                            var newBlade = {
                                id: 'moduleSettingsSection',
                                moduleId: blade.currentEntity.id,
                                data: results,
                                title: 'platform.blades.module-settings-detail.title',
                                //subtitle: '',
                                controller: 'platformWebApp.settingsDetailController',
                                template: '$(Platform)/Scripts/app/settings/blades/settings-detail.tpl.html'
                            };
                            bladeNavigationService.showBlade(newBlade, blade);
                        },
                        canExecuteMethod: function () { return true; }
                    });
                }
                blade.isLoading = false;
            });
        } else {
            blade.toolbarCommands = [];
            blade.mode = blade.currentEntity.$installedVersion ? 'update' : 'install';
            $scope.availableVersions = _.where(moduleHelper.modules, { id: blade.currentEntity.id, isInstalled: false });
            originalEntity = blade.currentEntity;
            blade.isLoading = false;
        }
    }

    $scope.toggleCustomVersion = function () {
        $scope.useCustomVersion = !$scope.useCustomVersion;
        if ($scope.useCustomVersion) {
            originalEntity = blade.currentEntity;
        } else {
            blade.currentEntity = originalEntity;
        }
        $scope.customVersionData.version = '';
        $scope.customVersionState = null;
    };

    $scope.onCustomVersionChanged = function () {
        if (validateTimer) {
            $timeout.cancel(validateTimer);
        }

        $scope.customVersionState = null;

        var version = ($scope.customVersionData.version || '').trim();
        if (!version) {
            return;
        }

        if (!semverRegex.test(version)) {
            $scope.customVersionState = 'invalid';
            return;
        }

        $scope.customVersionState = 'checking';
        validateTimer = $timeout(function () {
            modulesApi.validateVersion(
                { id: originalEntity.id, version: version },
                function (isValid) {
                    $scope.customVersionState = isValid ? 'valid' : 'invalid';
                }
            );
        }, 500);
    };

    $scope.canInstallCustomVersion = function () {
        return !$scope.useCustomVersion || $scope.customVersionState === 'valid';
    };

    $scope.hasOptionalDependencies = function (dependencies) {
        var result = _.find(dependencies, function (x) {
            return x.optional;
        });

        return result;
    }

    $scope.isModulePresent = function (dependencyId) {
        return _.any(moduleHelper.modules, function (x) {
            return x.id === dependencyId;
        });
    }

    $scope.formDependencyVersion = function (dependency) {
        return dependency.version.major + '.' + dependency.version.minor + '.' + dependency.version.patch;
    };

    $scope.openDependencyModule = function (dependency) {
        module = _.findWhere(moduleHelper.modules, { id: dependency.id, version: dependency.version }) ||
                    _.findWhere(moduleHelper.modules, { id: dependency.id }) ||
                     module;
        blade.parentBlade.selectNode(module);
    };

    $scope.confirmActionInDialog = function (action) {
        if ($scope.useCustomVersion && $scope.customVersionState === 'valid') {
            blade.isLoading = true;
            modulesApi.installVersion(
                { id: originalEntity.id, version: $scope.customVersionData.version },
                {}, // empty POST body; first arg is URL params
                function (progressData) {
                    blade.isLoading = false;
                    var newBlade = {
                        id: 'moduleInstallProgress',
                        currentEntity: progressData,
                        controller: 'platformWebApp.moduleInstallProgressController',
                        template: '$(Platform)/Scripts/app/modularity/wizards/newModule/module-wizard-progress-step.tpl.html',
                        title: 'platform.blades.module-wizard-progress-step.title-install'
                    };
                    bladeNavigationService.showBlade(newBlade, blade.parentBlade);
                },
                function (error) {
                    blade.isLoading = false;
                    bladeNavigationService.setError('Error ' + error.status, blade);
                }
            );
        } else {
            var selection = [blade.currentEntity];
            moduleHelper.performAction(action, selection, blade, true);
        }
    };

    if (blade.mode === 'advanced') {
        // the uploader
        var uploader = $scope.uploader = new FileUploader({
            scope: $scope,
            url: 'api/platform/modules/localstorage',
            autoUpload: true,
            removeAfterUpload: true
        });

        // ADD FILTERS: packages only
        uploader.filters.push({
            name: 'packageFilter',
            fn: function (i /*{File|FileLikeObject}*/, options) {
                return i.name.endsWith('.zip');
            }
        });

        uploader.onAfterAddingFile = function (item) {
            bladeNavigationService.setError(null, blade);
            blade.isLoading = true;
        };

        uploader.onCompleteAll = function () {
            blade.isLoading = false;
        };

        uploader.onSuccessItem = function (fileItem, data, status, headers) {
            if (data) {
                if (data.tags) {
                    data.tagsArray = data.tags.split(' ');
                }
                blade.currentEntity = data;
                blade.mode = undefined;
                initializeBlade();
            } else {
                bladeNavigationService.setError('Invalid module package: ' + fileItem.file.name, blade);
            }
        };

        uploader.onErrorItem = function (item, response, status, headers) {
            if (item.isUploaded) {
                bladeNavigationService.setError('File uploaded with error status: ' + status, blade);
            } else {
                bladeNavigationService.setError('Failed to upload. Error status: ' + status, blade);
            }
        };

        blade.isLoading = false;
    } else {
        initializeBlade();
    }
}]);
