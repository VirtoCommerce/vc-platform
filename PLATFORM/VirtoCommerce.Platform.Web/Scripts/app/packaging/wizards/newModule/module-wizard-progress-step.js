angular.module('platformWebApp')
.controller('platformWebApp.moduleInstallProgressController', ['$scope', '$interval', '$window', 'platformWebApp.bladeNavigationService', 'platformWebApp.modules', function ($scope, $interval, $window, bladeNavigationService, modules) {
    $scope.blade.refresh = function () {
        // $scope.blade.isLoading = true;

        modules.getInstallationStatus({ id: $scope.blade.currentEntityId }, function (results) {
            $scope.currentEntity = results;
            if (results.completed) {
                $scope.blade.isLoading = false;
                $scope.completed = true;
                stopRefresh();
                $scope.blade.parentBlade.refresh();
            }
        });

    };

    function stopRefresh() {
        if (angular.isDefined(intervalPromise)) {
            $interval.cancel(intervalPromise);
        }
    };

    $scope.$on('$destroy', function () {
        // Make sure that the interval is destroyed too
        stopRefresh();
    });

    $scope.restart = function () {
        modules.restart({}, function () {
            $window.location.reload();
        });
    }

    $scope.blade.refresh();
    var intervalPromise = $interval($scope.blade.refresh, 1500);
}]);
