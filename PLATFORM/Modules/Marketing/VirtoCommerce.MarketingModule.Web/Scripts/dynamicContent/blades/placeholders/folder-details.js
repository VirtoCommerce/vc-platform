angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.addFolderPlaceholderController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.folders', 'platformWebApp.bladeNavigationService', function ($scope, marketing_dynamicContents_res_folders, bladeNavigationService) {
    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    var blade = $scope.blade;
    blade.originalEntity = angular.copy(blade.entity);

    blade.initialize = function () {
        if (!blade.isNew) {
            $scope.bladeToolbarCommands = [
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
				        blade.parentBlade.deleteFolder(blade.entity);
				        bladeNavigationService.closeBlade(blade);
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
        if (blade.isNew) {
            marketing_dynamicContents_res_folders.save({}, blade.entity, function (data) {
                bladeNavigationService.closeBlade(blade);
                blade.parentBlade.updateChoosen();
            });
        }
        else {
            marketing_dynamicContents_res_folders.update({}, blade.entity, function (data) {
                blade.originalEntity = angular.copy(blade.entity);
                blade.parentBlade.updateChoosen();
            });
        }
    }

    $scope.bladeHeadIco = 'fa fa-location-arrow';

    blade.initialize();
}]);