angular.module('platformWebApp')
.config(['$stateProvider', function ($stateProvider) {
    $stateProvider
        .state('workspace.dynamicProperties', {
            url: '/dynamicProperties',
            templateUrl: 'Scripts/common/templates/home.tpl.html',
            controller: ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                var blade = {
                    id: 'dynamicObjects',
                    title: 'Dynamic object types',
                    subtitle: 'Pick object type to manage dynamic properties',
                    controller: 'platformWebApp.dynamicObjectListController',
                    template: 'Scripts/app/dynamicProperties/blades/dynamicObject-list.tpl.html',
                    isClosingDisabled: true
                };
                bladeNavigationService.showBlade(blade);
            }]
        });
}])
.run(
  ['$rootScope', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', function ($rootScope, mainMenuService, widgetService, $state) {
      var menuItem = {
          path: 'dynamicProperties',
          icon: 'fa fa-plus-square-o',
          title: 'Dynamic properties',
          priority: 300,
          action: function () { $state.go('workspace.dynamicProperties'); },
          permission: 'platform:dynamic_properties:manage'
      };
      mainMenuService.addMenuItem(menuItem);
  }])
;