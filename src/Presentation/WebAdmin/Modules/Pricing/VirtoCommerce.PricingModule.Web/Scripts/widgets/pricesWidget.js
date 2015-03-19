angular.module('virtoCommerce.pricingModule')
.controller('pricesWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.currentBlade = $scope.widget.blade;

    $scope.openBlade = function () {
        var blade = {
            id: "pricesList",
            // currentEntityId: $scope.currentBlade.currentEntityId,
            currentEntity: $scope.currentBlade.currentEntity,
            title: $scope.currentBlade.title,
            subtitle: 'Manage prices',
            controller: 'priceListController',
            template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/blades/price-list.tpl.html'
        };

        bladeNavigationService.showBlade(blade, $scope.currentBlade);
    };
}]);
