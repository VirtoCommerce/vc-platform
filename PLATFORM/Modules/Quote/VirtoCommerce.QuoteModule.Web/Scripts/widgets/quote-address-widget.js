angular.module('virtoCommerce.quoteModule')
.controller('virtoCommerce.quoteModule.quoteAddressWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;

    $scope.openBlade = function () {
        var newBlade = {
            id: 'quoteAddresses',
            currentEntities: blade.currentEntity.addresses,
            title: blade.title,
            subtitle: 'quotes.widgets.quote-address.blade-subtitle',
            controller: 'virtoCommerce.coreModule.common.coreAddressListController',
            template: 'Modules/$(VirtoCommerce.Core)/Scripts/common/blades/address-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };

    $scope.$watch('blade.currentEntity', function (data) {
        if (data) {
            // todo: search for default address
            $scope.address = data.addresses.length > 0 ? data.addresses[0] : null;
        }
    });
}]);
