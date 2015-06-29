angular.module('platformWebApp')
.controller('platformWebApp.editTemplateController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.newnotifications', function ($scope, bladeNavigationService, dialogService, notifications) {
	var blade = $scope.blade;
	blade.selectedItemId = null;
	blade.currentEntities = [];

	blade.initialize = function () {
		notifications.getNotifications({}, function (data) {
			blade.currentEntities = data;
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