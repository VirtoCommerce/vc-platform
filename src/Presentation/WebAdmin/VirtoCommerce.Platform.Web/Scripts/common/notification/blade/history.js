angular.module('notifications.blades.history', [
   'catalogModule.resources.catalogs',
   'angularMoment'
])
.controller('notificationsHistoryController', ['$scope', 'bladeNavigationService', 'notificationDetailResolver', 'notifications',
function ($scope, bladeNavigationService, notificationDetailResolver, notifications) {

    $scope.pageSettings = {};
    $scope.pageSettings.totalItems = 0;
    $scope.pageSettings.currentPage = 1;
    $scope.pageSettings.numPages = 5;
    $scope.pageSettings.itemsPerPageCount = 10;
    $scope.pageSettings.orderBy = 'created';
    $scope.pageSettings.order = 'DESC';
    $scope.notifications = [];

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;
        var start = $scope.pageSettings.currentPage * $scope.pageSettings.itemsPerPageCount - $scope.pageSettings.itemsPerPageCount;
        notifications.query({start:start, count: $scope.pageSettings.itemsPerPageCount }, function (data, status, headers, config) {
            console.log(data);
            $scope.notifications = data.notifyEvents;
            $scope.pageSettings.totalItems = angular.isDefined(data.totalCount) ? data.totalCount : 0;
            $scope.blade.isLoading = false;
        });
    };

    $scope.setOrder = function(field) {
        if($scope.pageSettings.orderBy === field) {
            $scope.pageSettings.order = ($scope.pageSettings.order === 'DESC')?'ASC':'DESC';
        } else {
            $scope.pageSettings.orderBy = field;
            $scope.pageSettings.order = 'DESC';
        }
    }

    $scope.$watchGroup(['pageSettings.currentPage',
                        'pageSettings.orderBy',
                        'pageSettings.order'], function () {
        $scope.blade.refresh();
    });

	//Excecute notify detail action
    $scope.selectNotify = function (notify) {
    	var notifyDetail = notificationDetailResolver.resolve(notify);
    	if (angular.isDefined(notifyDetail))
    	{
    		notifyDetail.openDetailAction(notify);
    	}
    };

    // actions on load
    $scope.blade.refresh();
}]);