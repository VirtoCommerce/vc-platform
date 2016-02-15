angular.module('platformWebApp')
.controller('platformWebApp.notificationsJournalController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.notifications', function ($scope, bladeNavigationService, notifications) {
    var blade = $scope.blade;
    blade.selectedItemId = null;
    blade.currentEntities = [];
    blade.checkedIds = [];
    blade.selectAll = true;

    $scope.pageSettings = {};
    $scope.pageSettings.totalItems = 0;
    $scope.pageSettings.currentPage = 1;
    $scope.pageSettings.numPages = 5;
    $scope.pageSettings.itemsPerPageCount = 20;

    blade.refresh = function () {
        var start = $scope.pageSettings.currentPage * $scope.pageSettings.itemsPerPageCount - $scope.pageSettings.itemsPerPageCount;
        notifications.getNotificationJournalList({ objectId: blade.objectId, objectTypeId: blade.objectTypeId, start: start, count: $scope.pageSettings.itemsPerPageCount }, function (data) {
            blade.currentEntities = data.notifications;
            $scope.pageSettings.totalItems = data.totalCount;
            blade.selectedAll = false;
            blade.isLoading = false;
        });
    };

    blade.openNotification = function (data) {
        var newBlade = {
            id: 'notificationDetails',
            title: 'platform.blades.notification-journal-details.title',
            currentNotificationId: data.id,
            currentEntity: data,
            controller: 'platformWebApp.notificationsJournalDetailtsController',
            template: '$(Platform)/Scripts/app/notifications/blades/notification-journal-details.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.stopNotification = function (id) {
        blade.isLoading = true;
        for (var i = 0; i < blade.currentEntities.length; i++) {
            if (blade.currentEntities[i].selected) {
                blade.checkedIds.push(blade.currentEntities[i].id);
            }
        }

        notifications.stopSendingNotifications({}, blade.checkedIds, function (data) {
            blade.refresh();
            blade.checkedIds = [];
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    }

    blade.checkAll = function (selectAll) {
        for (var i = 0; i < blade.currentEntities.length; i++) {
            blade.currentEntities[i].selected = selectAll;
        }
    }

    $scope.$watch('pageSettings.currentPage', blade.refresh);

    blade.toolbarCommands = [
		{
		    name: "platform.commands.stop-sending", icon: 'fa fa-stop',
		    executeMethod: function () {
		        blade.stopNotification();
		    },
		    canExecuteMethod: function () {
		        return blade.selectedAll || !angular.isUndefined(_.find(blade.currentEntities, function (entity) { return entity.selected; }));
		    }
		}
    ];

    //blade.refresh();
}]);