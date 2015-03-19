angular.module('virtoCommerce.catalogModule')
.controller('importJobCatalogsController', ['$scope', 'catalogs', function ($scope, catalogs)
{
    $scope.blade.refresh = function ()
    {
        $scope.selectedCatalogId = $scope.blade.parentBlade.item.catalogId;
        $scope.blade.isLoading = true;

        var searchResult = catalogs.getCatalogs({}, function (results)
        {
            $scope.blade.isLoading = false;
            $scope.catalogs = results;
        });

    };

    $scope.setCatalog = function (catalog) {
        $scope.selectedCatalogId = catalog.id;
        $scope.blade.parentBlade.item.catalogId = catalog.id;
        $scope.blade.parentBlade.item.catalogName = catalog.name;
        $scope.bladeClose();
    }

    $scope.blade.refresh();

}]);


