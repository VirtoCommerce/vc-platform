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
                          controller: 'pricingMainController',
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
  ['$rootScope', 'mainMenuService', 'widgetService', '$state', function ($rootScope, mainMenuService, widgetService, $state) {
      //Register module in main menu
      var menuItem = {
          path: 'browse/pricing',
          icon: 'fa fa-usd',
          title: 'Pricing',
          priority: 30,
          action: function () { $state.go('workspace.pricingModule'); },
          permission: 'pricingMenuPermission'
      };
      mainMenuService.addMenuItem(menuItem);

      //Register item prices widget
      var itemPricesWidget = {
          controller: 'itemPricesWidgetController',
          template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/widgets/itemPricesWidget.tpl.html',
      };
      widgetService.registerWidget(itemPricesWidget, 'itemDetail');

      //Register pricelist widgets
      widgetService.registerWidget({
          controller: 'pricesWidgetController',
          template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/widgets/pricesWidget.tpl.html',
      }, 'pricelistDetail');
      widgetService.registerWidget({
          controller: 'assignmentsWidgetController',
          template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/widgets/assignmentsWidget.tpl.html',
      }, 'pricelistDetail');
  }]);
