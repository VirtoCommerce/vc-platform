angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.storeCMSWidgetController', ['$state', '$scope', 'virtoCommerce.contentModule.contentApi', 'platformWebApp.bladeNavigationService', function ($state, $scope, contentApi, bladeNavigationService) {
	var blade = $scope.widget.blade;

	$scope.widget.initialize = function () {
		$scope.themesCount = '...';
		return contentApi.query({ contentType: 'themes', storeId: blade.currentEntityId }, function (data) {
			$scope.themesCount = data.length;
		}, function (error) {
		    //bladeNavigationService.setError('Error ' + error.status, $scope.blade);
		});
	}

	$scope.openBlade = function () {
		$state.go('workspace.content', { storeId: blade.currentEntity.id});
	};

	$scope.widget.initialize();
}]);
