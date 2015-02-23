angular.module('virtoCommerce.pricingModule.blades.item', [
    'virtoCommerce.pricingModule.resources.pricing',
    'platformWebApp.common.confirmDialog'
])
.controller('itemPricelistsListController', ['$scope', 'prices', 'bladeNavigationService', function ($scope, prices, bladeNavigationService) {
    $scope.selectedItem = null;

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;
        return prices.query({ id: $scope.blade.itemId }, function (data) {
            $scope.blade.isLoading = false;
            $scope.blade.currentEntities = data;
        }, function (error) {
            $scope.blade.isLoading = false;
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    }

    $scope.openBlade = function (data) {
        $scope.selectedItem = data;

        var newBlade = {
            id: "itemPrices",
            itemId: $scope.blade.itemId,
            data: data,
            title: data.name,
            subtitle: 'Manage prices',
            controller: 'itemPricesListController',
            template: 'Modules/Pricing/VirtoCommerce.PricingModule.Web/Scripts/blades/item/item-prices-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        closeCallback();
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.bladeToolbarCommands = [
        {
            name: "Refresh", icon: 'fa fa-refresh',
            executeMethod: function () {
                $scope.blade.refresh();
            },
            canExecuteMethod: function () {
                return true;
            }
        }
    ];

    $scope.blade.refresh();
}]);
