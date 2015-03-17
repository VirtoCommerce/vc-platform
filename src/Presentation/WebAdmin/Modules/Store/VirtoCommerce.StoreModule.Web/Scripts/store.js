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
              templateUrl: 'Modules/$(VirtoCommerce.Store)/Scripts/home.tpl.html',
              controller: [
                  '$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
                      var blade = {
                          id: 'store',
                          title: 'Stores',
                          controller: 'storesListController',
                          template: 'Modules/$(VirtoCommerce.Store)/Scripts/blades/!stores-list.tpl.html',
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
          icon: 'fa fa-archive',
          title: 'Stores',
          priority: 110,
          action: function () { $state.go('workspace.storeModule'); },
          permission: 'storesMenuPermission'
      };
      mainMenuService.addMenuItem(menuItem);

      //Register widgets in store details
      widgetService.registerWidget({
          controller: 'storeLanguagesWidgetController',
          template: 'Modules/$(VirtoCommerce.Store)/Scripts/widgets/storeLanguagesWidget.tpl.html'
      }, 'storeDetail');
      widgetService.registerWidget({
          controller: 'storeCurrenciesWidgetController',
          template: 'Modules/$(VirtoCommerce.Store)/Scripts/widgets/storeCurrenciesWidget.tpl.html'
      }, 'storeDetail');
      widgetService.registerWidget({
          controller: 'storeAdvancedWidgetController',
          template: 'Modules/$(VirtoCommerce.Store)/Scripts/widgets/storeAdvancedWidget.tpl.html'
      }, 'storeDetail');
      widgetService.registerWidget({
          controller: 'storeSettingsWidgetController',
          template: 'Modules/$(VirtoCommerce.Store)/Scripts/widgets/storeSettingsWidget.tpl.html'
      }, 'storeDetail');
      widgetService.registerWidget({
          controller: 'storePaymentsWidgetController',
          template: 'Modules/$(VirtoCommerce.Store)/Scripts/widgets/storePaymentsWidget.tpl.html'
      }, 'storeDetail');
  }])
;