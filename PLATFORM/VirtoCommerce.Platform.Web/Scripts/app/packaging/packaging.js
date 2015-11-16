angular.module(moduleName)
.config(
  ['$stateProvider', function ($stateProvider) {
      $stateProvider
          .state('workspace.packaging', {
              url: '/modules',
              templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
              controller: ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                  var blade = {
                      id: 'modules',
                      title: 'platform.blades.modules-list.title',
                      subtitle: 'platform.blades.modules-list.subtitle',
                      controller: 'platformWebApp.modulesListController',
                      template: '$(Platform)/Scripts/app/packaging/blades/modules-list.tpl.html',
                      isClosingDisabled: true
                  };
                  bladeNavigationService.showBlade(blade);
              }
              ]
          });
  }]
)
.run(
  ['platformWebApp.pushNotificationTemplateResolver', 'platformWebApp.bladeNavigationService', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', function (pushNotificationTemplateResolver, bladeNavigationService, mainMenuService, widgetService, $state) {
      //Register module in main menu
      var menuItem = {
          path: 'configuration/packaging',
          icon: 'fa fa-cubes',
          title: 'platform.menu.modules',
          priority: 6,
          action: function () { $state.go('workspace.packaging'); },
          permission: 'platform:module:access'
      };
      mainMenuService.addMenuItem(menuItem);

      //Push notifications
      var menuExportImportTemplate =
         {
             priority: 900,
             satisfy: function (notify, place) { return place == 'menu' && notify.notifyType == 'ModulePushNotification'; },
             template: '$(Platform)/Scripts/app/packaging/notifications/menu.tpl.html',
             action: function (notify) { $state.go('pushNotificationsHistory', notify); }
         };
      pushNotificationTemplateResolver.register(menuExportImportTemplate);

      var historyExportImportTemplate =
	  {
	      priority: 900,
	      satisfy: function (notify, place) { return place == 'history' && notify.notifyType == 'ModulePushNotification'; },
	      template: '$(Platform)/Scripts/app/packaging/notifications/history.tpl.html',
	      action: function (notify) {
	          var blade = {
	              id: 'moduleInstallProgress',
	              title: notify.title,
	              currentEntity: notify,
	              controller: 'platformWebApp.moduleInstallProgressController',
	              template: '$(Platform)/Scripts/app/packaging/wizards/newModule/module-wizard-progress-step.tpl.html'
	          };
	          bladeNavigationService.showBlade(blade);
	      }
	  };
      pushNotificationTemplateResolver.register(historyExportImportTemplate);
  }])
;
