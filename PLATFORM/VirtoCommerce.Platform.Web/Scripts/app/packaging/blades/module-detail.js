angular.module('platformWebApp')
.controller('platformWebApp.moduleDetailController', ['$scope', 'platformWebApp.dialogService', 'platformWebApp.bladeNavigationService', 'platformWebApp.modules', function ($scope, dialogService, bladeNavigationService, modules) {

    $scope.blade.refresh = function () {
        modules.get({ id: $scope.blade.currentEntityId }, function(data) {
            initializeBlade(data);
        });
    }

    function initializeBlade(data) {
        data.tags = data.tags.split(' ');
        $scope.blade.currentEntity = data;
        $scope.blade.isLoading = false;
    };
    
    $scope.openModule = function (module) {
        $scope.blade.parentBlade.selectItem(module);
    }

    $scope.bladeToolbarCommands = [
        {
            name: "Update", icon: 'fa fa-upload',
            executeMethod: function () {
                openUpdateEntityBlade();
            },
            canExecuteMethod: function () {
                return $scope.currentEntity && $scope.currentEntity.isRemovable;
            },
            permission: 'platform:module:manage'
        },
        {
            name: "Uninstall", icon: 'fa fa-trash-o',
            executeMethod: function () {
                openDeleteEntityBlade();
            },
            canExecuteMethod: function () {
                return $scope.currentEntity && $scope.currentEntity.isRemovable;
            },
            permission: 'platform:module:manage'
        },
        {
            name: "Settings", icon: 'fa fa-wrench',
            executeMethod: function () {
                var newBlade = {
                    id: 'moduleSettingsSection',
                    moduleId: $scope.blade.currentEntityId,
                    title: 'Module settings',
                    //subtitle: '',
                    controller: 'platformWebApp.settingsDetailController',
                    template: 'Scripts/app/settings/blades/settings-detail.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, $scope.blade);
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
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    function openDeleteEntityBlade() {
        var dialog = {
            id: "confirmDelete",
            title: "Delete confirmation",
            message: "Are you sure you want to uninstall this Module?",
            callback: function (remove) {
                if (remove) {
                    $scope.blade.isLoading = true;

                    modules.uninstall({ id: $scope.currentEntity.id }, function (data) {
                        var newBlade = {
                            id: 'moduleInstallProgress',
                            currentEntityId: data.id,
                            title: 'module uninstall',
                            subtitle: 'Installation progress',
                            controller: 'platformWebApp.moduleInstallProgressController',
                            template: 'Scripts/app/packaging/wizards/newModule/module-wizard-progress-step.tpl.html'
                        };
                        bladeNavigationService.showBlade(newBlade, $scope.blade.parentBlade);
                    });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    $scope.bladeHeadIco = 'fa fa-cubes';

    // on load
    $scope.blade.refresh();
}]);
