angular.module('virtoCommerce.orderModule.blades')
.controller('shpmentDetailController', ['$scope', 'dialogService', 'bladeNavigationService', 'customerOrders', function ($scope, dialogService, bladeNavigationService, customerOrders) {
	$scope.blade.shipment = {};

	$scope.blade.refresh = function () {
		$scope.blade.isLoading = true;

		customerOrders.get({ id: $scope.blade.customerOrder.id }, function (results) {
			$scope.blade.customerOrder = angular.copy(results);
			
			$scope.blade.operation = _.find($scope.blade.customerOrder.shipments, function (x) { return x.id == $scope.blade.currentEntityId; });
			$scope.blade.origEntity = _.find(results.shipments, function (x) { return x.id == $scope.blade.currentEntityId; });


			$scope.blade.isLoading = false;

		},
        function (error) {
        	$scope.blade.isLoading = false;
        	bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
	}

	function isDirty() {
		return !angular.equals($scope.blade.customerOrder, $scope.blade.origEntity);
	};

	function saveChanges() {
		$scope.blade.isLoading = true;
		customerOrders.update({}, $scope.blade.customerOrder, function (data, headers) {
			$scope.blade.refresh();
		});
	};

	$scope.bladeToolbarCommands = [
        {
        	name: "New document", icon: 'fa fa-plus',
        	executeMethod: function () {
        		openAddEntityWizard();
        	},
        	canExecuteMethod: function () {
        		return true;
        	}
        },
        {
        	name: "Save", icon: 'fa fa-save',
        	executeMethod: function () {
        		saveChanges();
        	},
        	canExecuteMethod: function () {
        		return isDirty();
        	}
        },
        {
        	name: "Reset", icon: 'fa fa-undo',
        	executeMethod: function () {
        		angular.copy($scope.blade.origEntity, $scope.blade.customerOrder);
        	},
        	canExecuteMethod: function () {
        		return isDirty();
        	}
        }
	];

	$scope.blade.onClose = function (closeCallback) {
		if (isDirty()) {
			var dialog = {
				id: "confirmItemChange",
				title: "Save changes",
				message: "The order has been modified. Do you want to save changes?",
				callback: function (needSave) {
					if (needSave) {
						saveChanges();
					}
					closeChildrenBlades();
					closeCallback();
				}
			};
			dialogService.showConfirmationDialog(dialog);
		}
		else {
			closeChildrenBlades();
			closeCallback();
		}
	};

	function closeChildrenBlades() {
		angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
			bladeNavigationService.closeBlade(child);
		});
	}

	// actions on load
	$scope.toolbarTemplate = 'Modules/Order/VirtoCommerce.OrderModule.Web/Scripts/blades/customerOrder-detail-toolbar.tpl.html';
	$scope.blade.refresh();

}]);