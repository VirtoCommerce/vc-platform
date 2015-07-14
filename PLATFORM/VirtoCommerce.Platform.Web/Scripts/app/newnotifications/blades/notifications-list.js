angular.module('platformWebApp')
.controller('platformWebApp.notificationsListController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.newnotifications', function ($scope, bladeNavigationService, notifications) {
	var blade = $scope.blade;
	blade.selectedType = null;

	blade.initialize = function () {
		blade.isLoading = true;

		notifications.getNotificationList({}, function (results) {
			blade.isLoading = false;
			blade.currentEntities = results;
		}, function (error) {
			bladeNavigationService.setError('Error ' + error.status, blade);
		});
	};

	blade.openList = function (type) {
		var newBlade = {
			id: 'templatesList',
			title: 'Notification templates',
			notificationType: type,
			objectId: blade.objectId,
			objectTypeId: blade.objectTypeId,
			controller: 'platformWebApp.notificationTemplatesListController',
			template: 'Scripts/app/newnotifications/blades/notification-templates-list.tpl.html'
		};

		bladeNavigationService.showBlade(newBlade, blade);
	}

	blade.editTemplate = function (type) {
		var newBlade = {
			id: 'editTemplate',
			title: 'Create notification template',
			notificationType: type,
			objectId: blade.objectId,
			objectTypeId: blade.objectTypeId,
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
		blade.selectedType = type;
		notifications.getTemplates({ type: type, objectId: blade.objectId, objectTypeId: blade.objectTypeId }, function (data) {
			if (data.length > 0) {
				blade.openList(type);
			}
			else {
				blade.editTemplate(type);
			}
		});
	}

	blade.headIcon = 'fa-list';

	blade.initialize();
}]);
