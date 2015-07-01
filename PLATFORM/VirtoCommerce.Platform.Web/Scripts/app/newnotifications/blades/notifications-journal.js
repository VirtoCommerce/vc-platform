angular.module('platformWebApp')
.controller('platformWebApp.notificationsJournalController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.newnotifications', function ($scope, bladeNavigationService, notifications) {
	var blade = $scope.blade;
	blade.selectedItemId = null;
	blade.currentEntities = [];

	blade.initialize = function () {
		notifications.getNotificationJournalList({ objectId: blade.objectId, objectTypeId: blade.objectTypeId }, function (data) {
			blade.currentEntities = data;
			blade.isLoading = false;
		});
	};

	blade.openNotitification = function (data) {
		var newBlade = {
			id: 'notificationDetails',
			title: 'Details of notification',
			currentEntity: data,
			controller: 'platformWebApp.notificationDetails',
			template: 'Scripts/app/newnotifications/blades/notification-details.tpl.html'
		};

		bladeNavigationService.showBlade(newBlade, blade);
	}

	blade.initialize();
}]);