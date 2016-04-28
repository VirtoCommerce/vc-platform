angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.catalogsSelectController', ['$scope', 'virtoCommerce.catalogModule.catalogs', 'platformWebApp.bladeNavigationService', function ($scope, catalogs, bladeNavigationService) {
    var blade = $scope.blade;

    blade.refresh = function () {
        blade.isLoading = true;

        catalogs.getCatalogs({}, function (results) {
            if (blade.doShowAllCatalogs) {
                $scope.objects = results;
            } else {
                $scope.objects = _.where(results, { isVirtual: false });
            }

            blade.isLoading = false;
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    };

    $scope.selectNode = function (selectedNode) {
        $scope.bladeClose(function () {
            blade.parentBlade.onAfterCatalogSelected(selectedNode);
        });
    };

    // actions on load
    blade.refresh();
}]);