angular.module('virtoCommerce.customerModule')
.controller('virtoCommerce.customerModule.customerAccountsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    
    $scope.openBlade = function () {
        var newBlade = {
            id: "customerChildBlade",
            title: $scope.blade.title,
            subtitle: 'customer.widgets.customer-accounts-list.blade-subtitle',
            controller: 'virtoCommerce.customerModule.customerAccountsListController',
            template: 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/customer-accounts-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };
}]);