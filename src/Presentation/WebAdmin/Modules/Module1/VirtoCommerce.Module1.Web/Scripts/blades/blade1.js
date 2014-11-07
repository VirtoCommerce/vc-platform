angular.module('platformWebApp.module1.blades.blade1', [
    'platformWebApp.module1.resources.module1resources'])
.controller('blade1Controller', ['$scope', 'bladeNavigationService', 'module1resource', function ($scope, bladeNavigationService, module1resource) {
    $scope.currentBlade = bladeNavigationService.currentBlade;

    $scope.currentBlade.refresh = function () {
        module1resource.get(function (data) {
            $scope.currentBlade.data = data;
            $scope.currentBlade.title = data;
            $scope.currentBlade.isLoading = false;
        });
    }
    $scope.currentBlade.refresh();
}]);
