angular.module('virtoCommerce.coreModule.common')
.controller('virtoCommerce.coreModule.common.coreAddressListController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
	$scope.selectedItem = null;

	$scope.openDetailBlade = function (address) {
		if (!address) {
			address = { isNew: true };
		}
		$scope.selectedItem = address;

		var newBlade = {
			id: 'coreAddressDetail',
			currentEntity: address,
			title: $scope.blade.title,
			subtitle: 'Edit address',
			controller: 'virtoCommerce.coreModule.common.coreAddressDetailController',
			confirmChangesFn: function(address)
			{
				if(address.isNew)
				{
					address.name = $scope.getAddressName(address);
					address.isNew = undefined;
					$scope.blade.currentEntities.push(address);
					if($scope.blade.confirmChangesFn){
						$scope.blade.confirmChangesFn(address);
					}
				}
			},
			deleteFn: function(address)
			{
				var toRemove = _.find($scope.blade.currentEntities, function(x) { return angular.equals(x, address) } );
				if (toRemove) {
					var idx = $scope.blade.currentEntities.indexOf(toRemove);
					$scope.blade.currentEntities.splice(idx, 1);
					if ($scope.blade.deleteFn) {
						$scope.blade.deleteFn(address);
					}
				}
			},

			template: 'Modules/$(VirtoCommerce.Core)/Scripts/common/blades/address-detail.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	}

	$scope.getAddressName = function (address) {
		var retVal = address.name;
		if (!retVal)
		{
			retVal = [address.countryCode, address.regionName, address.city, address.line1].join(",");
		}
		return retVal;
	};

	$scope.blade.onClose = function (closeCallback) {
		closeChildrenBlades();
		closeCallback();
	};

	function closeChildrenBlades() {
		angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
			bladeNavigationService.closeBlade(child);
		});
	}
	
	$scope.bladeHeadIco = 'fa fa-user';

	$scope.bladeToolbarCommands = [
        {
        	name: "Add", icon: 'fa fa-plus',
        	executeMethod: function () {
        		$scope.openDetailBlade();
        	},
        	canExecuteMethod: function () {
        		return true;
        	}
        }
	];

	$scope.blade.isLoading = false;
	
	// open blade for new setting
	if (!_.some($scope.blade.currentEntities)) {
		$scope.openDetailBlade();
	}
}]);
