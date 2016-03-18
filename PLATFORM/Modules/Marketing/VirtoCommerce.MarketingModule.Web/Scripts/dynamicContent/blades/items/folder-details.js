angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.addFolderContentItemsController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.folders', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, marketing_dynamicContents_res_folders, bladeNavigationService, dialogService) {
    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    var blade = $scope.blade;
    blade.updatePermission = 'marketing:update';
    blade.originalEntity = angular.copy(blade.entity);

    blade.initialize = function () {
        if (!blade.isNew) {
            $scope.blade.toolbarCommands = [
				{
				    name: "platform.commands.save", icon: 'fa fa-save',
				    executeMethod: function () {
				        blade.saveChanges();
				    },
				    canExecuteMethod: function () {
				        return !angular.equals(blade.originalEntity, blade.entity) && !$scope.formScope.$invalid;
				    },
				    permission: blade.updatePermission
				},
				{
				    name: "platform.commands.reset", icon: 'fa fa-undo',
				    executeMethod: function () {
				        blade.entity = angular.copy(blade.originalEntity);
				    },
				    canExecuteMethod: function () {
				        return !angular.equals(blade.originalEntity, blade.entity);
				    },
				    permission: blade.updatePermission
				},
                {
                    name: "platform.commands.delete", icon: 'fa fa-trash',
                	executeMethod: function () {
                		var dialog = {
                			id: "confirmDeleteContentItem",
                			title: "marketing.dialogs.content-folder-delete.title",
                			message: "marketing.dialogs.content-folder-delete.message",
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
                    permission: blade.updatePermission
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
    		blade.parentBlade.chosenFolder = id;
    		blade.parentBlade.initializeBlade();
    	},
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); blade.isLoading = false; });
    }

    $scope.blade.headIcon = 'fa-inbox';

    blade.initialize();
}]);