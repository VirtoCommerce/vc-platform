//Call this to register our module to main application
var moduleName = "virtoCommerce.coreModule.settings";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [
    'virtoCommerce.coreModule.settings.blades'
])
.config(
  ['$stateProvider', function ($stateProvider) {
      $stateProvider
          .state('workspace.coreModulesettings', {
              url: '/settings',
              templateUrl: 'Modules/Core/VirtoCommerce.Core.Web/Scripts/home.tpl.html',
              controller: [
                  '$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
                      var blade = {
                          id: 'settings',
                          title: 'Settings',
                          //subtitle: 'Manage settings',
                          controller: 'settingsListController',
                          template: 'Modules/Core/VirtoCommerce.Core.Web/Scripts/Settings/blades/$settings-list.tpl.html',
                          isClosingDisabled: true
                      };
                      bladeNavigationService.showBlade(blade);
                  }
              ]
          });
  }]
)
.run(
  ['$rootScope', 'mainMenuService', 'widgetService', '$state', function ($rootScope, mainMenuService, widgetService, $state) {
      //Register module in main menu
      var menuItem = {
          path: 'browse/settings',
          icon: 'fa fa-cogs',
          title: 'Settings',
          priority: 190,
          action: function () { $state.go('workspace.coreModulesettings'); },
          permission: 'settingsMenuPermission'
      };
      mainMenuService.addMenuItem(menuItem);
  }])
;
