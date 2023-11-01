angular.module('platformWebApp')
    .controller('platformWebApp.pushNotifications.historyDetailDefaultController', [
        '$scope',
        'platformWebApp.bladeNavigationService',
        function (
            $scope,
            bladeNavigationService) {
            var blade = $scope.blade;
            blade.isLoading = true;
            $scope.blade.headIcon = 'fa fa-folder';

            function initializeBlade() {
                blade.isLoading = false;
            }

            $scope.$on("new-notification-event", function (event, notification) {
                if (blade.notification && notification.id === blade.notification.id) {
                    angular.copy(notification, blade.notification);
                    if (notification.errorCount > 0) {
                        bladeNavigationService.setError('Action error', blade);
                    }

                    if (blade.notification.finished) {
                        if (blade.onCompleted) {
                            blade.onCompleted();
                        }
                    }
                }
            });

            blade.toolbarCommands = [];
            initializeBlade();
        }]);
