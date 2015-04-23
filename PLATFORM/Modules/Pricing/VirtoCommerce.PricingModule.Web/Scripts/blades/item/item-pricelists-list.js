angular.module('virtoCommerce.pricingModule')
.controller('itemPricelistsListController', ['$scope', 'prices', 'bladeNavigationService', function ($scope, prices, bladeNavigationService) {
    $scope.selectedItem = null;

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;
        return $scope.blade.parentWidget.refresh().$promise.then(function (results) {
            $scope.blade.isLoading = false;
            $scope.blade.currentEntities = results;
            return results;
        });
    }

    $scope.openBlade = function (data) {
        $scope.selectedItem = data;

        var newBlade = {
            id: "itemPrices",
            isApiSave: true,
            itemId: $scope.blade.itemId,
            data: data,
            currency: data.currency,
            title: data.name,
            subtitle: 'Manage prices',
            controller: 'pricesListController',
            template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/blades/prices-list.tpl.html'
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


    $scope.getPriceCount = function (pricelist) {
        var pricelistPrices = _.flatten(_.pluck(pricelist.productPrices, 'prices'), true);
        return pricelistPrices.length;
    }

    $scope.blade.refresh();
}]);
