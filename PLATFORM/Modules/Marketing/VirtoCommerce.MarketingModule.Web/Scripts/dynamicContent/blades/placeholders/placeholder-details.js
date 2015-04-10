angular.module('virtoCommerce.marketingModule')
.controller('addPlaceholderController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
	$scope.setForm = function (form) {
		$scope.formScope = form;
	}

	var blade = $scope.blade;
	blade.originalEntity = angular.copy(blade.entity);

	blade.initialize = function () {
		if (!blade.isNew) {
			$scope.bladeToolbarCommands = [
				{
					name: "Refresh", icon: 'fa fa-refresh',
					executeMethod: function () {
						blade.entity = angular.copy(blade.originalEntity);
					},
					canExecuteMethod: function () {
						return !angular.equals(blade.originalEntity, blade.entity);
					}
				},
				{
					name: "Save", icon: 'fa fa-save',
					executeMethod: function () {
						blade.saveChanges();
					},
					canExecuteMethod: function () {
						return !angular.equals(blade.originalEntity, blade.entity) && !$scope.formScope.$invalid;
					}
				},
				{
					name: "Delete", icon: 'fa fa-trash',
					executeMethod: function () {
						blade.delete();
					},
					canExecuteMethod: function () {
						return true;
					}
				}
			];
		}

		blade.isLoading = false;
	}

	blade.saveChanges = function () {
		blade.parentBlade.currentEntity.placeholders.push(blade.entity);
		blade.originalEntity = angular.copy(blade.entity);
		if (blade.isNew) {
			bladeNavigationService.closeBlade(blade);
		}
	}

	blade.initialize();
}]);