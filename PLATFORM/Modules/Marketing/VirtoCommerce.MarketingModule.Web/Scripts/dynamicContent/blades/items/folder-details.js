angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.addFolderContentItemsController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.folders', 'platformWebApp.bladeNavigationService', function ($scope, marketing_dynamicContents_res_folders, bladeNavigationService) {
    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    var blade = $scope.blade;
    blade.originalEntity = angular.copy(blade.entity);

    blade.initialize = function () {
        if (!blade.isNew) {
            $scope.blade.toolbarCommands = [
				{
				    name: "Save", icon: 'fa fa-save',
				    executeMethod: function () {
				        blade.saveChanges();
				    },
				    canExecuteMethod: function () {
				        return !angular.equals(blade.originalEntity, blade.entity) && !$scope.formScope.$invalid;
				    },
				    permission: 'marketing:manage'
				},
				{
				    name: "Reset", icon: 'fa fa-undo',
				    executeMethod: function () {
				        blade.entity = angular.copy(blade.originalEntity);
				    },
				    canExecuteMethod: function () {
				        return !angular.equals(blade.originalEntity, blade.entity);
				    },
				    permission: 'marketing:manage'
				},
                {
                	name: "Delete", icon: 'fa fa-trash',
                	executeMethod: function () {
                		var dialog = {
                			id: "confirmDeleteContentItem",
                			title: "Delete confirmation",
                			message: "Are you sure want to delete content item?",
                			callback: function (remove) {
                				if (remove) {
                					blade.deleteFolder(blade.entity);
                					bladeNavigationService.closeBlade(blade);
                				}
                			}
                		};

                		dialogService.showConfirmationDialog(dialog);
                	},
                    canExecuteMethod: function () {
                        return true;
                    },
                    permission: 'marketing:manage'
                }
            ];
        }

        blade.isLoading = false;
    }

    blade.saveChanges = function () {
        blade.isLoading = true;

        if (blade.isNew) {
            marketing_dynamicContents_res_folders.save({}, blade.entity, function (data) {
            	blade.parentBlade.initializeBlade();
                bladeNavigationService.closeBlade(blade);
                blade.isLoading = false;
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); blade.isLoading = false; });
        }
        else {
            marketing_dynamicContents_res_folders.update({}, blade.entity, function (data) {
            	blade.parentBlade.initializeBlade();
                blade.originalEntity = angular.copy(blade.entity);
                blade.isLoading = false;
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); blade.isLoading = false; });
        }
    }

    blade.deleteFolder = function (data) {
    	marketing_dynamicContents_res_folders.delete({ ids: [data.id] }, function () {
    		var pathSteps = data.outline.split(';');
    		var id = pathSteps[pathSteps.length - 2];
    		blade.parentBlade.choosenFolder = id;
    		blade.parentBlade.initializeBlade();
    	},
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); blade.isLoading = false; });
    }

    $scope.blade.headIcon = 'fa-inbox';

    blade.initialize();
}]);