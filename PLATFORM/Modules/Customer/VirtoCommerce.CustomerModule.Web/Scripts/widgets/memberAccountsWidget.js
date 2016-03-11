angular.module('virtoCommerce.customerModule')
.controller('virtoCommerce.customerModule.memberAccountsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    
    $scope.openBlade = function () {
        var newBlade = {
            id: "customerChildBlade",
            title: $scope.blade.title,
            subtitle: 'customer.widgets.member-accounts-list.blade-subtitle',
            controller: 'virtoCommerce.customerModule.memberAccountsListController',
            template: 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/member-accounts-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };
}]);