﻿//Call this to register our module to main application
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
              templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
              controller: [
                  '$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                      var blade = {
                          id: 'store',
                          title: 'stores.blades.stores-list.title',
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
  ['platformWebApp.toolbarService', 'platformWebApp.bladeNavigationService', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', 'platformWebApp.permissionScopeResolver', 'virtoCommerce.storeModule.stores',
	function (toolbarService, bladeNavigationService, mainMenuService, widgetService, $state, scopeResolver, stores) {
      //Register module in main menu
      var menuItem = {
          path: 'browse/store',
          icon: 'fa fa-archive',
          title: 'stores.main-menu-title',
          priority: 110,
          action: function () { $state.go('workspace.storeModule'); },
          permission: 'store:access'
      };
      mainMenuService.addMenuItem(menuItem);

  	  //Register widgets in store details
      widgetService.registerWidget({
          controller: 'virtoCommerce.storeModule.seoWidgetController',
          template: 'Modules/$(VirtoCommerce.Store)/Scripts/widgets/seoWidget.tpl.html'
      }, 'storeDetail');
      widgetService.registerWidget({
          controller: 'virtoCommerce.storeModule.storeAdvancedWidgetController',
          template: 'Modules/$(VirtoCommerce.Store)/Scripts/widgets/storeAdvancedWidget.tpl.html'
      }, 'storeDetail');
      widgetService.registerWidget({
      	controller: 'platformWebApp.dynamicPropertyWidgetController',
		title : 'Settings',
      	template: '$(Platform)/Scripts/app/dynamicProperties/widgets/dynamicPropertyWidget.tpl.html'
      }, 'storeDetail');
      widgetService.registerWidget({
          controller: 'virtoCommerce.storeModule.storePaymentsWidgetController',
          template: 'Modules/$(VirtoCommerce.Store)/Scripts/widgets/storePaymentsWidget.tpl.html'
      }, 'storeDetail');
      widgetService.registerWidget({
          controller: 'virtoCommerce.storeModule.storeShippingWidgetController',
          template: 'Modules/$(VirtoCommerce.Store)/Scripts/widgets/storeShippingWidget.tpl.html'
      }, 'storeDetail');
      widgetService.registerWidget({
          controller: 'virtoCommerce.storeModule.storeTaxingWidgetController',
          template: 'Modules/$(VirtoCommerce.Store)/Scripts/widgets/storeTaxingWidget.tpl.html'
      }, 'storeDetail');
      widgetService.registerWidget({
          controller: 'virtoCommerce.storeModule.storeNotificationsWidgetController',
          template: 'Modules/$(VirtoCommerce.Store)/Scripts/widgets/storeNotificationsWidget.tpl.html'
      }, 'storeDetail');
      widgetService.registerWidget({
          controller: 'virtoCommerce.storeModule.storeNotificationsLogWidgetController',
          template: 'Modules/$(VirtoCommerce.Store)/Scripts/widgets/storeNotificationsLogWidget.tpl.html'
      }, 'storeDetail');

      var resetCommand = {
          name: "platform.commands.reset",
          icon: 'fa fa-undo',
          executeMethod: function (blade) {
              angular.copy(blade.origEntity, blade.currentEntity);
          },
          canExecuteMethod: function (blade) {
              return !angular.equals(blade.origEntity, blade.currentEntity);
          },
          permission: 'store:update',
          index: 0
      };
      toolbarService.register(resetCommand, 'virtoCommerce.storeModule.paymentMethodDetailController');
      toolbarService.register(resetCommand, 'virtoCommerce.storeModule.shippingMethodDetailController');
      toolbarService.register(resetCommand, 'virtoCommerce.storeModule.taxProviderDetailController');

  	//Register permission scopes templates used for scope bounded definition in role management ui
      var selectedStoreScope = {
      	type: 'StoreSelectedScope',
      	title: 'Only for selected stores',
      	selectFn: function (blade, callback) {
      		var newBlade = {
      			id: 'store-pick',
      			title: this.title,
      			subtitle: 'Select stores',
      			currentEntity: this,
      			onChangesConfirmedFn: callback,
      			dataPromise: stores.query().$promise,
      			controller: 'platformWebApp.security.scopeValuePickFromSimpleListController',
      			template: '$(Platform)/Scripts/app/security/blades/common/scope-value-pick-from-simple-list.tpl.html'
      		};
      		bladeNavigationService.showBlade(newBlade, blade);
      	}
      };
      scopeResolver.register(selectedStoreScope);
  }])
;