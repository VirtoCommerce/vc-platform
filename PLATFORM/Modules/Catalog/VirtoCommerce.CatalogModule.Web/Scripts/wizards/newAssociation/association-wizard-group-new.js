angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.associationGroupNewController', ['$scope', function ($scope) {

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.saveChanges = function () {
        $scope.blade.parentBlade.setSelected($scope.blade.name);
    }

    $scope.$on('$destroy', function () {
        $scope.blade.parentBlade.isNewGroup = false;
    });

    $scope.blade.name = null;
    $scope.blade.isLoading = false;
}]);
