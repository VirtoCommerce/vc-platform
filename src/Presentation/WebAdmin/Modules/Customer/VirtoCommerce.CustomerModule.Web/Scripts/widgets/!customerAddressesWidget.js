angular.module('virtoCommerce.customerModule.widgets', [])
.controller('customerAddressesWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.widget.blade;

    $scope.openBlade = function () {
        var newBlade = {
            id: "customerChildBlade",
            currentEntities: blade.currentEntity.addresses,
            title: blade.title,
            subtitle: 'Manage customer addresses',
            controller: 'customerAddressListController',
            template: 'Modules/customer/VirtoCommerce.customerModule.Web/Scripts/blades/customer-address-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);