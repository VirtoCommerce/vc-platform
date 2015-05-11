angular.module('virtoCommerce.customerModule')
.controller('virtoCommerce.customerModule.memberAddressesWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.widget.blade;

    $scope.openBlade = function () {
        var newBlade = {
        	id: "orderOperationAddresses",
            currentEntities: blade.currentEntity.addresses,
            title: blade.title,
            subtitle: 'Manage addresses',
            controller: 'virtoCommerce.coreModule.common.coreAddressListController',
            template: 'Modules/$(VirtoCommerce.Core)/Scripts/common/blades/address-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);