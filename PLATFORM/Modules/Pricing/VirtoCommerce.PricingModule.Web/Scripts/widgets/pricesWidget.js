angular.module('virtoCommerce.pricingModule')
.controller('pricesWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.currentBlade = $scope.widget.blade;
    $scope.priceCount = '';

    $scope.widget.refresh = function () {
        // all prices count
        if ($scope.currentBlade.currentEntity) {
            var pricelistPrices = _.flatten(_.pluck($scope.currentBlade.currentEntity.productPrices, 'prices'), true);
            $scope.priceCount = pricelistPrices.length;
        }
    }
    
    $scope.openBlade = function () {
        var blade = {
            id: "pricelistChild",
            // currentEntityId: $scope.currentBlade.currentEntityId,
            currentEntity: $scope.currentBlade.currentEntity,
            title: $scope.currentBlade.title,
            subtitle: 'Manage prices',
            controller: 'pricelistItemListController',
            template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/blades/pricelist-item-list.tpl.html'
        };

        bladeNavigationService.showBlade(blade, $scope.currentBlade);
    };

    $scope.$watch('currentBlade.currentEntity', function (data) {
        $scope.widget.refresh();
    });
}]);
