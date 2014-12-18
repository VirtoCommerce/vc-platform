angular.module('notifications.blades.history', [
   'catalogModule.resources.catalogs',
   'angularMoment'
])
.controller('notificationsHistoryController', ['$scope', 'bladeNavigationService', 'notificationTemplateResolver', 'notifications',
function ($scope, bladeNavigationService, notificationTemplateResolver, notifications) {

    $scope.notifications = [];
    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

        notifications.query({start:0, count: 1000 }, function (data, status, headers, config) {
            $scope.notifications = data.notifyEvents;
            $scope.blade.isLoading = false;
        });
    };

	//Excecute notify detail action
    $scope.selectNotify = function (notify) {
    	var notificationTemplate = notificationTemplateResolver.resolve(notify);
    	if (angular.isDefined(notificationTemplate))
    	{
    		notificationTemplate.action(notify);
    	}
    };

    // actions on load
    $scope.blade.refresh();
}]);