angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.addContentItemsController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.contentItems', 'platformWebApp.bladeNavigationService', function ($scope, marketing_dynamicContents_res_contentItems, bladeNavigationService) {
    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    var blade = $scope.blade;

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
				        blade.delete();
				    },
				    canExecuteMethod: function () {
				        return true;
				    },
				    permission: 'marketing:manage'
				},
				{
					name: "Manage type properties", icon: 'fa fa-edit',
					executeMethod: function () {
						var newBlade = {
							id: 'dynamicPropertyList',
							objectType: blade.entity.objectType,
							controller: 'platformWebApp.dynamicPropertyListController',
							template: 'Scripts/app/dynamicProperties/blades/dynamicProperty-list.tpl.html'
						};
						bladeNavigationService.showBlade(newBlade, blade);
					},
					canExecuteMethod: function () {
						return angular.isDefined(blade.entity.objectType);
					}
				}
            ];
        }

        blade.originalEntity = angular.copy(blade.entity);

        blade.isLoading = false;
    }

    blade.delete = function () {
        marketing_dynamicContents_res_contentItems.delete({ ids: [blade.entity.id] }, function () {
            blade.parentBlade.updateChoosen();
            bladeNavigationService.closeBlade(blade);
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    }

    blade.saveChanges = function () {
        if (blade.isNew) {
            marketing_dynamicContents_res_contentItems.save({}, blade.entity, function (data) {
                blade.parentBlade.updateChoosen();
                bladeNavigationService.closeBlade(blade);
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
        }
        else {
            marketing_dynamicContents_res_contentItems.update({}, blade.entity, function (data) {
                blade.parentBlade.updateChoosen();
                blade.originalEntity = angular.copy(blade.entity);
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
        }
    }

    $scope.blade.headIcon = 'fa-inbox';

    blade.initialize();
}]);