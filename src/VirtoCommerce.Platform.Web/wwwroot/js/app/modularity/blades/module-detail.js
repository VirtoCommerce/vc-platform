angular.module('platformWebApp')
.controller('platformWebApp.moduleDetailController', ['$scope', 'platformWebApp.dialogService', 'platformWebApp.bladeNavigationService', 'platformWebApp.modules', 'platformWebApp.moduleHelper', 'FileUploader', 'platformWebApp.settings', function ($scope, dialogService, bladeNavigationService, modules, moduleHelper, FileUploader, settings) {
    var blade = $scope.blade;
    blade.allowInstallModules = window.allowInstallModules;

    function initializeBlade() {        
        if (blade.currentEntity.isInstalled) {
            var canUpdate = _.any(moduleHelper.allmodules, function (x) {
                return x.id === blade.currentEntity.id && !x.isInstalled;
            });
            if (window.allowInstallModules) {

                blade.toolbarCommands = [
                    {
                        name: "platform.commands.update", icon: 'fa fa-upload',
                        executeMethod: function () {
                            blade.currentEntity = _.last(_.where(moduleHelper.allmodules, { id: blade.currentEntity.id, isInstalled: false }));
                            initializeBlade();
                        },
                        canExecuteMethod: function () { return canUpdate; },
                        permission: 'platform:module:manage'
                    },
                    {
                        name: "platform.commands.uninstall", icon: 'fa fa-trash-o',
                        executeMethod: function () {
                            $scope.confirmActionInDialog('uninstall');
                        },
                        canExecuteMethod: function () { return true; },
                        permission: 'platform:module:manage'
                    }
                ];
            }
            else {
                blade.toolbarCommands = [];
            }

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
            blade.mode = blade.currentEntity.$alternativeVersion ? 'update' : 'install';
            $scope.availableVersions = _.where(moduleHelper.allmodules, { id: blade.currentEntity.id, isInstalled: false });
            blade.isLoading = false;
        }
    }

    $scope.formDependencyVersion = function (dependency) {
        return dependency.version.major + '.' + dependency.version.minor + '.' + dependency.version.patch;
    };

    $scope.openDependencyModule = function (dependency) {
        module = _.findWhere(moduleHelper.allmodules, { id: dependency.id, version: dependency.version }) ||
                    _.findWhere(moduleHelper.allmodules, { id: dependency.id }) ||
                     module;
        blade.parentBlade.selectNode(module);
    };

    $scope.confirmActionInDialog = function (action) {
        blade.isLoading = true;

        //var clone = {
        //    id: blade.currentEntity.id,
        //    version: blade.currentEntity.version,
        //};
        var selection = [blade.currentEntity];
        var modulesApiMethod = action === 'uninstall' ? modules.getDependents : modules.getDependencies;
        modulesApiMethod(selection, function (data) {
            blade.isLoading = false;

            var dialog = {
                id: "confirmation",
                action: action,
                selection: selection,
                dependencies: data,
                callback: function () {
                    // initiate module (un)installation
                    blade.isLoading = true;
                    _.each(selection, function (x) {
                        if (!_.findWhere(data, { id: x.id })) {
                            data.push(x);
                        }
                    });

                    switch (action) {
                        case 'install':
                        case 'update':
                            modulesApiMethod = modules.install;
                            break;
                        case 'uninstall':
                            modulesApiMethod = modules.uninstall;
                            break;
                    }
                    modulesApiMethod(data, function (data) {
                        // show module (un)installation progress
                        var newBlade = {
                            id: 'moduleInstallProgress',
                            currentEntity: data,
                            controller: 'platformWebApp.moduleInstallProgressController',
                            template: '$(Platform)/Scripts/app/modularity/wizards/newModule/module-wizard-progress-step.tpl.html'
                        };
                        switch (action) {
                            case 'install':
                                _.extend(newBlade, { title: 'platform.blades.module-wizard-progress-step.title-install' });
                                break;
                            case 'update':
                                _.extend(newBlade, { title: 'platform.blades.module-wizard-progress-step.title-update' });
                                break;
                            case 'uninstall':
                                _.extend(newBlade, { title: 'platform.blades.module-wizard-progress-step.title-uninstall' });
                                break;
                        }
                        bladeNavigationService.showBlade(newBlade, blade.parentBlade);
                    }, function (error) {
                        bladeNavigationService.setError('Error ' + error.status, blade);
                    });
                }
            }
            dialogService.showDialog(dialog, '$(Platform)/Scripts/app/modularity/dialogs/moduleAction-dialog.tpl.html', 'platformWebApp.confirmDialogController');
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, blade);
        });
    };

    blade.headIcon = 'fa-cubes';

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
