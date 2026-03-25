angular.module('platformWebApp').controller('platformWebApp.moduleInstallProgressController', ['$scope', 'platformWebApp.modulesApi', 'platformWebApp.WaitForRestart', 'platformWebApp.dialogService', '$timeout', function ($scope, modulesApi, waitForRestart, dialogService, $timeout) {
    var blade = $scope.blade;
    blade.subtitle = 'Installation progress';

    $scope.progressItems = [];
    $scope.progressStats = { total: 0, completed: 0, percent: 0 };
    $scope.showDetailedLog = false;

    function parseProgressLog(progressLog) {
        var items = [];
        var currentItem = null;

        _.each(progressLog, function (entry) {
            var msg = entry.message || '';

            // Detect new module operation start
            var installMatch = msg.match(/^Installing '(.+?)'/);
            var updateMatch = msg.match(/^Updating '(.+?)'/);
            var uninstallMatch = msg.match(/^Uninstalling '(.+?)'/);
            var startMatch = installMatch || updateMatch || uninstallMatch;

            if (startMatch) {
                // Close previous item if still active
                if (currentItem && currentItem.status === 'active') {
                    currentItem.status = 'done';
                }
                currentItem = {
                    id: items.length,
                    name: startMatch[1],
                    status: 'active',
                    messages: []
                };
                items.push(currentItem);
            } else if (entry.level === 'Error' && currentItem) {
                currentItem.status = 'error';
                currentItem.messages.push(entry);
            } else if (currentItem && currentItem.status === 'active') {
                // Detect completion messages
                if (msg.match(/^Successfully installed/) || msg.match(/uninstalled successfully/)) {
                    currentItem.status = 'done';
                }
                currentItem.messages.push(entry);
            }
        });

        // Compute stats
        var completed = _.filter(items, function (i) { return i.status === 'done' || i.status === 'error'; }).length;
        var total = items.length;

        $scope.progressItems = items;
        $scope.progressStats = {
            total: total,
            completed: completed,
            percent: total > 0 ? Math.round((completed / total) * 100) : 0
        };
    }

    $scope.$on("new-notification-event", function (event, notification) {
        if (blade.currentEntity && notification.id == blade.currentEntity.id) {
            angular.copy(notification, blade.currentEntity);
            parseProgressLog(blade.currentEntity.progressLog);
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
                        modulesApi.restart(function () {
                            //$window.location.reload(); returns 400 bad request due server restarts
                        });
                    }
                    catch (err) {
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
        dialogService.showWarningDialog(dialog);
    }

    // Parse initial state if progressLog already has entries
    if (blade.currentEntity && blade.currentEntity.progressLog) {
        parseProgressLog(blade.currentEntity.progressLog);
    }

    blade.isLoading = false;
}]);
