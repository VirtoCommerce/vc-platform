angular.module('platformWebApp')
.controller('platformWebApp.notificationsListController', ['$rootScope', '$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.newnotifications', function ($rootScope, $scope, bladeNavigationService, dialogService, notifications) {
	$scope.selectedEntityId = null;
	var blade = $scope.blade;

	blade.initialize = function () {
		blade.isLoading = true;

		notifications.getNotificationList({}, function (results) {
			blade.isLoading = false;
			blade.currentEntities = results;
		}, function (error) {
			bladeNavigationService.setError('Error ' + error.status, blade);
		});
	};

	blade.editTemplate = function (data) {
		var newBlade = {
					id: 'editTemplate',
					title: 'Edit notification template',
					currentEntityParent: data,
					controller: 'platformWebApp.editTemplateController',
					template: 'Scripts/app/newnotifications/blades/notifications-edit-template.tpl.html'
				};

				bladeNavigationService.showBlade(newBlade, blade);
	};

	blade.initialize();
}]);
