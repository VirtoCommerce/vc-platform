angular.module('virtoCommerce.inventoryModule')
.controller('virtoCommerce.inventoryModule.inventoryWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.inventoryModule.inventories', function ($scope, bladeNavigationService, inventories) {
    var blade = $scope.blade;

    function refresh() {
        $scope.currentInventory = '...';
        return inventories.query({ id: blade.itemId }, function (results) {
            if (_.any(results)) {
                $scope.currentInventory = _.reduce(results, function (memo, x) { return memo + x.inStockQuantity; }, 0);
                var reservedQuantity = _.reduce(results, function (memo, x) { return memo + x.reservedQuantity; }, 0);
                $scope.availableInventory = $scope.currentInventory - reservedQuantity;
            } else {
                $scope.currentInventory = 'N/A';
            }
        }, function (error) {
            //bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    }

    $scope.openBlade = function () {
        if ($scope.currentInventory !== '...') {
            var newBlade = {
                id: "inventoryFulfillmentcentersListBlade",
                itemId: blade.itemId,
                parentWidgetRefresh: refresh,
                title: blade.title,
                subtitle: 'inventory.widgets.inventoryWidget.blade-subtitle',
                controller: 'virtoCommerce.inventoryModule.inventoryFulfillmentcentersListController',
                template: 'Modules/$(VirtoCommerce.Inventory)/Scripts/blades/inventory-fulfillmentcenters-list.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        }
    };

    $scope.$watch('blade.origItem.productType', function (productType) {
        if (productType && productType === 'Digital') {
            $scope.widget.widgetsInContainer.splice($scope.widget.widgetsInContainer.indexOf($scope.widget), 1);
        }
    });

    refresh();
}]);