angular.module("platformWebApp")
.config(
  ['$stateProvider', function ($stateProvider) {
      $stateProvider
          .state('workspace.modulesSettings', {
              url: '/settings',
              templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
              controller: ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                  var blade = {
                      id: 'settings',
                      title: 'Settings',
                      //subtitle: 'Manage settings',
                      controller: 'platformWebApp.settingGroupListController',
                      template: '$(Platform)/Scripts/app/settings/blades/settingGroup-list.tpl.html',
                      isClosingDisabled: true
                  };
                  bladeNavigationService.showBlade(blade);
              }
              ]
          });
  }]
)
.run(
  ['$rootScope', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', function ($rootScope, mainMenuService, widgetService, $state) {
      //Register module in main menu
      var menuItem = {
          path: 'configuration/settings',
          icon: 'fa fa-gears',
          title: 'Settings',
          priority: 1,
          action: function () { $state.go('workspace.modulesSettings'); },
          permission: 'platform:setting:access'
      };
      mainMenuService.addMenuItem(menuItem);
  }]);
