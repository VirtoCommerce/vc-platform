angular.module('virtoCommerce.orderModule')
.controller('operationCommentWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
	$scope.currentBlade = $scope.widget.blade;
	$scope.operation = {};

	$scope.openCommentBlade = function () {
		var newBlade = {
			id: 'operationComment',
			title: 'Wtite comments',
			currentEntity: $scope.operation,
			controller: 'orderOperationCommentDetail',
			template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/operation-comment.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	};

	$scope.$watch('widget.blade.currentEntity', function (operation) {
		$scope.operation = operation;
	});
}]);
