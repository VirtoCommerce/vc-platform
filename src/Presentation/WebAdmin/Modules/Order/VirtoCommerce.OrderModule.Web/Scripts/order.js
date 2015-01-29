//Call this to register our module to main application
var moduleName = "virtoCommerce.orderModule";

if (AppDependencies != undefined) {
	AppDependencies.push(moduleName);
}

angular.module(moduleName, [
    'virtoCommerce.orderModule.blades'
])
.config(
  ['$stateProvider', function ($stateProvider) {
  	$stateProvider
		.state('workspace.orderModule', {
			url: '/orders',
			templateUrl: 'Modules/Order/VirtoCommerce.OrderModule.Web/Scripts/home.tpl.html',
			controller: [
				'$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
					var blade = {
						id: 'orders',
						title: 'Customer orders',
						//subtitle: 'Manage Orders',
						controller: 'customerOrderListController',
						template: 'Modules/Order/VirtoCommerce.OrderModule.Web/Scripts/blades/!customerOrder-list.tpl.html',
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
  		path: 'browse/orders',
  		icon: 'fa fa-shopping-cart',
  		title: 'Orders',
  		priority: 99,
  		action: function () { $state.go('workspace.orderModule'); },
  		permission: 'ordersMenuPermission'
  	};
  	mainMenuService.addMenuItem(menuItem);
  }])
;
