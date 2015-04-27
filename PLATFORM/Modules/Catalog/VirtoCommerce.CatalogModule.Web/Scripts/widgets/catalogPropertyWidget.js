angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.catalogPropertyWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService)
{
    $scope.currentBlade = $scope.widget.blade;

    $scope.openCatalogPropertyBlade = function ()
    {

        var blade = {
            id: "categoryPropertyDetail",
            currentEntityId: $scope.currentBlade.currentEntityId,
            currentEntity: $scope.currentBlade.currentEntity,
            title: $scope.currentBlade.title,
            subtitle: 'Catalog properties',
            controller: 'virtoCommerce.catalogModule.catalogPropertyController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/catalog-property-detail.tpl.html'
        };


        bladeNavigationService.showBlade(blade, $scope.currentBlade);
    };

}]);
