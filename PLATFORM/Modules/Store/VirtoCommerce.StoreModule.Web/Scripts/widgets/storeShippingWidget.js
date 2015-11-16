angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storeShippingWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;
    
    $scope.openBlade = function () {
        var newBlade = {
            id: "storeChildBlade",
            title: blade.title,
            subtitle: 'stores.widgets.storeShippingWidget.blade-subtitle',
            controller: 'virtoCommerce.storeModule.shippingMethodListController',
            template: 'Modules/$(VirtoCommerce.Store)/Scripts/blades/shippingMethod-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);