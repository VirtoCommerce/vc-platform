angular.module('platformWebApp')
.controller('platformWebApp.notificationsJournalDetailtsController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.notifications', function ($scope, bladeNavigationService, notifications) {
	var blade = $scope.blade;

	blade.initialize = function () {
		notifications.getNotificationJournalDetails({ id: blade.currentNotificationId }, function (data) {
			blade.currentEntity = data;
			blade.isLoading = false;
		}, function (error) {
			bladeNavigationService.setError('Error ' + error.status, $scope.blade);
		});
	}

	blade.stopNotification = function (id) {
		blade.isLoading = true;
		notifications.stopSendingNotifications({}, [blade.currentNotificationId], function (data) {
			blade.initialize();
			blade.parentBlade.initialize();
		}, function (error) {
			bladeNavigationService.setError('Error ' + error.status, $scope.blade);
		});
	}

	$scope.blade.toolbarCommands = [
		{
		    name: "platform.commands.stop-sending", icon: 'fa fa-stop',
			executeMethod: function () {
				blade.stopNotification(blade.currentEntity.id);
			},
			canExecuteMethod: function () {
				return blade.currentEntity.isActive;
			}
		}
	];

	blade.initialize();
}]);