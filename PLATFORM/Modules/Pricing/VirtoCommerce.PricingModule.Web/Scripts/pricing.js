//Call this to register our module to main application
var moduleName = "virtoCommerce.pricingModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
.config(
  ['$stateProvider', function ($stateProvider) {
      $stateProvider
          .state('workspace.pricingModule', {
              url: '/pricing',
              templateUrl: 'Modules/$(VirtoCommerce.Pricing)/Scripts/home.tpl.html',
              controller: [
                  '$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
                      var blade = {
                          id: 'pricing',
                          title: 'Pricing',
                          subtitle: 'Merchandise management',
                          controller: 'virtoCommerce.pricingModule.pricingMainController',
                          template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/blades/pricing-main.tpl.html',
                          isClosingDisabled: true
                      };
                      bladeNavigationService.showBlade(blade);
                  }
              ]
          });
  }]
)
.run(
  ['$rootScope', 'mainMenuService', 'widgetService', '$state', 'authService', function ($rootScope, mainMenuService, widgetService, $state, authService) {
      //Register module in main menu
      var menuItem = {
          path: 'browse/pricing',
          icon: 'fa fa-usd',
          title: 'Pricing',
          priority: 30,
          action: function () { $state.go('workspace.pricingModule'); },
          permission: 'pricing:query'
      };
      mainMenuService.addMenuItem(menuItem);

      //Register item prices widget
      var itemPricesWidget = {
          isVisible: function (blade) { return authService.checkPermission('pricing:query'); },
          controller: 'virtoCommerce.pricingModule.itemPricesWidgetController',
          size: [2, 1],
          template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/widgets/itemPricesWidget.tpl.html',
      };
      widgetService.registerWidget(itemPricesWidget, 'itemDetail');

      //Register pricelist widgets
      widgetService.registerWidget({
          controller: 'virtoCommerce.pricingModule.pricesWidgetController',
          template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/widgets/pricesWidget.tpl.html',
      }, 'pricelistDetail');
      widgetService.registerWidget({
          controller: 'virtoCommerce.pricingModule.assignmentsWidgetController',
          template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/widgets/assignmentsWidget.tpl.html',
      }, 'pricelistDetail');
  }]);
