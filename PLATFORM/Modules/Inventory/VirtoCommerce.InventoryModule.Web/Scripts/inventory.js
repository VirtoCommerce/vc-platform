//Call this to register our module to main application
var moduleName = "virtoCommerce.inventoryModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
//.config(
//  ['$stateProvider', function ($stateProvider) {
//      $stateProvider
//          .state('workspace.inventoryModule', {
//              url: '/inventory',
//              templateUrl: 'Scripts/common/templates/home.tpl.html',
//              controller: [
//                  '$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
//                      var blade = {
//                          id: 'inventory',
//                          title: 'inventory',
//                          controller: 'virtoCommerce.inventoryModule.inventoryListController',
//                          template: 'Modules/$(VirtoCommerce.Inventory)/Scripts/blades/ -list.tpl.html',
//                          isClosingDisabled: true
//                      };
//                      bladeNavigationService.showBlade(blade);
//                  }
//              ]
//          });
//  }]
//)
.run(
  ['$rootScope', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', 'platformWebApp.authService', function ($rootScope, mainMenuService, widgetService, authService) {
      //Register module in main menu
      //var menuItem = {
      //    path: 'browse/inventory',
      //    icon: 'fa fa-shopping-cart',
      //    title: 'inventorys',
      //    priority: 110,
      //    action: function () { $state.go('workspace.inventoryModule'); },
      //    permission: 'inventory:manage'
      //};
      //mainMenuService.addMenuItem(menuItem);

      //Register widgets in catalog item details
      widgetService.registerWidget({
          isVisible: function (blade) { return blade.productType === 'Physical' && authService.checkPermission('inventory:manage'); },
          controller: 'virtoCommerce.inventoryModule.inventoryWidgetController',
          template: 'Modules/$(VirtoCommerce.Inventory)/Scripts/widgets/inventoryWidget.tpl.html'
      }, 'itemDetail');
  }])
;