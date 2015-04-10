angular.module('virtoCommerce.marketingModule')
.controller('addFolderContentItemsController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
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
						return !angular.equals(blade.originalEntity, blade.entity);
					}
				}
			];
		}

		blade.isLoading = false;
	}

	blade.saveChanges = function () {
		if (blade.isNew) {
			if (angular.isUndefined(blade.parentBlade.currentEntity)) {
				blade.parentBlade.currentEntities.push(blade.entity);
				blade.originalEntity = angular.copy(blade.entity);
			}
			else {
				blade.parentBlade.currentEntity.childrenFolders.push(blade.entity);
			}
			bladeNavigationService.closeBlade(blade);
		}
		else {
			blade.originalEntity = angular.copy(blade.entity);
		}
	}

	blade.initialize();
}]);