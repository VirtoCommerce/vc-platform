//Call this to register our module to main application
var moduleName = "virtoCommerce.orderModule";

if (AppDependencies != undefined) {
	AppDependencies.push(moduleName);
}

angular.module(moduleName, ['virtoCommerce.catalogModule', 'virtoCommerce.pricingModule'])
.config(
  ['$stateProvider', function ($stateProvider) {
  	$stateProvider
		.state('workspace.orderModule', {
			url: '/orders',
			templateUrl: 'Modules/$(VirtoCommerce.Orders)/Scripts/home.tpl.html',
			controller: [
				'$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
					var blade = {
						id: 'orders',
						title: 'Customer orders',
						//subtitle: 'Manage Orders',
						controller: 'virtoCommerce.orderModule.customerOrderListController',
						template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/customerOrder-list.tpl.html',
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
  		icon: 'fa fa-file-text',
  		title: 'Orders',
  		priority: 99,
  		action: function () { $state.go('workspace.orderModule'); },
  		permission: 'ordersMenuPermission'
  	};
  	mainMenuService.addMenuItem(menuItem);


  	//Register widgets
  	var operationItemsWidget = {
  		controller: 'virtoCommerce.orderModule.operationItemsWidgetController',
  		template: 'Modules/$(VirtoCommerce.Orders)/Scripts/widgets/operation-items-widget.tpl.html',
  	};
  	widgetService.registerWidget(operationItemsWidget, 'customerOrderDetailWidgets');
  	widgetService.registerWidget(operationItemsWidget, 'shipmentDetailWidgets');


  	var customerOrderAddressWidget = {
  	    controller: 'virtoCommerce.orderModule.customerOrderAddressWidgetController',
  		template: 'Modules/$(VirtoCommerce.Orders)/Scripts/widgets/customerOrder-address-widget.tpl.html',
  	};
  	widgetService.registerWidget(customerOrderAddressWidget, 'customerOrderDetailWidgets');
  	
  	var customerOrderTotalsWidget = {
  	    controller: 'virtoCommerce.orderModule.customerOrderTotalsWidgetController',
  		template: 'Modules/$(VirtoCommerce.Orders)/Scripts/widgets/customerOrder-totals-widget.tpl.html',
  	};
  	widgetService.registerWidget(customerOrderTotalsWidget, 'customerOrderDetailWidgets');


  	var operationCommentWidget = {
  		controller: 'virtoCommerce.orderModule.operationCommentWidgetController',
  		template: 'Modules/$(VirtoCommerce.Orders)/Scripts/widgets/operation-comment-widget.tpl.html',
  	};
  	widgetService.registerWidget(operationCommentWidget, 'customerOrderDetailWidgets');


  	var shipmentAddressWidget = {
  		controller: 'virtoCommerce.orderModule.shipmentAddressWidgetController',
  		template: 'Modules/$(VirtoCommerce.Orders)/Scripts/widgets/shipment-address-widget.tpl.html',
  	};
  	widgetService.registerWidget(shipmentAddressWidget, 'shipmentDetailWidgets');


  	var shipmentTotalWidget = {
  		controller: 'virtoCommerce.orderModule.shipmentTotalsWidgetController',
  		template: 'Modules/$(VirtoCommerce.Orders)/Scripts/widgets/shipment-totals-widget.tpl.html',
  	};
  	widgetService.registerWidget(shipmentTotalWidget, 'shipmentDetailWidgets');


  	var operationsTreeWidget = {
  		controller: 'virtoCommerce.orderModule.operationTreeWidgetController',
  		template: 'Modules/$(VirtoCommerce.Orders)/Scripts/widgets/operation-tree-widget.tpl.html',
  	};
  	widgetService.registerWidget(operationsTreeWidget, 'customerOrderDetailWidgets');
  	widgetService.registerWidget(operationsTreeWidget, 'shipmentDetailWidgets');
  	widgetService.registerWidget(operationsTreeWidget, 'paymentDetailWidgets');


  }]);
