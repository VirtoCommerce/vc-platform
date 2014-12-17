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
            name: "Update", icon: 'icon-arrow-up',
            executeMethod: function () {
                openUpdateEntityBlade();
            },
            canExecuteMethod: function () {
                // return $scope.currentEntity.isRemovable;
                return true;
            }
        },
        {
            name: "Delete", icon: 'icon-remove',
            executeMethod: function () {
                openDeleteEntityBlade();
            },
            canExecuteMethod: function () {
                // return $scope.currentEntity.isRemovable;
                return true;
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
            bladeActions: 'Modules/Packaging/VirtoCommerce.PackagingModule.Web/Scripts/wizards/newModule/install-wizard-actions.tpl.html',
            template: 'Modules/Packaging/VirtoCommerce.PackagingModule.Web/Scripts/blades/module-detail.tpl.html'
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
                    // $scope.blade.isLoading = true;

                    var newBlade = {
                        id: 'moduleInstallProgress',
                        title: 'module uninstall',
                        subtitle: 'Installation progress',
                        controller: 'moduleInstallProgressController',
                        template: 'Modules/Packaging/VirtoCommerce.PackagingModule.Web/Scripts/wizards/newModule/module-wizard-progress-step.tpl.html'
                    };

                    modules.uninstall({ id: $scope.currentEntity.id }, function (data) {
                        newBlade.currentEntityId = data.id;
                        bladeNavigationService.showBlade(newBlade, $scope.blade);
                    });
            }
        }
    }
        dialogService.showConfirmationDialog(dialog);
}

    // on load
    initializeBlade($scope.blade.currentEntity);
}]);
