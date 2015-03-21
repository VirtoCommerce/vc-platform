angular.module('virtoCommerce.inventoryModule.widgets', [
    'virtoCommerce.inventoryModule.blades'
])
.controller('inventoryWidgetController', ['$scope', 'bladeNavigationService', 'inventories', function ($scope, bladeNavigationService, inventories) {
    var blade = $scope.widget.blade;
    
    $scope.widget.refresh = function () {
        $scope.currentInventory = '...';
        return inventories.query({ id: blade.itemId }, function (results) {
            $scope.currentInventory = _.reduce(results, function (memo, x) { return memo + x.inStockQuantity; }, 0);
        });
    }

    $scope.openBlade = function () {
        var newBlade = {
            id: "inventoryFulfillmentcentersListBlade",
            itemId: blade.itemId,
            parentWidget: $scope.widget,
            title: blade.title,
            subtitle: 'Select Fulfillment center to edit inventory',
            controller: 'inventoryFulfillmentcentersListController',
            template: 'Modules/$(VirtoCommerce.Inventory)/Scripts/blades/!inventory-fulfillmentcenters-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };

    $scope.widget.refresh();
}]);