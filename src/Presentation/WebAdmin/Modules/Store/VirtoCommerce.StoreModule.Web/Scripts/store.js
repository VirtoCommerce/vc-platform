//Call this to register our module to main application
var moduleName = "virtoCommerce.storeModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [
    'virtoCommerce.storeModule.blades',
    'virtoCommerce.storeModule.widgets'
])
.config(
  ['$stateProvider', function ($stateProvider) {
      $stateProvider
          .state('workspace.storeModule', {
              url: '/store',
              templateUrl: 'Modules/Store/VirtoCommerce.StoreModule.Web/Scripts/home.tpl.html',
              controller: [
                  '$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
                      var blade = {
                          id: 'store',
                          title: 'Stores',
                          controller: 'storesListController',
                          template: 'Modules/Store/VirtoCommerce.StoreModule.Web/Scripts/blades/!stores-list.tpl.html',
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
          path: 'browse/store',
          icon: 'fa fa-shopping-cart',
          title: 'Stores',
          priority: 110,
          action: function () { $state.go('workspace.storeModule'); },
          permission: 'storesMenuPermission'
      };
      mainMenuService.addMenuItem(menuItem);

      //Register widgets in store details
      widgetService.registerWidget({
          group: 'storeDetail',
          controller: 'storeLanguagesWidgetController',
          template: 'Modules/Store/VirtoCommerce.StoreModule.Web/Scripts/widgets/storeLanguagesWidget.tpl.html'
      });
      widgetService.registerWidget({
          group: 'storeDetail',
          controller: 'storeAdvancedWidgetController',
          template: 'Modules/Store/VirtoCommerce.StoreModule.Web/Scripts/widgets/storeAdvancedWidget.tpl.html'
      });
      widgetService.registerWidget({
          group: 'storeDetail',
          controller: 'storeSettingsWidgetController',
          template: 'Modules/Store/VirtoCommerce.StoreModule.Web/Scripts/widgets/storeSettingsWidget.tpl.html'
      });

  }])
;