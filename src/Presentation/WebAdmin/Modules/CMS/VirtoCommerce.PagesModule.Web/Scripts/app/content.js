//Call this to register our module to main application
var moduleName = "virtoCommerce.content";
var tempateRoot = "Modules/Content/VirtoCommerce.ContentModule.Web/Scripts/app";
var navigationId = "contents";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [
    'virtoCommerce.content.blades.collectionList'
])
.config(
  ['$stateProvider', function ($stateProvider) {
      $stateProvider
          .state('workspace.content', {
              url: '/' + navigationId,
              templateUrl: tempateRoot + '/home.tpl.html',
              controller: [
                  '$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
                      var blade = {
                          id: navigationId,
                          title: 'Collections',
                          subtitle: 'Pick content collection',
                          controller: 'collectionListController',
                          template: tempateRoot + '/blades/collection-list.tpl.html',
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
          path: 'browse/' + navigationId,
          icon: 'glyphicon glyphicon-cog',
          title: 'Contents',
          priority: 200,
          action: function () { $state.go('workspace.content') },
          permission: 'contentsMenuPermission'
      };
      mainMenuService.addMenuItem(menuItem);
  }])
;
