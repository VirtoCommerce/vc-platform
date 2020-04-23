angular.module('platformWebApp').controller('platformWebApp.moduleInstallProgressController', ['$scope', '$window', 'platformWebApp.bladeNavigationService', 'platformWebApp.modules', 'platformWebApp.WaitForRestart', 'platformWebApp.dialogService', '$timeout', function ($scope, $window, bladeNavigationService, modules, waitForRestart, dialogService, $timeout) {
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
        var dialog = {
            id: "confirmRestart",
            title: "platform.dialogs.app-restart.title",
            message: "platform.dialogs.app-restart.message",
            callback: function (confirm) {
                if (confirm) {
                    $scope.restarted = true;
                    blade.isLoading = true;
                    try {
                        modules.restart(function () {
                            //$window.location.reload(); returns 400 bad request due server restarts
                        });
                    }
                    catch (err){
                    }
                    finally {
                        // delay initial start for 3 seconds
                        $timeout(function () { }, 3000).then(function () {
                            return waitForRestart(1000);
                        });
                    }
                }
            }
        }
        dialogService.showAcceptanceDialog(dialog);
    }

    blade.isLoading = false;
}]);
