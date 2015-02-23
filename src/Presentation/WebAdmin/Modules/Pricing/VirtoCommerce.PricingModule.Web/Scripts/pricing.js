//Call this to register our module to main application
var moduleName = "virtoCommerce.pricingModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [
    'virtoCommerce.pricingModule.widget.itemPricesWidget'
])
//.config(
//  ['$stateProvider', function ($stateProvider) {
//  	$stateProvider
//		.state('workspace.pricingModule', {
//			url: '/pricing',
//			templateUrl: 'Modules/Pricing/VirtoCommerce.PricingModule.Web/Scripts/home.tpl.html',
//			controller: [
//				'$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
//					var blade = {
//						id: 'pricing',
//						title: 'Pricing',
//						//subtitle: 'Manage prices',
//						controller: 'pricesListController',
//						template: 'Modules/Pricing/VirtoCommerce.PricingModule.Web/Scripts/blades/!pricing-list.tpl.html',
//						isClosingDisabled: true
//					};
//					bladeNavigationService.showBlade(blade);
//				}
//			]
//		});
//  }]
//)
.run(
  ['$rootScope', 'mainMenuService', 'widgetService', '$state', function ($rootScope, mainMenuService, widgetService, $state) {
      //Register module in main menu
      //var menuItem = {
      //	path: 'browse/pricing',
      //	icon: 'fa fa-shopping-cart',
      //	title: 'Pricing',
      //	priority: 99,
      //	action: function () { $state.go('workspace.pricingModule'); },
      //	permission: 'pricingsMenuPermission'
      //};
      //mainMenuService.addMenuItem(menuItem);

      //Register item prices widget
      var itemPricesWidget = {
          group: 'itemDetail',
          controller: 'itemPricesWidgetController',
          template: 'Modules/Pricing/VirtoCommerce.PricingModule.Web/Scripts/widgets/itemPricesWidget.tpl.html',
      };
      widgetService.registerWidget(itemPricesWidget);
  }])
;
