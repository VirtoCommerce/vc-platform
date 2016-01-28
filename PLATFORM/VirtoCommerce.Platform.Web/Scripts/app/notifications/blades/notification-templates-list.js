angular.module('platformWebApp')
.controller('platformWebApp.notificationTemplatesListController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.notifications', function ($scope, bladeNavigationService, dialogService, notifications) {
    var blade = $scope.blade;
    blade.selectedLanguage = null;

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
            notificationType: blade.notificationType,
            objectId: blade.objectId,
            objectTypeId: blade.objectTypeId,
            language: template.language,
            isNew: false,
            isFirst: false,
            usedLanguages: _.pluck(blade.currentEntities, 'language'),
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
            usedLanguages: _.pluck(blade.currentEntities, 'language'),
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