angular.module('virtoCommerce.orderModule')
.controller('virtoCommerce.orderModule.shipmentItemsWidgetController', ['$scope', '$translate', 'platformWebApp.bladeNavigationService', function ($scope, $translate, bladeNavigationService) {
    $scope.blade = $scope.widget.blade;
    $scope.operation = {};

    $scope.$watch('widget.blade.currentEntity', function (operation) {
        $scope.operation = operation;
    });

    $scope.openItemsBlade = function () {
        $translate('orders.blades.shipment-detail.title', { number: $scope.operation.number }).then(function (result) {
            var newBlade = {
                id: 'shipmentItems',
                title: 'orders.blades.shipment-items.title',
                titleValues: { title: result },
                subtitle: 'orders.blades.shipment-items.subtitle',
                currentEntity: $scope.blade.currentEntity,
                controller: 'virtoCommerce.orderModule.shipmentItemsController',
                template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/shipment-items.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, $scope.blade);
        });
    };
}]);
