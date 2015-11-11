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
              templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
              controller: [
                  '$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                      var blade = {
                          id: 'pricing',
                          title: 'pricing.blades.pricing-main.title',
                          subtitle: 'pricing.blades.pricing-main.subtitle',
                          controller: 'virtoCommerce.pricingModule.pricingMainController',
                          template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/blades/pricing-main.tpl.html',
                          isClosingDisabled: true
                      };
                      bladeNavigationService.showBlade(blade);
                  	  //Need for isolate and prevent conflict module css to naother modules 
                      $scope.moduleName = "vc-pricing";
                  }
              ]
          });
  }]
)
.run(
  ['$rootScope', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', 'platformWebApp.authService', function ($rootScope, mainMenuService, widgetService, $state, authService) {
      //Register module in main menu
      var menuItem = {
          path: 'browse/pricing',
          icon: 'fa fa-usd',
          title: 'pricing.main-menu-title',
          priority: 30,
          action: function () { $state.go('workspace.pricingModule'); },
          permission: 'pricing:access'
      };
      mainMenuService.addMenuItem(menuItem);

      //Register item prices widget
      var itemPricesWidget = {
          isVisible: function (blade) { return authService.checkPermission('pricing:read'); },
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
