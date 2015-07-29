angular.module('platformWebApp')
.controller('platformWebApp.demo.dashboard.notificationsWidgetController', ['$scope', 'platformWebApp.pushNotificationService', function ($scope, eventService) {
    $scope.notification = function (type) {
        var title = "Some notification text";
        var desc = "Some notification description";
        switch (type) {
            case 'error':
                eventService.error({ title: title, description: desc });
                break;
            case 'warning':
                eventService.warning({ title: title, description: desc });
                break;
            case 'info':
                eventService.info({ title: title, description: desc });
                break;
            case 'task':
                eventService.task({ title: title, description: desc });
                break;
        }
    };
}])
;