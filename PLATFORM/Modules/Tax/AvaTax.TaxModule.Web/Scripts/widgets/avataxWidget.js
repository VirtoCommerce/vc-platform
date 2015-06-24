angular.module('virtoCommerce.avataxModule')
.controller('virtoCommerce.avataxModule.avataxWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.avataxModule.ping', function ($scope, bladeNavigationService, ping) {
    $scope.widget.refresh = function () {
        $scope.result = "transparent";
    }

    $scope.testConnection = function () {
        $scope.result = "transparent";
        $scope.blade.isLoading = true;
        ping.query(function () {
            $scope.result = "green";
            $scope.blade.isLoading = false;
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
            $scope.result = "red";
        });
    }

    $scope.widget.refresh();
}]);