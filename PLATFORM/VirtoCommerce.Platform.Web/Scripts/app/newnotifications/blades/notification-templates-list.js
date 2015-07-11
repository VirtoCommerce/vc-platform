angular.module('platformWebApp')
.controller('platformWebApp.notificationTemplatesListController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.newnotifications', function ($scope, bladeNavigationService, dialogService, notifications) {
	var blade = $scope.blade;
	blade.selectedLanguage = null;

	blade.initialize = function () {
		blade.isLoading = true;
		notifications.getTemplates({ type: blade.notificationType, objectId: blade.objectId, objectTypeId: blade.objectTypeId }, function (data) {
			blade.currentEntities = data;
			blade.isLoading = false;
		});
	}

	blade.openTemplate = function (template) {
		blade.selectedLanguage = template.language;

		var newBlade = {
			id: 'editTemplate',
			title: 'Edit notification template',
			notificationType: blade.notificationType,
			objectId: blade.objectId,
			objectTypeId: blade.objectTypeId,
			language: template.language,
			isNew: false,
			isFirst: false,
			usedLanguages: _.pluck(blade.currentEntities, 'language'),
			controller: 'platformWebApp.editTemplateController',
			template: 'Scripts/app/newnotifications/blades/notifications-edit-template.tpl.html'
		};

		bladeNavigationService.showBlade(newBlade, blade);
	}

	blade.createTemplate = function (template) {
		var newBlade = {
			id: 'editTemplate',
			title: 'Create notification template',
			notificationType: blade.notificationType,
			objectId: blade.objectId,
			objectTypeId: blade.objectTypeId,
			language: 'undefined',
			isNew: true,
			isFirst: false,
			usedLanguages: _.pluck(blade.currentEntities, 'language'),
			controller: 'platformWebApp.editTemplateController',
			template: 'Scripts/app/newnotifications/blades/notifications-edit-template.tpl.html'
		};

		bladeNavigationService.showBlade(newBlade, blade);
	}

	$scope.blade.toolbarCommands = [
			{
				name: "Add", icon: 'fa fa-plus',
				executeMethod: function () {
					blade.createTemplate();
				},
				canExecuteMethod: function () {
					return true;
				}
			}
	];

	blade.getFlag = function (x) {
		switch (x) {
			case 'ru-RU':
				return 'ru';

			case 'en-US':
				return 'us';

			case 'fr-FR':
				return 'fr';

			case 'zh-CN':
				return 'ch';

			case 'ru-RU':
				return 'ru';

			case 'ja-JP':
				return 'jp';

			case 'de-DE':
				return 'de';
		}
	}

	blade.initialize();
}]);