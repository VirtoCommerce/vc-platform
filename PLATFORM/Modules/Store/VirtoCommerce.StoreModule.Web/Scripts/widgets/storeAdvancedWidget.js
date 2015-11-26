﻿angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storeAdvancedWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.widget.blade;
    
    $scope.openBlade = function () {
        var newBlade = {
            id: "storeChildBlade",
            entity: blade.currentEntity,
            title: blade.title,
            subtitle: 'stores.widgets.storeAdvancedWidget.blade-subtitle',
            controller: 'virtoCommerce.storeModule.storeAdvancedController',
            template: 'Modules/$(VirtoCommerce.Store)/Scripts/blades/store-advanced.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);