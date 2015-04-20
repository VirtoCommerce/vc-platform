angular.module('virtoCommerce.gshoppingModule')
.controller('gshoppingWidgetController', ['$scope', 'bladeNavigationService', 'gshopping_res_items', function ($scope, bladeNavigationService, items) {
    $scope.syncItems = function () {
        return items.query({ id: $scope.blade.currentEntityId }, function () {
        });
    }
}])

.controller('gshoppingSyncCatWidgetController', ['$scope', 'bladeNavigationService', 'gshopping_res_cat_items', function ($scope, bladeNavigationService, items) {
    $scope.syncItems = function () {
        return items.query({ catalogId: $scope.blade.parentBlade.catalogId, categoryId: $scope.blade.currentEntityId, }, function () {
        });
    }
}]);