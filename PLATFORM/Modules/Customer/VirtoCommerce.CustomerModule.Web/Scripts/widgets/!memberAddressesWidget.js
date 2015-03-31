angular.module('virtoCommerce.customerModule.widgets', [])
.controller('memberAddressesWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.widget.blade;

    $scope.openBlade = function () {
        var newBlade = {
        	id: "orderOperationAddresses",
            currentEntities: blade.currentEntity.addresses,
            title: blade.title,
            subtitle: 'Manage addresses',
            controller: 'coreAddressListController',
            template: 'Modules/$(VirtoCommerce.Core)/Scripts/common/blades/address-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);