angular.module('virtoCommerce.orderModule')
.controller('virtoCommerce.orderModule.operationTreeWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;
    $scope.operation = {};
    $scope.$watch('widget.blade.customerOrder', function (operation) {
        $scope.operation = operation;
        if (!$scope.currentOperationId) {
            $scope.currentOperationId = operation.id;
        }
    });

    $scope.selectOperation = function (operation) {
        if ($scope.currentOperationId != operation.id) {
            $scope.currentOperationId = operation.id;
            var newBlade = undefined;
            if (operation.operationType.toLowerCase() == 'shipment') {
                newBlade = {
                    id: 'operationDetail',
                    title: 'orders.blades.shipment-detail.title',
                    titleValues: { number: operation.number },
                    subtitle: 'orders.blades.shipment-detail.subtitle',
                    customerOrder: blade.customerOrder,
                    currentEntity: operation,
                    controller: 'virtoCommerce.orderModule.operationDetailController',
                    template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/shipment-detail.tpl.html'
                };
            }
            else if (operation.operationType.toLowerCase() == 'customerorder') {
                bladeNavigationService.closeChildrenBlades(blade);
            }
            else if (operation.operationType.toLowerCase() == 'paymentin') {
                newBlade = {
                    id: 'operationDetail',
                    title: 'orders.blades.payment-detail.title',
                    titleValues: { number: operation.number },
                    subtitle: 'orders.blades.payment-detail.subtitle',
                    customerOrder: blade.customerOrder,
                    currentEntity: operation,
                    stores: blade.stores,
                    controller: 'virtoCommerce.orderModule.operationDetailController',
                    template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/payment-detail.tpl.html'
                };
            }
            if (newBlade) {
                bladeNavigationService.showBlade(newBlade, blade);
            }
        }
    };

}]);
