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
            template: 'Modules/$(VirtoCommerce.Store)/Scripts/blades/store-advanced.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);