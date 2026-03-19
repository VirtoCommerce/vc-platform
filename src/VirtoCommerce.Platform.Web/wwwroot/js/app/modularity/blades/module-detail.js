angular.module('platformWebApp')
.controller('platformWebApp.moduleDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.moduleHelper', 'FileUploader', 'platformWebApp.settings', function ($scope, bladeNavigationService, moduleHelper, FileUploader, settings) {
    var blade = $scope.blade;

    blade.headIcon = 'fa fa-cubes';
    blade.fallbackIconUrl = '/images/module-logo.png';

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
            blade.isLoading = false;
        }
    }

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
        var selection = [blade.currentEntity];
        moduleHelper.performAction(action, selection, blade, true);
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
