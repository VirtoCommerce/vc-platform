angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storeShippingWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.widget.blade;
    
    $scope.openBlade = function () {
        var newBlade = {
            id: "storeChildBlade",
            currentEntities: blade.currentEntity.settings,
            title: blade.title,
            subtitle: 'Shipping methods',
            controller: 'virtoCommerce.storeModule.shippingMethodListController',
            template: 'Modules/$(VirtoCommerce.Store)/Scripts/blades/shippingMethod-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);