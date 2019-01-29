angular.module('platformWebApp')
.controller('platformWebApp.moduleInstallProgressController', ['$scope', '$window', 'platformWebApp.bladeNavigationService', 'platformWebApp.modules', 'platformWebApp.dialogService', function ($scope, $window, bladeNavigationService, modules, dialogService) {
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

    $scope.$watch('blade.currentEntity.finished', function (data) {
        if (!data) {
            return;
        }

        var dialog = {
            id: "restartRequired",
            title: "platform.dialogs.app-restart-required.title",
            message: "platform.dialogs.app-restart-required.message",
            callback: function (confirmed) {
                if (confirmed) {
                    $scope.restart();
                }
            }
        };
        dialogService.showConfirmationDialog(dialog);
    });

    blade.isLoading = false;
}]);
