angular.module('catalogModule.widget.virtualCatalogMappingWidget', [
    'catalogModule.blades.virtualCatalogMapping'
])
.controller('virtualCatalogMappingWidgetController', ['$scope', 'virtualCatalogSearch', 'bladeNavigationService', function ($scope, virtualCatalogSearch, bladeNavigationService) {
    $scope.widget.blade.mappedNodes = [];

    $scope.widget.blade.refreshMappings = function () {
        $scope.widget.blade.isLoading = true;
        var serviceParameters = {};
        if (angular.isDefined($scope.widget.blade.parentCatalogId)) {
            serviceParameters.catalogId = $scope.widget.blade.parentCatalogId;
            serviceParameters.categoryId = $scope.widget.blade.currentEntityId;
        } else {
            serviceParameters.catalogId = $scope.widget.blade.currentEntityId;
        }

        return virtualCatalogSearch.getcatalogmappinginfo(serviceParameters,
            function (searchResult) {
                $scope.widget.blade.mappedNodes = searchResult.treeNodes;
                $scope.widget.blade.isLoading = false;
                return searchResult;
            });
    }

    $scope.openBlade = function () {
        var childBlade = {
            id: "mappingTarget",
            title: $scope.widget.blade.origEntity.name + ' - mapped categories',
            subtitle: 'currently mapped items',
            controller: 'virtualCatalogMappingTargetController',
            template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/virtual-catalog-mapping-target.tpl.html'
        };

        if (angular.isDefined($scope.widget.blade.parentCatalogId)) {
            childBlade.catalogId = $scope.widget.blade.parentCatalogId;
            childBlade.categoryId = $scope.widget.blade.currentEntityId;
        } else {
            childBlade.currentEntityId = $scope.widget.blade.currentEntityId;
        }

        bladeNavigationService.showBlade(childBlade, $scope.widget.blade);
    };

    // on load
    $scope.widget.blade.refreshMappings();
}]);
