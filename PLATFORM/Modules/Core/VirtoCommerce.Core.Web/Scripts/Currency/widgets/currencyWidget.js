angular.module('virtoCommerce.coreModule.currency')
.controller('virtoCommerce.coreModule.currency.currencyWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.coreModule.currency.currencyApi', function ($scope, bladeNavigationService, currencyApi) {
    var blade = $scope.blade;

    $scope.widget.refresh = function () {
        $scope.currentNumberInfo = '...';
        currencyApi.query({}, function (results) {
            $scope.currentNumberInfo = results.length;
        }, function (error) {
            //bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    }

    $scope.openBlade = function () {
        var newBlade = {
            id: 'currencyList',
            controller: 'virtoCommerce.coreModule.currency.currencyListController',
            template: 'Modules/$(VirtoCommerce.Core)/Scripts/currency/blades/currency-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };

    $scope.widget.refresh();
}]);