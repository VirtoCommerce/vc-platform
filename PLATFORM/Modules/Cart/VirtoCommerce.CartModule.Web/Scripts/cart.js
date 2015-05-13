//Call this to register our module to main application
var moduleName = "virtoCommerce.cartModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
.config(
  ['$stateProvider', function ($stateProvider) {
      $stateProvider
          .state('workspace.cartModule', {
              url: '/carts',
              templateUrl: 'Scripts/common/templates/home.tpl.html',
              controller: [
                  '$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                      var blade = {
                          id: 'carts',
                          title: 'Shopping carts',
                          //subtitle: 'Manage Shopping carts',
                          controller: 'virtoCommerce.cartModule.cartListController',
                          template: 'Modules/$(VirtoCommerce.Cart)/Scripts/blades/carts-list.tpl.html',
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
          path: 'browse/carts',
          icon: 'fa fa-shopping-cart',
          title: 'Shopping carts',
          priority: 99,
          action: function () { $state.go('workspace.cartModule'); },
          permission: 'cart:query'
      };
      mainMenuService.addMenuItem(menuItem);
  }])
;
