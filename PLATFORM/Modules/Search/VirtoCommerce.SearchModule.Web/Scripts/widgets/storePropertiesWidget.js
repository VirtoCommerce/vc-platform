angular.module('virtoCommerce.searchModule')
.controller('virtoCommerce.searchModule.storePropertiesWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;

    $scope.openBlade = function () {
        var newBlade = {
            id: "storeFilteringProperties",
            storeId: blade.currentEntity.id,
            title: 'Filtering properties',
            controller: 'virtoCommerce.searchModule.storePropertiesController',
            template: 'Modules/$(VirtoCommerce.Search)/Scripts/blades/store-properties.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);
