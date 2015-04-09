angular.module('virtoCommerce.marketingModule')
.controller('addFolderPlaceholderController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
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
        				return !angular.equals(blade.originalEntity, blade.entity);
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
		else
		{
			$scope.bladeToolbarCommands = [
				{
					name: "Save", icon: 'fa fa-save',
					executeMethod: function () {
						blade.saveChanges();
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
		blade.parentBlade.currentEntities.push(blade.entity);
		blade.originalEntity = angular.copy(blade.entity);
		blade.isNew = false;
		blade.initialize();
	}

	blade.initialize();
}]);