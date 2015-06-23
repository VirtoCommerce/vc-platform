angular.module("platformWebApp")
.config(
  ['$stateProvider', function ($stateProvider) {
      $stateProvider
          .state('workspace.modulesSettings', {
              url: '/settings',
              templateUrl: 'Scripts/common/templates/home.tpl.html',
              controller: ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                  var blade = {
                      id: 'settings',
                      title: 'Settings',
                      //subtitle: 'Manage settings',
                      controller: 'platformWebApp.settingGroupListController',
                      template: 'Scripts/app/settings/blades/settingGroup-list.tpl.html',
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
          path: 'browse/settings',
          icon: 'fa fa-wrench',
          title: 'Settings',
          priority: 196,
          action: function () { $state.go('workspace.modulesSettings'); },
          permission: 'platform:setting:manage'
      };
      mainMenuService.addMenuItem(menuItem);
  }])
;
