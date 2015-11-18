angular.module('platformWebApp')
.controller('platformWebApp.notificationsListController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.notifications', function ($scope, bladeNavigationService, notifications) {
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
			title: 'platform.blades.notification-templates-list.title',
			notificationType: type,
			objectId: blade.objectId,
			objectTypeId: blade.objectTypeId,
			controller: 'platformWebApp.notificationTemplatesListController',
			template: '$(Platform)/Scripts/app/notifications/blades/notification-templates-list.tpl.html'
		};

		bladeNavigationService.showBlade(newBlade, blade);
	}

	blade.editTemplate = function (type) {
		var newBlade = {
			id: 'editTemplate',
			title: 'platform.blades.notifications-edit-template.title-new',
			notificationType: type,
			objectId: blade.objectId,
			objectTypeId: blade.objectTypeId,
			language: 'undefined',
			isNew: true,
			isFirst: true,
			usedLanguages: [],
			controller: 'platformWebApp.editTemplateController',
			template: '$(Platform)/Scripts/app/notifications/blades/notifications-edit-template.tpl.html'
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
