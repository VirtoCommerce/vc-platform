angular.module('virtoCommerce.quoteModule')
.controller('virtoCommerce.quoteModule.quoteAssetWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    
    $scope.openBlade = function () {
        var blade = {
            id: "quoteAssets",
            title: $scope.blade.title,
            subtitle: 'quotes.blades.quotes-assets.subtitle',
            currentEntity: $scope.blade.currentEntity,
            controller: 'virtoCommerce.quoteModule.quoteAssetController',
            template: 'Modules/$(VirtoCommerce.Quote)/Scripts/blades/quote-assets.tpl.html'
        };
        bladeNavigationService.showBlade(blade, $scope.blade);
    };

}]);