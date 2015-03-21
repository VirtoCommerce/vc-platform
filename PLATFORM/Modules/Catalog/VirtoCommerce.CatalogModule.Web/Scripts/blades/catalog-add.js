angular.module('virtoCommerce.catalogModule')
.controller('catalogAddController', ['$scope', 'bladeNavigationService', 'catalogs', function ($scope, bladeNavigationService, catalogs) {

    $scope.addCatalog = function () {
        catalogs.newCatalog({}, function (data) {
            showCatalogBlade(null, data, null);
            $scope.bladeClose();
          
        });
    };

    $scope.addVirtualCatalog = function () {
        catalogs.newVirtualCatalog({}, function (data) {
            showVirtualCatalogBlade(null, data, null);
            $scope.bladeClose();
          
        });
    };

    function showCatalogBlade(id, data, title) {
        var newBlade = {
            currentEntityId: id,
            currentEntity: data,
            title: title,
            id: 'catalogEdit',
            subtitle: 'Catalog details',
            controller: 'catalogDetailController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/catalog-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, $scope.blade.parentBlade);
    };

    function showVirtualCatalogBlade(id, data, title) {
        var newBlade = {
            currentEntityId: id,
            currentEntity: data,
            title: title,
            subtitle: 'Virtual catalog details',
            id: 'catalogEdit',
            controller: 'virtualCatalogDetailController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/virtual-catalog-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, $scope.blade.parentBlade);
    };

    $scope.blade.isLoading = false;
}]);
