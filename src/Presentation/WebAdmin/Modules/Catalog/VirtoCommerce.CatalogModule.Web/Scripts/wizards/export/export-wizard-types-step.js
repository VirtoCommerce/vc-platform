angular.module('virtoCommerce.catalogModule')
.controller('exportTypesController', ['$scope', function ($scope) {
    $scope.blade.refresh = function () {
        $scope.list = [
            { name: "Product" },
            { name: "Category" }
        ];

        _.each($scope.blade.parentBlade.currentEntity.types, function (x) {
            var found = _.find($scope.list, function (xx) { return xx.name === x; });
            found.selected = true;
        });
    };

    $scope.isValid = function () {
        return _.any($scope.list, function (x) { return x.selected; });
    }

    $scope.saveChanges = function () {
        $scope.blade.parentBlade.currentEntity.types = _.pluck(_.where($scope.list, { selected: true }), 'name');
        $scope.bladeClose();
    }

    $scope.blade.refresh();
    $scope.blade.isLoading = false;
}]);
