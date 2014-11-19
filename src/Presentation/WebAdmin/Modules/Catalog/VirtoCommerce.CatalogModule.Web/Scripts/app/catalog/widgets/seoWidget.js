angular.module('catalogModule.widget.seoWidget', [
])
.controller('seoWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService)
{
    $scope.currentBlade = $scope.widget.blade;

    $scope.openSeoBlade = function ()
    {

        var blade = {
            id: "seoDetail",
            currentEntityId: $scope.currentBlade.currentEntityId,
            title: $scope.currentBlade.title,
            subtitle: 'Seo details',
            controller: 'seoDetailController',
            template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/seo-detail.tpl.html'
        };


        bladeNavigationService.showBlade(blade, $scope.currentBlade);
    };

}]);
