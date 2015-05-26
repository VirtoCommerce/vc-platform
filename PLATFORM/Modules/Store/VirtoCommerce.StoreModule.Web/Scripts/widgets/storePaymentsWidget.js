angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storePaymentsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;
    
    $scope.openBlade = function () {
        var newBlade = {
            id: "storeChildBlade",
            title: blade.title,
            subtitle: 'Payment methods',
            controller: 'virtoCommerce.storeModule.paymentMethodListController',
            template: 'Modules/$(VirtoCommerce.Store)/Scripts/blades/paymentMethod-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);