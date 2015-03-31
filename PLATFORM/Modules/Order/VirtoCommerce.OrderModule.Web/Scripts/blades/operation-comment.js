angular.module('virtoCommerce.orderModule')
.controller('orderOperationCommentDetail', ['$scope', 'dialogService', function ($scope, dialogService) {
	
	function initializeBlade() {
	
		$scope.blade.origEntity = $scope.blade.currentEntity.comment;
		$scope.blade.isLoading = false;
	};

	function isDirty() {
		return  $scope.blade.currentEntity.comment !== $scope.blade.origEntity;
	};

	$scope.setForm = function (form) {
		$scope.formScope = form;
	}

	$scope.cancelChanges = function () {
		$scope.bladeClose();
	}

	$scope.isValid = function () {
		return $scope.formScope && $scope.formScope.$valid;
	}

	$scope.saveChanges = function () {
		$scope.blade.origEntity = $scope.blade.currentEntity.comment;
		$scope.bladeClose();
	};

	$scope.blade.onClose = function (closeCallback) {
		closeCallback();
	};

	$scope.bladeHeadIco = 'fa-file-text';

	initializeBlade();
}]);