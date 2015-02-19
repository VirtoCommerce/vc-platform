angular.module('virtoCommerce.orderModule.widgets')
.controller('operationTreeWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
	$scope.currentBlade = $scope.widget.blade;
	$scope.currentOperation = {};
	$scope.$watch('widget.blade.currentEntity', function (operation) {
		$scope.currentOperation = operation;
	});

	$scope.selectOperation = function (operation) {
		if ($scope.currentOperation.id != operation.id) {
			$scope.currentOperation = operation;
		}
	};

}]);
