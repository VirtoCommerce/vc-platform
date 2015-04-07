//Call this to register our module to main application
var moduleName = "virtoCommerce.marketingModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
.config(
  ['$stateProvider', function ($stateProvider) {
      $stateProvider
          .state('workspace.marketingModule', {
              url: '/marketing',
              templateUrl: 'Modules/$(VirtoCommerce.Core)/Scripts/home.tpl.html',
              controller: [
                  '$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
                      var blade = {
                          id: 'marketing',
                          title: 'Marketing',
                          controller: 'marketingMainController',
                          template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/common/marketing-main.tpl.html',
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
          path: 'browse/marketing',
          icon: 'fa fa-flag',
          title: 'Marketing',
          priority: 40,
          action: function () { $state.go('workspace.marketingModule'); },
          permission: 'marketingMenuPermission'
      };
      mainMenuService.addMenuItem(menuItem);

      //Register widgets...
  }]);