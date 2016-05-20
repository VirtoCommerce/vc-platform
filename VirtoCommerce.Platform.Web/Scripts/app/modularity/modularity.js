angular.module('platformWebApp')
.config(['$stateProvider', function ($stateProvider) {
    $stateProvider
        .state('workspace.modularity', {
            url: '/modules',
            templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
            controller: ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                var blade = {
                    id: 'modulesMain',
                    title: 'platform.blades.modules-main.title',
                    controller: 'platformWebApp.modulesMainController',
                    template: '$(Platform)/Scripts/app/modularity/blades/modules-main.tpl.html',
                    isClosingDisabled: true
                };
                bladeNavigationService.showBlade(blade);
            }]
        });
}])
.run(
  ['platformWebApp.pushNotificationTemplateResolver', 'platformWebApp.bladeNavigationService', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', function (pushNotificationTemplateResolver, bladeNavigationService, mainMenuService, widgetService, $state) {
      //Register module in main menu
      var menuItem = {
          path: 'configuration/modularity',
          icon: 'fa fa-cubes',
          title: 'platform.menu.modules',
          priority: 6,
          action: function () { $state.go('workspace.modularity'); },
          permission: 'platform:module:access'
      };
      mainMenuService.addMenuItem(menuItem);

      //Push notifications
      var menuExportImportTemplate =
         {
             priority: 900,
             satisfy: function (notify, place) { return place == 'menu' && notify.notifyType == 'ModulePushNotification'; },
             template: '$(Platform)/Scripts/app/modularity/notifications/menu.tpl.html',
             action: function (notify) { $state.go('pushNotificationsHistory', notify); }
         };
      pushNotificationTemplateResolver.register(menuExportImportTemplate);

      var historyExportImportTemplate =
	  {
	      priority: 900,
	      satisfy: function (notify, place) { return place == 'history' && notify.notifyType == 'ModulePushNotification'; },
	      template: '$(Platform)/Scripts/app/modularity/notifications/history.tpl.html',
	      action: function (notify) {
	          var blade = {
	              id: 'moduleInstallProgress',
	              title: notify.title,
	              currentEntity: notify,
	              controller: 'platformWebApp.moduleInstallProgressController',
	              template: '$(Platform)/Scripts/app/modularity/wizards/newModule/module-wizard-progress-step.tpl.html'
	          };
	          bladeNavigationService.showBlade(blade);
	      }
	  };
      pushNotificationTemplateResolver.register(historyExportImportTemplate);
  }])
.factory('platformWebApp.moduleHelper', function () {
    // semver comparison: https://gist.github.com/TheDistantSea/8021359
    return {};
})
;
