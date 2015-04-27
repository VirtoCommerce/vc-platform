angular.module('virtoCommerce.gshoppingModule')
.controller('virtoCommerce.gshoppingModule.gshoppingWidgetController', ['$scope', 'bladeNavigationService', 'virtoCommerce.gshoppingModule.items', function ($scope, bladeNavigationService, items) {
    $scope.syncItems = function () {
        return items.query({ id: $scope.blade.currentEntityId }, function () {
        });
    }
}])

.controller('virtoCommerce.gshoppingModule.gshoppingSyncCatWidgetController', ['$scope', 'bladeNavigationService', 'virtoCommerce.gshoppingModule.catalog_items', function ($scope, bladeNavigationService, items) {

    $scope.widget.refresh = function() {
        $scope.importedProducts = 0;
    }

    $scope.syncItems = function () {
        $scope.importedProducts = 0;
        $scope.blade.isLoading = true;
        items.query({ catalogId: $scope.blade.parentBlade.catalogId, categoryId: $scope.blade.currentEntityId }, function (data) {
            $scope.importedProducts = data[0];
            $scope.blade.isLoading = false;
        });
    }

    $scope.widget.refresh();
}]);