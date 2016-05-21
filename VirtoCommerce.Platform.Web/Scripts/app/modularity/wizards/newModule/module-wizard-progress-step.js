angular.module('platformWebApp')
.controller('platformWebApp.moduleInstallProgressController', ['$scope', '$window', 'platformWebApp.bladeNavigationService', 'platformWebApp.modules', function ($scope, $window, bladeNavigationService, modules) {
    var blade = $scope.blade;
    blade.subtitle = 'Installation progress';

    $scope.$on("new-notification-event", function (event, notification) {
        if (blade.currentEntity && notification.id == blade.currentEntity.id) {
            angular.copy(notification, blade.currentEntity);
            if (notification.finished) {
                blade.isLoading = false;
                if (_.any(notification.progressLog) && _.last(notification.progressLog).level !== 'Error') {
                    blade.parentBlade.refresh();
                }
            }
        }
    });

    $scope.restart = function () {
        $scope.restarted = true;
        modules.restart({},
            function () { $window.location.reload(); },
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    };

    blade.isLoading = false;
}]);
