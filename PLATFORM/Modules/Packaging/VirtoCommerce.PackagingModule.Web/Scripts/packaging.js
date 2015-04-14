//Call this to register our module to main application
var moduleName = "virtoCommerce.packaging";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [
    'virtoCommerce.packaging.blades.modulesList'
])
.config(
  ['$stateProvider', function ($stateProvider) {
      $stateProvider
          .state('workspace.packaging', {
              url: '/modules',
              templateUrl: 'Modules/$(VirtoCommerce.Packaging)/Scripts/home.tpl.html',
              controller: [
                  '$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
                      var blade = {
                          id: 'modules',
                          title: 'Modules',
                          subtitle: 'Manage installed modules',
                          controller: 'modulesListController',
                          template: 'Modules/$(VirtoCommerce.Packaging)/Scripts/blades/modules-list.tpl.html',
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
          path: 'browse/packaging',
          icon: 'fa fa-cubes',
          title: 'Modules',
          priority: 200,
          action: function () { $state.go('workspace.packaging'); },
          permission: 'modulesMenuPermission'
      };
      mainMenuService.addMenuItem(menuItem);
  }])
;
