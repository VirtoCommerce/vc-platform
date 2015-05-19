angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.exporterListController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.catalogModule.catalogExportService', function ($scope, bladeNavigationService, catalogExportService) {
    var blade = $scope.blade;

	$scope.selectedNodeId = null;

	function initializeBlade() {
	    $scope.registrationsList = catalogExportService.registrationsList;
		blade.isLoading = false;
	};

	$scope.openBlade = function (data) {
	    data.catalogId = blade.catalogId;
		bladeNavigationService.showBlade(data, blade.parentBlade);
	}

	$scope.bladeHeadIco = 'fa fa-upload';

	initializeBlade();
}]);
