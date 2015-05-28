angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.exporterListController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.catalogModule.catalogExportService', function ($scope, bladeNavigationService, catalogExportService) {
    var blade = $scope.blade;

	$scope.selectedNodeId = null;

	function initializeBlade() {
	    $scope.registrationsList = catalogExportService.registrationsList;
		blade.isLoading = false;
	};

	$scope.openBlade = function (data) {
		var newBlade = {};
		angular.copy(data, newBlade);
		newBlade.catalog = blade.catalog;
		newBlade.title = 'Catalog ' + blade.catalog.name + ' to csv export';

		bladeNavigationService.showBlade(newBlade, blade.parentBlade);
	}

	$scope.blade.headIcon = 'fa fa-upload';

	initializeBlade();
}]);
