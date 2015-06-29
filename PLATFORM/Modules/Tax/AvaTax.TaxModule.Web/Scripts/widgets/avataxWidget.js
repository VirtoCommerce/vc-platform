angular.module('virtoCommerce.avataxModule')
.controller('virtoCommerce.avataxModule.avataxWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.avataxModule.ping', function ($scope, bladeNavigationService, ping) {
    $scope.widget.refresh = function () {
        $scope.background = "transparent";
        $scope.message = "";
        bladeNavigationService.setError('', $scope.blade);
    }

    $scope.testConnection = function () {
        $scope.background = "transparent";
        $scope.message = "";
        $scope.blade.isLoading = true;
        ping.query(function () {
            $scope.background = "LightGreen";
            $scope.blade.isLoading = false;
            $scope.message = "Tested successfully!";
            bladeNavigationService.setError('', $scope.blade);
        }, function (error) {
            bladeNavigationService.setError('Error: ' + error.data.message.substring(0, 40) + '...', $scope.blade);
            $scope.background = "LightCoral";
            $scope.message = error.data.message;
        });
    }

    $scope.widget.refresh();
}]);