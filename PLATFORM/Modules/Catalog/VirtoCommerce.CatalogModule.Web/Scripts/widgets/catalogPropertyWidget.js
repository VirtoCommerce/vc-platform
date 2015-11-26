﻿angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.catalogPropertyWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;
    
    $scope.openCatalogPropertyBlade = function () {
        var newBlade = {
            id: "categoryPropertyDetail",
            currentEntityId: blade.currentEntityId,
            currentEntity: blade.currentEntity,
            title: blade.title,
            subtitle: 'catalog.widgets.catalogProperty.blade-subtitle',
            controller: 'virtoCommerce.catalogModule.catalogPropertyListController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/catalog-property-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);
