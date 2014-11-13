angular.module('platformWebApp.module1.blades.blade1', [
    'platformWebApp.module1.resources.module1resources'])
.controller('blade1Controller', ['$scope', 'module1resource', function ($scope, module1resource) {
    $scope.blade.refresh = function () {
        module1resource.get(function (data) {
            $scope.blade.data = data;
            $scope.blade.title = data;
            $scope.blade.isLoading = false;
        });
    }
    $scope.blade.refresh();
}]);
