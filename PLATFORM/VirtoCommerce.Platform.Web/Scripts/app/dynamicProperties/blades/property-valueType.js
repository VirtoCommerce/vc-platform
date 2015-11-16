angular.module('platformWebApp')
.controller('platformWebApp.propertyValueTypeController', ['$scope', function ($scope) {
    $scope.blade.title = 'platform.blades.property-valueType.title';
    $scope.blade.subtitle = 'platform.blades.property-valueType.subtitle';

    $scope.selectOption = function (option) {
        $scope.blade.currentEntity.valueType = option;
        $scope.bladeClose();
    };

    $scope.blade.isLoading = false;
}]);
