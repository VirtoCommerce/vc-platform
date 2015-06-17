angular.module('platformWebApp')
.controller('platformWebApp.demo.dashboard.notificationsWidgetController', ['$scope', 'platformWebApp.notificationService', function ($scope, notificationService) {
    $scope.notification = function (type) {
        var title = "Some notification text";
        var desc = "Some notification description";
        switch (type) {
            case 'error':
                notificationService.error({ title: title, description: desc });
                break;
            case 'warning':
                notificationService.warning({ title: title, description: desc });
                break;
            case 'info':
                notificationService.info({ title: title, description: desc });
                break;
            case 'task':
                notificationService.task({ title: title, description: desc });
                break;
        }
    };
}])
;