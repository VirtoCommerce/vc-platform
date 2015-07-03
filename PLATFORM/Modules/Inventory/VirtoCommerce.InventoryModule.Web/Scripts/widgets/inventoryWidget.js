angular.module('virtoCommerce.inventoryModule')
.controller('virtoCommerce.inventoryModule.inventoryWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.inventoryModule.inventories', function ($scope, bladeNavigationService, inventories) {
    var blade = $scope.blade;

    function refresh () {
        $scope.currentInventory = '...';
        return inventories.query({ id: blade.itemId }, function (results) {
            $scope.currentInventory = _.reduce(results, function (memo, x) { return memo + x.inStockQuantity; }, 0);
        }, function (error) {
            //bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    }

    $scope.openBlade = function () {
        var newBlade = {
            id: "inventoryFulfillmentcentersListBlade",
            itemId: blade.itemId,
            parentWidgetRefresh: refresh,
            title: blade.title,
            subtitle: 'Select Fulfillment center to edit inventory',
            controller: 'virtoCommerce.inventoryModule.inventoryFulfillmentcentersListController',
            template: 'Modules/$(VirtoCommerce.Inventory)/Scripts/blades/inventory-fulfillmentcenters-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };

    $scope.$watch('blade.origItem.productType', function (productType) {
        if (productType && productType === 'Digital') {
            $scope.widget.widgetsInContainer.splice($scope.widget.widgetsInContainer.indexOf($scope.widget), 1);
        }
    });

    refresh();
}]);