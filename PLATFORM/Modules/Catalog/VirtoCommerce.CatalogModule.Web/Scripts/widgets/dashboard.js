angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.dashboard.catalogsWidgetController', ['$scope', '$state', 'virtoCommerce.catalogModule.catalogs', function ($scope, $state, catalogs) {
    $scope.data = { count: '', descr: 'Catalogs' };

    $scope.widgetAction = function () {
        $state.go('workspace.catalog');
    };

    catalogs.getCatalogs({}, function (data) {
        $scope.data.count = data.length;
    });
}])
.controller('virtoCommerce.catalogModule.dashboard.productsWidgetController', ['$scope', '$state', 'virtoCommerce.catalogModule.listEntries', function ($scope, $state, listEntries) {
    $scope.data = { count: '', descr: 'Products' };

    $scope.widgetAction = function () {
        $state.go('workspace.catalog');
    };

    listEntries.listitemssearch({ count: 0 }, function (data) {
        $scope.data.count = data.totalCount;
    });
}])
;