angular.module('virtoCommerce.inventoryModule')
.controller('virtoCommerce.inventoryModule.inventoryFulfillmentcentersListController', ['$scope', '$timeout', 'platformWebApp.bladeNavigationService', function ($scope, $timeout, bladeNavigationService) {
    $scope.selectedItem = null;
    var openFirstEntityDetailsOnce = _.once(function () {
        if ($scope.blade.currentEntities && $scope.blade.currentEntities.length > 0)
            $timeout(function () {
                $scope.openBlade($scope.blade.currentEntities[0]);
            }, 0, false);
    });

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;
        return $scope.blade.parentWidgetRefresh().$promise.then(function (results) {
            $scope.blade.isLoading = false;
            $scope.blade.currentEntities = results;

            openFirstEntityDetailsOnce();
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
            controller: 'virtoCommerce.inventoryModule.inventoryDetailController',
            template: 'Modules/$(VirtoCommerce.Inventory)/Scripts/blades/inventory-detail.tpl.html'
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

    $scope.blade.toolbarCommands = [
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