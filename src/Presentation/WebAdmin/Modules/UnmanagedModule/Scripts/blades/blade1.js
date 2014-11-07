angular.module('unmanagedModule.blades.blade1', [])
.controller('um-blade1Controller', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {

    $scope.data = "unmanagedModule content";
    $scope.blade.title = "unmanagedModule title";
    $scope.blade.isLoading = false;
}]);
