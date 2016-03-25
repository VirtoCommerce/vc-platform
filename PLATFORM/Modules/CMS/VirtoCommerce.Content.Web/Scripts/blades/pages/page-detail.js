angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.pageDetailController', ['$scope', 'platformWebApp.validators', 'platformWebApp.dialogService', 'virtoCommerce.contentModule.contentApi', '$http', 'platformWebApp.bladeNavigationService', function ($scope, validators, dialogService, contentApi, $http, bladeNavigationService) {
    $scope.validators = validators;
    var formScope;
    $scope.setForm = function (form) { formScope = form; }

    var blade = $scope.blade;
    blade.editAsMarkdown = true;
    blade.editAsHtml = false;

    blade.initializeBlade = function () {
        if (blade.isNew) {
            blade.toolbarCommands = [
                {
                    name: "platform.commands.create", icon: 'fa fa-save',
                    executeMethod: $scope.saveChanges,
                    canExecuteMethod: function () { return isDirty() && formScope && formScope.$valid; },
                    permission: 'content:create'
                }
            ];

            blade.isLoading = false;
        } else {
            $http.get(blade.currentEntity.url, {
                responseType: 'text',
            }).then(function (results) {
                blade.isLoading = false;
                blade.currentEntity.content = results.data;
                blade.origEntity = angular.copy(blade.currentEntity);
                $scope.$broadcast('resetContent', { body: blade.currentEntity.content });
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });


            blade.toolbarCommands = [
                {
                    name: "content.commands.save-page", icon: 'fa fa-save',
                    executeMethod: $scope.saveChanges,
                    canExecuteMethod: function () { return isDirty() && formScope && formScope.$valid; },
                    permission: blade.updatePermission
                },
                {
                    name: "content.commands.reset-page", icon: 'fa fa-undo',
                    executeMethod: function () {
                        angular.copy(blade.origEntity, blade.currentEntity);
                        $scope.$broadcast('resetContent', { body: blade.currentEntity.content });
                    },
                    canExecuteMethod: isDirty,
                    permission: blade.updatePermission
                },
                {
                    name: "content.commands.delete-page", icon: 'fa fa-trash-o',
                    executeMethod: blade.deleteEntry,
                    canExecuteMethod: function () { return true; },
                    permission: 'content:delete'
                },
                {
                    name: "content.commands.edit-as-markdown", icon: 'fa fa-code',
                    executeMethod: function () {
                        blade.editAsMarkdown = true;
                        blade.editAsHtml = false;
                        $scope.$broadcast('changeEditType', { editAsMarkdown: true, editAsHtml: false });
                    },
                    canExecuteMethod: function () { return !blade.editAsMarkdown; },
                    permission: blade.updatePermission
                },
                {
                    name: "content.commands.edit-as-html", icon: 'fa fa-code',
                    executeMethod: function () {
                        blade.editAsHtml = true;
                        blade.editAsMarkdown = false;
                        $scope.$broadcast('changeEditType', { editAsHtml: true, editAsMarkdown: false });
                    },
                    canExecuteMethod: function () { return !blade.editAsHtml; },
                    permission: blade.updatePermission
                }
            ];
        }
        blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
    }

    $scope.saveChanges = function () {
        blade.isLoading = true;

        var fd = new FormData();
        fd.append(blade.currentEntity.name, blade.currentEntity.content);
        var folderParameter = '?folderUrl=' + (blade.folderUrl ? blade.folderUrl : '');
        $http.post('api/content/' + blade.contentType + '/' + blade.storeId + folderParameter, fd,
            {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            }).then(function (results) {
                blade.parentBlade.refresh();
                if (blade.isNew) {
                    blade.origEntity = blade.currentEntity;
                    $scope.bladeClose();
                } else {
                    blade.initializeBlade();
                }

            }, function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    };

    blade.deleteEntry = function () {
        var dialog = {
            id: "confirmDelete",
            title: "content.dialogs.page-delete.title",
            message: "content.dialogs.page-delete.message",
            callback: function (remove) {
                if (remove) {
                    blade.isLoading = true;

                    contentApi.delete({
                        contentType: blade.contentType,
                        storeId: blade.storeId,
                        urls: [blade.currentEntity.url]
                    }, function () {
                        blade.currentEntity = blade.origEntity;
                        $scope.bladeClose();
                        blade.parentBlade.refresh();
                    },
                    function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    };

    function canSave() {
        return blade.currentEntity && blade.currentEntity.name && ((isDirty() && !blade.isNew) || (blade.currentEntity.content && blade.isNew));
    }

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, $scope.saveChanges, closeCallback, "content.dialogs.page-save.title", "content.dialogs.page-save.message");
    };


    blade.headIcon = 'fa-file-o';
    blade.initializeBlade();
}]);
