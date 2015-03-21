angular.module('virtoCommerce.packaging.blades.moduleDetail', [])
.controller('moduleDetailController', ['$scope', 'dialogService', 'bladeNavigationService', 'modules', function ($scope, dialogService, bladeNavigationService, modules) {

    function initializeBlade(data) {
        $scope.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.isLoading = false;
    };

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        closeCallback();
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.bladeToolbarCommands = [
        {
            name: "Update", icon: 'fa fa-upload',
            executeMethod: function () {
                openUpdateEntityBlade();
            },
            canExecuteMethod: function () {
                return $scope.currentEntity && $scope.currentEntity.isRemovable;
            }
        },
        {
            name: "Uninstall", icon: 'fa fa-trash-o',
            executeMethod: function () {
                openDeleteEntityBlade();
            },
            canExecuteMethod: function () {
                return $scope.currentEntity && $scope.currentEntity.isRemovable;
            }
        }
    ];

    function openUpdateEntityBlade() {
        closeChildrenBlades();

        var newBlade = {
            id: "moduleWizard",
            title: "Module update",
            // subtitle: '',
            mode: 'update',
            controller: 'installWizardController',
            template: 'Modules/$(VirtoCommerce.Packaging)/Scripts/blades/module-detail.tpl.html'
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
                            controller: 'moduleInstallProgressController',
                            template: 'Modules/$(VirtoCommerce.Packaging)/Scripts/wizards/newModule/module-wizard-progress-step.tpl.html'
                        };
                        bladeNavigationService.showBlade(newBlade, $scope.blade.parentBlade);
                        $scope.bladeClose();
                    });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    $scope.bladeHeadIco = 'fa fa-cubes';

    // on load
    initializeBlade($scope.blade.currentEntity);
}]);
