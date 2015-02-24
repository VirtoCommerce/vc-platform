angular.module('virtoCommerce.inventoryModule.blades', [
    'virtoCommerce.inventoryModule.resources',
    'platformWebApp.common.confirmDialog'
])
.controller('inventoryFulfillmentcentersListController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.selectedItem = null;

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;
        return $scope.blade.parentWidget.refresh().$promise.then(function (results) {
            $scope.blade.isLoading = false;
            $scope.blade.currentEntities = results;
            return results;
        });
    }

    $scope.openBlade = function (data) {
        $scope.selectedItem = data;

        var newBlade = {
            id: "inventoryDetailBlade",
            itemId: $scope.blade.itemId,
            data: data,
            title: data.fulfillmentCenter.name,
            subtitle: 'Edit Inventory',
            controller: 'inventoryDetailController',
            template: 'Modules/Inventory/VirtoCommerce.InventoryModule.Web/Scripts/blades/inventory-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
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
            name: "Refresh", icon: 'fa fa-refresh',
            executeMethod: function () {
                $scope.blade.refresh();
            },
            canExecuteMethod: function () {
                return true;
            }
        }
    ];

    $scope.blade.refresh();
}]);