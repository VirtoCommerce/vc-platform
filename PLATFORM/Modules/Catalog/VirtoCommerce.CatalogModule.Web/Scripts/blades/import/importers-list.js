angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.importerListController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.catalogModule.catalogImportService', function ($scope, bladeNavigationService, catalogImportService) {
    var blade = $scope.blade;

	$scope.selectedNodeId = null;

	function initializeBlade() {
		$scope.registrationsList = catalogImportService.registrationsList;
		blade.isLoading = false;
	};

	$scope.openBlade = function (data) {
		var newBlade = {};
		angular.copy(data, newBlade);
		newBlade.catalog = blade.catalog;

		bladeNavigationService.showBlade(newBlade, blade.parentBlade);
	}

	$scope.bladeHeadIco = 'fa fa-upload';

	initializeBlade();
}]);
