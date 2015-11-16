angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storeTaxingWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;
    
    $scope.openBlade = function () {
        var newBlade = {
            id: "storeChildBlade",
            title: blade.title,
            subtitle: 'stores.widgets.storeTaxingWidget.blade-subtitle',
            controller: 'virtoCommerce.storeModule.taxProviderListController',
            template: 'Modules/$(VirtoCommerce.Store)/Scripts/blades/taxProvider-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);