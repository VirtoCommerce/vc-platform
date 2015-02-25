//Call this to register our module to main application
var moduleName = "virtoCommerce.orderModule";

if (AppDependencies != undefined) {
	AppDependencies.push(moduleName);
}

angular.module(moduleName, [
    'virtoCommerce.orderModule.blades',
	'virtoCommerce.orderModule.widgets',
	'virtoCommerce.orderModule.services'
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
						template: 'Modules/Order/VirtoCommerce.OrderModule.Web/Scripts/blades/customerOrder-list.tpl.html',
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


  	//Register widgets
  	var customerOrderItemsWidget = {
   		controller: 'customerOrderItemsWidgetController',
  		template: 'Modules/Order/VirtoCommerce.OrderModule.Web/Scripts/widgets/customerOrder-items-widget.tpl.html',
  	};
  	widgetService.registerWidget(customerOrderItemsWidget, 'customerOrderDetailWidgets');

  	var operationCommentWidget = {
  		controller: 'operationCommentWidgetController',
  		template: 'Modules/Order/VirtoCommerce.OrderModule.Web/Scripts/widgets/operation-comment-widget.tpl.html',
  	};
  	widgetService.registerWidget(operationCommentWidget, 'customerOrderDetailWidgets');

  	var customerOrderTotalsWidget = {
  		controller: 'customerOrderTotalsWidgetController',
  		template: 'Modules/Order/VirtoCommerce.OrderModule.Web/Scripts/widgets/customerOrder-totals-widget.tpl.html',
  	};
  	widgetService.registerWidget(customerOrderTotalsWidget, 'customerOrderDetailWidgets');

  	var shipmentTotalWidget = {
  		controller: 'shipmentTotalsWidgetController',
  		template: 'Modules/Order/VirtoCommerce.OrderModule.Web/Scripts/widgets/shipment-totals-widget.tpl.html',
  	};
  	widgetService.registerWidget(shipmentTotalWidget, 'shipmentDetailWidgets');


  	var operationsTreeWidget = {
   		controller: 'operationTreeWidgetController',
  		template: 'Modules/Order/VirtoCommerce.OrderModule.Web/Scripts/widgets/operation-tree-widget.tpl.html',
  	};
  	widgetService.registerWidget(operationsTreeWidget, 'customerOrderDetailWidgets');
  	widgetService.registerWidget(operationsTreeWidget, 'shipmentDetailWidgets');
  	widgetService.registerWidget(operationsTreeWidget, 'paymentDetailWidgets');

  	
  }]);
