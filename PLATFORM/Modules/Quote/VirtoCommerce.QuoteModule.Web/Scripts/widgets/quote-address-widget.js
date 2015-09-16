angular.module('virtoCommerce.quoteModule')
.controller('virtoCommerce.quoteModule.quoteAddressWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
	$scope.currentBlade = $scope.widget.blade;
	$scope.operation = {};
	$scope.openBlade = function () {
	
		var deliveryAddress = $scope.operation.deliveryAddress;
		if (!deliveryAddress) {
			deliveryAddress = { isNew: true };
		};
		var newBlade = {
		    id: 'quoteAddresses',
			title: 'Manage delivery address',
			currentEntity: deliveryAddress,
			controller: 'virtoCommerce.coreModule.common.coreAddressDetailController',
			template: 'Modules/$(VirtoCommerce.Core)/Scripts/common/blades/address-detail.tpl.html',
			deleteFn : function(address)
			{
				$scope.operation.deliveryAddress = null
			},
			confirmChangesFn : function(address)
			{
				$scope.operation.deliveryAddress = address;
				address.isNew = false;
			}
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	};

	$scope.getAddressName = function (address) {

		if (address) {
			retVal = [address.countryCode, address.regionName, address.city, address.line1].join(",");
		}
		return null;
	};

	$scope.$watch('widget.blade.currentEntity', function (operation) {
		$scope.operation = operation;
	});
}]);
