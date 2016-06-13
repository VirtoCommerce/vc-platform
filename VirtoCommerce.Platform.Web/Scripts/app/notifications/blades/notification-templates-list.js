angular.module('platformWebApp')
.controller('platformWebApp.notificationTemplatesListController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.notifications', 'platformWebApp.settings', function ($scope, bladeNavigationService, dialogService, notifications, settings) {
	var blade = $scope.blade;
	blade.selectedLanguage = null;

	if (!blade.languages) {
		blade.languages = settings.getValues({ id: 'VirtoCommerce.Core.General.Languages' });
	}

	blade.initialize = function () {
		blade.isLoading = true;
		notifications.getTemplates({ type: blade.notificationType, objectId: blade.objectId, objectTypeId: blade.objectTypeId }, function (data) {
			blade.currentEntities = data;
			if (blade.currentEntities.length < 1) {
				bladeNavigationService.closeBlade(blade);
			}
			blade.isLoading = false;
		});
	}

	blade.openTemplate = function (template) {
		blade.selectedLanguage = template.language;

		var newBlade = {
			id: 'editTemplate',
			title: 'platform.blades.notifications-edit-template.title',
			templateId: template.id,
			notificationType: blade.notificationType,
			isNew: false,
			isFirst: false,
			languages: blade.languages,
			controller: 'platformWebApp.editTemplateController',
			template: '$(Platform)/Scripts/app/notifications/blades/notifications-edit-template.tpl.html'
		};

		bladeNavigationService.showBlade(newBlade, blade);
	}

	function createTemplate(template) {
		var newBlade = {
			id: 'editTemplate',
			title: 'platform.blades.notifications-edit-template.title-new',
			notificationType: blade.notificationType,
			objectId: blade.objectId,
			objectTypeId: blade.objectTypeId,
			language: 'undefined',
			isNew: true,
			isFirst: false,
			languages: blade.languages,
			controller: 'platformWebApp.editTemplateController',
			template: '$(Platform)/Scripts/app/notifications/blades/notifications-edit-template.tpl.html'
		};

		bladeNavigationService.showBlade(newBlade, blade);
	}

	blade.toolbarCommands = [
			{
				name: "platform.commands.add", icon: 'fa fa-plus',
				executeMethod: createTemplate,
				canExecuteMethod: function () { return true; },
				permission: 'platform:notification:create'
			}
	];

	blade.initialize();
}]);