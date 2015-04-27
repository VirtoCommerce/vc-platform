angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storeAdvancedWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.widget.blade;
    
    $scope.openBlade = function () {
        var newBlade = {
            id: "storeChildBlade",
            entity: blade.currentEntity,
            title: blade.title,
            subtitle: 'Advanced properties',
            controller: 'virtoCommerce.storeModule.storeAdvancedController',
            template: 'Modules/$(VirtoCommerce.Store)/Scripts/blades/store-advanced.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);