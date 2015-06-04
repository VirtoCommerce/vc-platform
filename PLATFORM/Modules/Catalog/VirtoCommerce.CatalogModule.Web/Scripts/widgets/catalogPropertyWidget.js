angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.catalogPropertyWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;
    
    $scope.openCatalogPropertyBlade = function () {
        var newBlade = {
            id: "categoryPropertyDetail",
            currentEntityId: blade.currentEntityId,
            currentEntity: blade.currentEntity,
            title: blade.title,
            subtitle: 'Catalog properties',
            controller: 'virtoCommerce.catalogModule.catalogPropertyController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/catalog-property-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);
