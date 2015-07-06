angular.module('platformWebApp')
.config(['$stateProvider', function ($stateProvider) {
    $stateProvider
        .state('workspace.exportImport', {
            url: '/exportImport',
            templateUrl: 'Scripts/common/templates/home.tpl.html',
            controller: ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                var blade = {
                    id: 'exportImport',
                    title: 'Data export and import',
                    controller: 'platformWebApp.exportImport.mainController',
                    template: 'Scripts/app/exportImport/blades/exportImport-main.tpl.html',
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
      var menuItem = {
          path: 'browse/exportImport',
          icon: 'fa fa-database',
          title: 'Export & Import',
          priority: 210,
          action: function () { $state.go('workspace.exportImport'); },
          permission: 'platform:backupAdministrator'
      };
      mainMenuService.addMenuItem(menuItem);
  }])
;
