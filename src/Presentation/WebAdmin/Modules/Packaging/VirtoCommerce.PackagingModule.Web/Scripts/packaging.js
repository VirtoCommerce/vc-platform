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
              templateUrl: 'Modules/Packaging/VirtoCommerce.PackagingModule.Web/Scripts/home.tpl.html',
              controller: [
                  '$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
                      var blade = {
                          id: 'modules',
                          title: 'Modules',
                          subtitle: 'Manage installed modules',
                          controller: 'modulesListController',
                          template: 'Modules/Packaging/VirtoCommerce.PackagingModule.Web/Scripts/blades/modules-list.tpl.html',
                          isClosingDisabled: true
                      };
                      bladeNavigationService.showBlade(blade);
                  }
              ]
          });
  }]
)
.run(
  ['$rootScope', 'mainMenuService', 'widgetService', function ($rootScope, mainMenuService, widgetService) {
      //Register module in main menu
      var menuItem = {
          path: 'browse/catalog',
          icon: 'glyphicon glyphicon-cog',
          title: 'Modules',
          priority: 200,
          state: 'workspace.packaging',
          permission: 'modulesMenuPermission'
      };
      mainMenuService.addMenuItem(menuItem);
  }])
;
