angular.module('catalogModule.blades.catalogsSelect', [
   'catalogModule.resources.catalogs'
])
.controller('catalogsSelectController', ['$scope', 'catalogs', 'bladeNavigationService',
function ($scope, catalogs, bladeNavigationService) {

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

        catalogs.getCatalogs({}, function (results) {
            $scope.objects = _.where(results, { virtual: false });

            $scope.blade.isLoading = false;
        });
    };

    $scope.selectNode = function (selectedNode) {
        $scope.bladeClose();

        var newBlade = {
            id: 'itemsList' + ($scope.blade.parentBlade.level + 1),
            level: $scope.blade.parentBlade.level + 1,
            mode: $scope.blade.mode,
            breadcrumbs: [],
            title: $scope.blade.childTitle,
            subtitle: $scope.blade.subtitle,
            catalogId: selectedNode.id,
            catalog: selectedNode,
            controller: 'categoriesItemsListController',
            template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/categories-items-list.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, $scope.blade.parentBlade);
    };

    // actions on load
    $scope.blade.refresh();
}]);