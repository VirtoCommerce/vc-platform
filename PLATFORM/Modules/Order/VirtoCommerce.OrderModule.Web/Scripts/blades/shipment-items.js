angular.module('virtoCommerce.orderModule')
.controller('virtoCommerce.orderModule.shipmentItemsController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, bladeNavigationService, dialogService) {
	//pagination settings
	$scope.pageSettings = {};
	$scope.totals = {};
	$scope.pageSettings.totalItems = $scope.blade.currentEntity.items.length;
	$scope.pageSettings.currentPage = 1;
	$scope.pageSettings.numPages = 5;
	$scope.pageSettings.itemsPerPageCount = 4;

	var selectedNode = null;
	var selectedProducts = [];

	
	$scope.blade.refresh = function () {
		$scope.blade.isLoading = false;
		$scope.blade.selectedAll = false;
	};

	$scope.blade.onClose = function (closeCallback) {
		closeChildrenBlades();
		closeCallback();
	};

	function closeChildrenBlades() {
		angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
			bladeNavigationService.closeBlade(child);
		});
	}

	$scope.blade.toolbarCommands = [
        {
            name: "orders.commands.add-item", icon: 'fa fa-plus',
        	executeMethod: function () {
        		openAddEntityWizard();
        	},
        	canExecuteMethod: function () {
        		return false;
        	},
        	permission: 'order:update'
        },
        {
            name: "platform.commands.remove", icon: 'fa fa-trash-o',
        	executeMethod: function () {
        		var items = $scope.blade.currentEntity.items;
        		$scope.blade.currentEntity.items = _.difference(items, _.filter(items, function (x) { return x.selected }));

        	},
        	canExecuteMethod: function () {
        		return _.any($scope.blade.currentEntity.items, function (x) { return x.selected; });;
        	},
        	permission: 'order:update'
        }
	];

	//$scope.$watch('pageSettings.currentPage', function (newPage) {
	//    $scope.blade.refresh();
	//});

	$scope.selectItem = function (node) {
		selectedNode = node;
		$scope.selectedNodeId = selectedNode.id;
	};

	$scope.checkAll = function (selected) {
		angular.forEach($scope.blade.currentEntity.items, function (item) {
			item.selected = selected;
		});
	};

	$scope.blade.refresh();
}]);