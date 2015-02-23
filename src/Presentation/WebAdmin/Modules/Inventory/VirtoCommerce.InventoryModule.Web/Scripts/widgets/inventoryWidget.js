angular.module('virtoCommerce.inventoryModule.widgets', [
    'virtoCommerce.inventoryModule.blades'
])
.controller('inventoryWidgetController', ['$scope', 'bladeNavigationService', 'inventories', function ($scope, bladeNavigationService, inventories) {
    var blade = $scope.widget.blade;
    
    $scope.widget.refresh = function () {
        $scope.currentInventory = '...';
        return inventories.get({ id: blade.itemId }, function (data) {
            $scope.currentInventory = data.inStockQuantity;
        });
    }

    $scope.openBlade = function () {
        var newBlade = {
            id: "inventoryDetailBlade",
            itemId: blade.itemId,
            parentWidget: $scope.widget,
            title: blade.title,
            subtitle: 'Edit Inventory',
            controller: 'inventoryDetailController',
            template: 'Modules/Inventory/VirtoCommerce.InventoryModule.Web/Scripts/blades/inventory-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };

    $scope.widget.refresh();
}]);