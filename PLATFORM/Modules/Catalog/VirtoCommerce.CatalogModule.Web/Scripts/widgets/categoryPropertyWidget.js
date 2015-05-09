angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.categoryPropertyWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService)
{
    $scope.currentBlade = $scope.widget.blade;

    $scope.openCategoryPropertyBlade = function ()
    {

        var blade = {
            id: "categoryPropertyDetail",
            currentEntityId: $scope.currentBlade.currentEntityId,
            currentEntity: $scope.currentBlade.currentEntity,
            title: $scope.currentBlade.title,
            subtitle: 'Category properties',
            controller: 'virtoCommerce.catalogModule.categoryPropertyController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/category-property-detail.tpl.html'
        };


        bladeNavigationService.showBlade(blade, $scope.currentBlade);
    };

}]);
