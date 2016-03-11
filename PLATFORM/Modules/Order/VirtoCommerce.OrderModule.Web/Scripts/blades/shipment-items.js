angular.module('virtoCommerce.orderModule')
.controller('virtoCommerce.orderModule.shipmentItemsController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, bladeNavigationService, dialogService) {
    var blade = $scope.blade;
    blade.updatePermission = 'order:update';

	//pagination settings
	$scope.pageSettings = {};
	$scope.totals = {};
	$scope.pageSettings.totalItems = blade.currentEntity.items.length;
	$scope.pageSettings.currentPage = 1;
	$scope.pageSettings.numPages = 5;
	$scope.pageSettings.itemsPerPageCount = 4;

	var selectedNode = null;
	var selectedProducts = [];

	
	blade.refresh = function () {
		blade.isLoading = false;
		blade.selectedAll = false;
	};
        
	blade.toolbarCommands = [
        {
            name: "orders.commands.add-item", icon: 'fa fa-plus',
        	executeMethod: function () {
        		openAddEntityWizard();
        	},
        	canExecuteMethod: function () {
        		return false;
        	},
        	permission: blade.updatePermission
        },
        {
            name: "platform.commands.remove", icon: 'fa fa-trash-o',
        	executeMethod: function () {
        		var items = blade.currentEntity.items;
        		blade.currentEntity.items = _.difference(items, _.filter(items, function (x) { return x.selected }));

        	},
        	canExecuteMethod: function () {
        		return _.any(blade.currentEntity.items, function (x) { return x.selected; });;
        	},
        	permission: blade.updatePermission
        }
	];

	//$scope.$watch('pageSettings.currentPage', function (newPage) {
	//    blade.refresh();
	//});

	$scope.selectItem = function (node) {
		selectedNode = node;
		$scope.selectedNodeId = selectedNode.id;
	};

	$scope.checkAll = function (selected) {
		angular.forEach(blade.currentEntity.items, function (item) {
			item.selected = selected;
		});
	};

	blade.refresh();
}]);