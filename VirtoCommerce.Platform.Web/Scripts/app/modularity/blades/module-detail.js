﻿angular.module('platformWebApp')
.controller('platformWebApp.moduleDetailController', ['$scope', 'platformWebApp.dialogService', 'platformWebApp.bladeNavigationService', 'platformWebApp.modules', 'platformWebApp.moduleHelper', 'FileUploader', function ($scope, dialogService, bladeNavigationService, modules, moduleHelper, FileUploader) {
    var blade = $scope.blade;

    function initializeBlade() {
        if (blade.currentEntity.isInstalled) {
            blade.toolbarCommands = [
                {
                    name: "platform.commands.update", icon: 'fa fa-upload',
                    executeMethod: function () {
                        blade.currentEntity = blade.currentEntity.$latestModule;
                        initializeBlade();
                    },
                    canExecuteMethod: function () {
                        return blade.currentEntity.$latestModule;
                    },
                    permission: 'platform:module:manage'
                },
                {
                    name: "platform.commands.uninstall", icon: 'fa fa-trash-o',
                    executeMethod: function () {
                        $scope.confirmActionInDialog('uninstall');
                    },
                    canExecuteMethod: function () { return true; },
                    permission: 'platform:module:manage'
                },
                {
                    name: "platform.commands.settings", icon: 'fa fa-wrench',
                    executeMethod: function () {
                        var newBlade = {
                            id: 'moduleSettingsSection',
                            moduleId: blade.currentEntityId,
                            title: 'platform.blades.module-settings-detail.title',
                            //subtitle: '',
                            controller: 'platformWebApp.settingsDetailController',
                            template: '$(Platform)/Scripts/app/settings/blades/settings-detail.tpl.html'
                        };
                        bladeNavigationService.showBlade(newBlade, blade);
                    },
                    canExecuteMethod: function () { return true; }
                }
            ];
        } else {
            blade.toolbarCommands = [];
            blade.mode = blade.currentEntity.installedVersion ? 'update' : 'install';
            $scope.availableVersions = _.where(blade.currentEntity.$all, { isInstalled: false });
        }
    }

    $scope.openDependencyModule = function (dependency) {
        module = _.findWhere(moduleHelper.allmodules, { id: dependency.id, version: dependency.version }) ||
                    _.findWhere(moduleHelper.allmodules, { id: dependency.id }) ||
                     module;
        blade.parentBlade.selectNode(module);
    };

    $scope.confirmActionInDialog = function (action) {
        blade.isLoading = true;

        var clone = angular.copy(blade.currentEntity);
        clone.$all = clone.$latestModule = undefined;
        //var clone = {
        //    id: blade.currentEntity.id,
        //    version: blade.currentEntity.version,
        //    title: blade.currentEntity.title,
        //    moduleName: blade.currentEntity.moduleName,
        //    moduleType: blade.currentEntity.moduleType,
        //    fullPhysicalPath: blade.currentEntity.fullPhysicalPath
        //};

        var selection = [clone];
        var modulesApiMethod = action === 'uninstall' ? modules.getDependents : modules.getDependencies;
        modulesApiMethod(selection, function (data) {
            blade.isLoading = false;
            data = _.filter(data, function (x) { return _.all(selection, function (s) { return s.id !== x.id; }) });

            var dialog = {
                id: "confirmation",
                action: action,
                selection: selection,
                dependencies: data,
                callback: function (remove) {
                    if (remove) {
                        blade.isLoading = true;

                        switch (action) {
                            case 'install':
                            case 'update':
                                modules.install(selection, onAfterSubmitted, function (error) {
                                    bladeNavigationService.setError('Error ' + error.status, blade);
                                });
                                break;
                            case 'uninstall':
                                modules.uninstall(selection, onAfterSubmitted, function (error) {
                                    bladeNavigationService.setError('Error ' + error.status, blade);
                                });
                                break;
                        }
                    }
                }
            }
            dialogService.showDialog(dialog, '$(Platform)/Scripts/app/modularity/dialogs/moduleAction-dialog.tpl.html', 'platformWebApp.confirmDialogController');
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, blade);
        });
    };

    function onAfterSubmitted(data) {
        var newBlade = {
            id: 'moduleInstallProgress',
            currentEntity: data,
            // currentEntityId: data.id,
            title: 'platform.blades.module-wizard-progress-step.title',
            controller: 'platformWebApp.moduleInstallProgressController',
            template: '$(Platform)/Scripts/app/modularity/wizards/newModule/module-wizard-progress-step.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade.parentBlade);
    }

    blade.headIcon = 'fa-cubes';

    if (blade.mode === 'advanced') {
        // the uploader
        var uploader = $scope.uploader = new FileUploader({
            scope: $scope,
            headers: {
                Accept: 'application/json'
            },
            url: 'api/platform/modules',
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
            if (data.tags) {
                data.tagsArray = data.tags.split(' ');
            }
            blade.currentEntity = data;
            blade.mode = undefined;
            initializeBlade();
        };

        uploader.onErrorItem = function (item, response, status, headers) {
            if (item.isUploaded) {
                bladeNavigationService.setError('File uploaded with error status: ' + status, blade);
            } else {
                bladeNavigationService.setError('Failed to upload. Error status: ' + status, blade);
            }
        };
    } else {
        initializeBlade();
    }

    blade.isLoading = false;
}]);
