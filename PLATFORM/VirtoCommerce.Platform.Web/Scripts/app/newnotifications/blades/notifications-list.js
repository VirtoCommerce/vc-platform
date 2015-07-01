angular.module('platformWebApp')
.controller('platformWebApp.notificationsListController', ['$rootScope', '$scope', '$stateParams', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.newnotifications', function ($rootScope, $scope, $stateParams, bladeNavigationService, dialogService, notifications) {
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

	blade.openList = function (type, objectId, objectTypeId) {
		var newBlade = {
			id: 'templatesList',
			title: 'Notification templates',
			notificationType: type,
			objectId: objectId,
			objectTypeId: objectTypeId,
			controller: 'platformWebApp.notificationTemplatesListController',
			template: 'Scripts/app/newnotifications/blades/notification-templates-list.tpl.html'
		};

		bladeNavigationService.showBlade(newBlade, blade);
	}

	blade.editTemplate = function (type, objectId, objectTypeId) {
		var newBlade = {
			id: 'editTemplate',
			title: 'Edit notification template',
			notificationType: type,
			objectId: objectId,
			objectTypeId: objectTypeId,
			language: 'undefined',
			isNew: true,
			isFirst: true,
			usedLanguages: [],
			controller: 'platformWebApp.editTemplateController',
			template: 'Scripts/app/newnotifications/blades/notifications-edit-template.tpl.html'
		};

		bladeNavigationService.showBlade(newBlade, blade);
	};

	blade.openNotification = function (type) {
		var objectId = (angular.isUndefined($stateParams.objectId) || $stateParams.objectId === null) ? 'Platform' : $stateParams.objectId;
		var objectTypeId = (angular.isUndefined($stateParams.objectTypeId) || $stateParams.objectTypeId === null) ? 'Platform' : $stateParams.objectTypeId;
		notifications.getTemplates({ type: type, objectId: objectId, objectTypeId: objectTypeId }, function (data) {
			if (data.length > 0) {
				blade.openList(type, objectId, objectTypeId);
			}
			else {
				blade.editTemplate(type, objectId, objectTypeId);
			}
		});
	}

	blade.headIcon = 'fa-list';

	blade.initialize();
}]);
