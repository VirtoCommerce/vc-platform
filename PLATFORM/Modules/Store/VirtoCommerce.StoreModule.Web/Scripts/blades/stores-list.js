angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storesListController', ['$scope', 'virtoCommerce.storeModule.stores', 'platformWebApp.bladeNavigationService', 'platformWebApp.authService', function ($scope, stores, bladeNavigationService, authService) {
    $scope.selectedNodeId = null;

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;
        stores.query({}, function (data) {
        	$scope.blade.isLoading = false;

        	$scope.blade.currentEntities = data;
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    }

    $scope.blade.openBlade = function (data) {
        $scope.selectedNodeId = data.id;

        var newBlade = {
            id: 'storeDetails',
            currentEntityId: data.id,
            // currentEntity: data,
            title: data.name,
            subtitle: 'stores.blades.store-detail.subtitle',
            controller: 'virtoCommerce.storeModule.storeDetailController',
            template: 'Modules/$(VirtoCommerce.Store)/Scripts/blades/store-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    function openBladeNew() {
        $scope.selectedNodeId = null;

        var newBlade = {
            id: 'storeDetails',
            // currentEntityId: data.id,
            currentEntity: {},
            title: 'stores.blades.new-store-wizard.title',
            subtitle: 'stores.blades.new-store-wizard.subtitle',
            controller: 'virtoCommerce.storeModule.newStoreWizardController',
            template: 'Modules/$(VirtoCommerce.Store)/Scripts/wizards/newStore/new-store-wizard.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        closeCallback();
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.blade.headIcon = 'fa-archive';

    $scope.blade.toolbarCommands = [
        {
            name: "platform.commands.refresh", icon: 'fa fa-refresh',
            executeMethod: function () {
                $scope.blade.refresh();
            },
            canExecuteMethod: function () {
                return true;
            }
        },
        {
            name: "platform.commands.add", icon: 'fa fa-plus',
            executeMethod: function () {
                openBladeNew();
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'store:create'
        }

    ];

    $scope.blade.refresh();
}]);
