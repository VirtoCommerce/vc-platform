angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.catalogsSelectController', ['$scope', 'virtoCommerce.catalogModule.catalogs', '$timeout', function ($scope, catalogs, $timeout) {

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

        catalogs.getCatalogs({}, function (results) {
            if ($scope.blade.doShowAllCatalogs) {
                $scope.objects = results;
            } else {
                $scope.objects = _.where(results, { virtual: false });
            }

            $scope.blade.isLoading = false;
        });
    };

    $scope.selectNode = function (selectedNode) {
        $scope.bladeClose(function () {
            $scope.blade.parentBlade.onAfterCatalogSelected(selectedNode);
        });
    };

    // actions on load
    $scope.blade.refresh();
}]);