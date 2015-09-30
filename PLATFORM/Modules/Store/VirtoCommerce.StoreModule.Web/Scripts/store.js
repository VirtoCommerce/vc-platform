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
              templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
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
  ['platformWebApp.toolbarService', 'platformWebApp.bladeNavigationService', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', 'platformWebApp.securityRoleScopeService', 'virtoCommerce.storeModule.stores', function (toolbarService, bladeNavigationService, mainMenuService, widgetService, $state, securityRoleScopeService, stores) {
      //Register module in main menu
      var menuItem = {
          path: 'browse/store',
          icon: 'fa fa-archive',
          title: 'Stores',
          priority: 110,
          action: function () { $state.go('workspace.storeModule'); },
          permission: 'store:access'
      };
      mainMenuService.addMenuItem(menuItem);

  	 //Register security scope types used for scope bounded ACL definition
      var getScopesFn = function () {
      	return stores.query({}).$promise.then(function (result) {
      		var scopes = _.map(result, function (x) { return "store:" + x.name; });
      		return scopes;
      	});
      };
      securityRoleScopeService.registerScopeGetter(getScopesFn);

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
          name: "Reset",
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

   
  }])
;