angular.module('virtoCommerce.storeModule.widgets', [])
.controller('storeAdvancedWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.widget.blade;
    
    $scope.openBlade = function () {
        var newBlade = {
            id: "storeChildBlade",
            entity: blade.currentEntity,
            title: blade.title,
            subtitle: 'Advanced properties',
            controller: 'storeAdvancedController',
            template: 'Modules/Store/VirtoCommerce.StoreModule.Web/Scripts/blades/store-advanced.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);