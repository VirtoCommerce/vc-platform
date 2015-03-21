angular.module('virtoCommerce.customerModule.widgets', [])
.controller('memberAddressesWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.widget.blade;

    $scope.openBlade = function () {
        var newBlade = {
            id: "customerChildBlade",
            currentEntities: blade.currentEntity.addresses,
            title: blade.title,
            subtitle: 'Manage addresses',
            controller: 'memberAddressListController',
            template: 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/member-address-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);