angular.module('platformWebApp')
.controller('platformWebApp.moduleDetailController', ['$scope', 'platformWebApp.dialogService', 'platformWebApp.bladeNavigationService', 'platformWebApp.modules', function ($scope, dialogService, bladeNavigationService, modules) {
    var blade = $scope.blade;

    blade.refresh = function () {
        modules.get({ id: blade.currentEntityId }, function (data) {
            initializeBlade(data);
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    }

    function initializeBlade(data) {
        if (data.tags) {
            data.tags = data.tags.split(' ');
        }
        blade.currentEntity = data;
        blade.isLoading = false;
    };

    $scope.openModule = function (module) {
        blade.parentBlade.selectItem(module);
    }

    $scope.blade.toolbarCommands = [
        {
            name: "Update", icon: 'fa fa-upload',
            executeMethod: function () {
                openUpdateEntityBlade();
            },
            canExecuteMethod: function () {
                return blade.currentEntity && blade.currentEntity.isRemovable;
            },
            permission: 'platform:module:manage'
        },
        {
            name: "Uninstall", icon: 'fa fa-trash-o',
            executeMethod: function () {
                openDeleteEntityBlade();
            },
            canExecuteMethod: function () {
                return blade.currentEntity && blade.currentEntity.isRemovable;
            },
            permission: 'platform:module:manage'
        },
        {
            name: "Settings", icon: 'fa fa-wrench',
            executeMethod: function () {
                var newBlade = {
                    id: 'moduleSettingsSection',
                    moduleId: blade.currentEntityId,
                    title: 'Module settings',
                    //subtitle: '',
                    controller: 'platformWebApp.settingsDetailController',
                    template: 'Scripts/app/settings/blades/settings-detail.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, blade);
            },
            canExecuteMethod: function () {
                return true;
            }
        }
    ];

    function openUpdateEntityBlade() {
        var newBlade = {
            id: "moduleWizard",
            title: "Module update",
            // subtitle: '',
            mode: 'update',
            controller: 'platformWebApp.installWizardController',
            template: 'Scripts/app/packaging/blades/module-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    function openDeleteEntityBlade() {
        var dialog = {
            id: "confirmDelete",
            title: "Delete confirmation",
            message: "Are you sure you want to uninstall this Module?",
            callback: function (remove) {
                if (remove) {
                    blade.isLoading = true;

                    modules.uninstall({ id: blade.currentEntity.id }, function (data) {
                        var newBlade = {
                            id: 'moduleInstallProgress',
                            currentEntityId: data.id,
                            title: 'module uninstall',
                            subtitle: 'Installation progress',
                            controller: 'platformWebApp.moduleInstallProgressController',
                            template: 'Scripts/app/packaging/wizards/newModule/module-wizard-progress-step.tpl.html'
                        };
                        bladeNavigationService.showBlade(newBlade, blade.parentBlade);
                    }, function (error) {
                        bladeNavigationService.setError('Error ' + error.status, blade);
                    });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    $scope.blade.headIcon = 'fa fa-cubes';

    // on load
    blade.refresh();
}]);
