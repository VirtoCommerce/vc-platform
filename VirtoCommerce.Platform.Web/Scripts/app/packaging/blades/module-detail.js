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

    $scope.openModule = function (id) {
        blade.parentBlade.selectNode(id);
    }

    $scope.blade.toolbarCommands = [
        {
            name: "platform.commands.update", icon: 'fa fa-upload',
            executeMethod: function () {
                openUpdateEntityBlade();
            },
            canExecuteMethod: function () {
                return blade.currentEntity && blade.currentEntity.isRemovable;
            },
            permission: 'platform:module:manage'
        },
        {
            name: "platform.commands.uninstall", icon: 'fa fa-trash-o',
            executeMethod: function () {
                openDeleteEntityBlade();
            },
            canExecuteMethod: function () {
                return blade.currentEntity && blade.currentEntity.isRemovable;
            },
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
            canExecuteMethod: function () {
                return true;
            }
        }
    ];

    function openUpdateEntityBlade() {
        var newBlade = {
            id: "moduleWizard",
            title: "platform.blades.module-detail.title-update",
            // subtitle: '',
            mode: 'update',
            controller: 'platformWebApp.installWizardController',
            template: '$(Platform)/Scripts/app/packaging/blades/module-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    function openDeleteEntityBlade() {
        var dialog = {
            id: "confirmDelete",
            title: "platform.dialogs.module-delete.title",
            message: "platform.dialogs.module-delete.message",
            callback: function (remove) {
                if (remove) {
                    blade.isLoading = true;

                    modules.uninstall({ id: blade.currentEntity.id }, function (data) {
                        var newBlade = {
                            id: 'moduleInstallProgress',
                            currentEntity: data,
                            currentEntityId: data.id,
                            title: 'platform.blades.module-wizard-progress-step.title',
                            controller: 'platformWebApp.moduleInstallProgressController',
                            template: '$(Platform)/Scripts/app/packaging/wizards/newModule/module-wizard-progress-step.tpl.html'
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

    $scope.blade.headIcon = 'fa-cubes';

    // on load
    blade.refresh();
}]);
