angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.themesWidgetController', ['$injector', '$state', '$rootScope', '$scope', 'virtoCommerce.contentModule.themes', 'virtoCommerce.contentModule.stores', 'platformWebApp.bladeNavigationService', function ($injector, $state, $rootScope, $scope, themes, stores, bladeNavigationService) {
	var blade = $scope.widget.blade;

	$scope.widget.initialize = function () {
		$scope.themesCount = '...';
		return themes.query({ storeId: blade.currentEntityId }, function (data) {
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
