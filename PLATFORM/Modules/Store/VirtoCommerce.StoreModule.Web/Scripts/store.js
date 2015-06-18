//Call this to register our module to main application
var moduleName = "virtoCommerce.storeModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [
    // 'catalogModule.resources.catalogs'
    'ngSanitize'
])
.config(
  ['$stateProvider', function ($stateProvider) {
      $stateProvider
          .state('workspace.storeModule', {
              url: '/store',
              templateUrl: 'Scripts/common/templates/home.tpl.html',
              controller: [
                  '$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                      var blade = {
                          id: 'store',
                          title: 'Stores',
                          controller: 'virtoCommerce.storeModule.storesListController',
                          template: 'Modules/$(VirtoCommerce.Store)/Scripts/blades/stores-list.tpl.html',
                          isClosingDisabled: true
                      };
                      bladeNavigationService.showBlade(blade);
                  }
              ]
          });
  }]
)
.run(
  ['platformWebApp.toolbarService', 'platformWebApp.bladeNavigationService', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', function (toolbarService, bladeNavigationService, mainMenuService, widgetService, $state) {
      //Register module in main menu
      var menuItem = {
          path: 'browse/store',
          icon: 'fa fa-archive',
          title: 'Stores',
          priority: 110,
          action: function () { $state.go('workspace.storeModule'); },
          permission: 'store:query'
      };
      mainMenuService.addMenuItem(menuItem);

      //Register widgets in store details
      widgetService.registerWidget({
          controller: 'virtoCommerce.storeModule.storeLanguagesWidgetController',
          template: 'Modules/$(VirtoCommerce.Store)/Scripts/widgets/storeLanguagesWidget.tpl.html'
      }, 'storeDetail');
      widgetService.registerWidget({
          controller: 'virtoCommerce.storeModule.storeCurrenciesWidgetController',
          template: 'Modules/$(VirtoCommerce.Store)/Scripts/widgets/storeCurrenciesWidget.tpl.html'
      }, 'storeDetail');
      widgetService.registerWidget({
          controller: 'virtoCommerce.storeModule.seoWidgetController',
          template: 'Modules/$(VirtoCommerce.Store)/Scripts/widgets/seoWidget.tpl.html',
      }, 'storeDetail');
      widgetService.registerWidget({
          controller: 'virtoCommerce.storeModule.storeAdvancedWidgetController',
          template: 'Modules/$(VirtoCommerce.Store)/Scripts/widgets/storeAdvancedWidget.tpl.html'
      }, 'storeDetail');
      widgetService.registerWidget({
          controller: 'virtoCommerce.storeModule.storeSettingsWidgetController',
          template: 'Modules/$(VirtoCommerce.Store)/Scripts/widgets/storeSettingsWidget.tpl.html'
      }, 'storeDetail');
      widgetService.registerWidget({
          controller: 'virtoCommerce.storeModule.storePaymentsWidgetController',
          template: 'Modules/$(VirtoCommerce.Store)/Scripts/widgets/storePaymentsWidget.tpl.html'
      }, 'storeDetail');
      widgetService.registerWidget({
          controller: 'virtoCommerce.storeModule.storeShippingWidgetController',
          template: 'Modules/$(VirtoCommerce.Store)/Scripts/widgets/storeShippingWidget.tpl.html'
      }, 'storeDetail');

      var resetCommand = {
          name: "Reset",
          icon: 'fa fa-undo',
          executeMethod: function (blade) {
              angular.copy(blade.origEntity, blade.currentEntity);
          },
          canExecuteMethod: function (blade) {
              return !angular.equals(blade.origEntity, blade.currentEntity);
          },
          permission: 'store:manage',
          index: 0
      };
      toolbarService.register(resetCommand, 'virtoCommerce.storeModule.paymentMethodDetailController');
      toolbarService.register(resetCommand, 'virtoCommerce.storeModule.shippingMethodDetailController');

      var settingsCommand = {
          name: "Settings", icon: 'fa fa-wrench',
          executeMethod: function (blade) {
              var newBlade = {
                  id: 'entitySettingList',
                  controller: 'platformWebApp.entitySettingListController',
                  template: 'Scripts/app/settings/blades/entitySetting-list.tpl.html'
              };
              bladeNavigationService.showBlade(newBlade, blade);
          },
          canExecuteMethod: function () {
              return true;
          },
          index: 1
      };
      toolbarService.register(settingsCommand, 'virtoCommerce.storeModule.paymentMethodDetailController');
      toolbarService.register(settingsCommand, 'virtoCommerce.storeModule.shippingMethodDetailController');

      //var settingsWidget = {
      //    controller: 'platformWebApp.entitySettingsWidgetController',
      //    template: 'Scripts/app/settings/widgets/entitySettingsWidget.tpl.html'
      //};
      //widgetService.registerWidget(settingsWidget, 'shippingMethodDetail');
      //widgetService.registerWidget(settingsWidget, 'paymentMethodDetail');
  }])
;