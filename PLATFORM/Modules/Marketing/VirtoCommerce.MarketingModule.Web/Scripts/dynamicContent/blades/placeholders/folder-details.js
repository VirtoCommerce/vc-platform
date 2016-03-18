angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.addFolderPlaceholderController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.folders', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, marketing_dynamicContents_res_folders, bladeNavigationService, dialogService) {
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
				    		id: "confirmDeleteContentPlaceholdersFolder",
				    		title: "marketing.dialogs.placeholders-folder-delete.title",
				    		message: "marketing.dialogs.placeholders-folder-delete.message",
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
        if (blade.isNew) {
            marketing_dynamicContents_res_folders.save({}, blade.entity, function (data) {
            	blade.parentBlade.initialize();
            	bladeNavigationService.closeBlade(blade);
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
        }
        else {
            marketing_dynamicContents_res_folders.update({}, blade.entity, function (data) {
                blade.originalEntity = angular.copy(blade.entity);
                blade.parentBlade.initialize();
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
        }
    }

    blade.deleteFolder = function (data) {
    	marketing_dynamicContents_res_folders.delete({ ids: [data.id] }, function () {
    		var pathSteps = data.outline.split(';');
    		var id = pathSteps[pathSteps.length - 2];
    		blade.parentBlade.chosenFolder = id;
    		blade.parentBlade.initialize();
    	},
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); blade.isLoading = false; });
    }

    $scope.blade.headIcon = 'fa-location-arrow';

    blade.initialize();
}]);