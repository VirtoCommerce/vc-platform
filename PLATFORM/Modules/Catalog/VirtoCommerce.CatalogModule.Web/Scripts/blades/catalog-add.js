angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.catalogAddController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.catalogModule.catalogs', function ($scope, bladeNavigationService, catalogs) {

    $scope.addCatalog = function () {
        catalogs.newCatalog({}, function (data) {
            var newBlade = {
                id: 'catalogEdit',
                isNew: true,
                currentEntity: data,
                title: 'New catalog',
                subtitle: 'Catalog details',
                controller: 'virtoCommerce.catalogModule.catalogDetailController',
                template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/catalog-detail.tpl.html'
            };

            bladeNavigationService.showBlade(newBlade, $scope.blade.parentBlade);
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    };

    $scope.addVirtualCatalog = function () {
        catalogs.newVirtualCatalog({}, function (data) {
            var newBlade = {
                id: 'catalogEdit',
                isNew: true,
                currentEntity: data,
                title: 'New virtual catalog',
                subtitle: 'Virtual catalog details',
                controller: 'virtoCommerce.catalogModule.virtualCatalogDetailController',
                template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/catalog-detail.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, $scope.blade.parentBlade);
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    };
    
    $scope.blade.isLoading = false;
}]);
