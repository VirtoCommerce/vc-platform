﻿angular.module('virtoCommerce.orderModule')
.controller('virtoCommerce.orderModule.operationCommentWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
	$scope.operation = {};

	$scope.openCommentBlade = function () {
		var newBlade = {
			id: 'operationComment',
			title: 'orders.widgets.operation-comment.blade-title',
			currentEntity: $scope.operation,
			controller: 'virtoCommerce.orderModule.orderOperationCommentDetail',
			template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/operation-comment.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	};

	$scope.$watch('widget.blade.currentEntity', function (operation) {
		$scope.operation = operation;
	});
}]);
