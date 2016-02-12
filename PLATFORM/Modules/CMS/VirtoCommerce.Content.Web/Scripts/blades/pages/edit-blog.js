angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.editBlogController', ['$scope', 'platformWebApp.validators', 'virtoCommerce.contentModule.pages', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, validators, pages, bladeNavigationService, dialogService) {
    $scope.validators = validators;
    var formScope;
    $scope.setForm = function (form) { $scope.formScope = formScope = form; }
    
    var blade = $scope.blade;
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
				        return !angular.equals(blade.originalEntity, blade.entity) && formScope.$valid;
				    },
				    permission: 'content:update'
				},
				{
				    name: "platform.commands.reset", icon: 'fa fa-undo',
				    executeMethod: function () {
				        blade.entity.name = blade.originalEntity.name;
				    },
				    canExecuteMethod: function () {
				        return blade.entity.name !== blade.originalEntity.name;
				    },
				    permission: 'content:update'
				},
                {
                    name: "platform.commands.delete", icon: 'fa fa-trash',
                    executeMethod: function () {
                        var dialog = {
                            id: "confirmDeleteContentItem",
                            title: "content.dialogs.blog-delete.title",
                            message: "content.dialogs.blog-delete.message",
                            callback: function (remove) {
                                if (remove) {
                                    blade.deleteBlog(blade.entity);
                                    bladeNavigationService.closeBlade(blade);
                                }
                            }
                        };

                        dialogService.showConfirmationDialog(dialog);
                    },
                    canExecuteMethod: function () {
                        return true;
                    },
                    permission: 'content:update'
                }
            ];
        }

        blade.isLoading = false;
    }

    blade.saveChanges = function () {
        blade.isLoading = true;

        if (blade.isNew) {
            pages.createBlog({ storeId: blade.choosenStoreId, blogName: blade.entity.name }, {}, function (data) {
                blade.parentBlade.isLoading = true;
                blade.parentBlade.initialize();
                bladeNavigationService.closeBlade(blade);
                blade.isLoading = false;
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); blade.isLoading = false; });
        }
        else {
            pages.updateBlog({ storeId: blade.choosenStoreId, blogName: blade.entity.name, oldBlogName: blade.originalEntity.name }, {}, function (data) {
                blade.parentBlade.initialize();
                blade.parentBlade.checkPreviousStep();
                blade.originalEntity = angular.copy(blade.entity);
                blade.isLoading = false;
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); blade.isLoading = false; });
        }
    }

    blade.deleteBlog = function (data) {
        pages.deleteBlog({ storeId: blade.choosenStoreId, blogName: blade.entity.name }, function () {
            blade.parentBlade.isLoading = true;
            blade.parentBlade.initialize();
            blade.parentBlade.checkPreviousStep();
            bladeNavigationService.closeBlade(blade);
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); blade.isLoading = false; });
    }

    $scope.blade.headIcon = 'fa-inbox';

    blade.initialize();
}]);