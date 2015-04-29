angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storePaymentsWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.widget.blade;
    
    $scope.openBlade = function () {
        var newBlade = {
            id: "storeChildBlade",
            currentEntities: blade.currentEntity.paymentGateways,
            title: blade.title,
            subtitle: 'Manage store payments',
            controller: 'virtoCommerce.storeModule.storePaymentsListController',
            template: 'Modules/$(VirtoCommerce.Store)/Scripts/blades/store-payments-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);