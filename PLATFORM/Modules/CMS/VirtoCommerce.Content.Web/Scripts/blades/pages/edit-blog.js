angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.editBlogController', ['$scope', 'platformWebApp.validators', 'virtoCommerce.contentModule.contentApi', 'platformWebApp.dynamicProperties.api', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, validators, contentApi, dynamicPropertiesApi, bladeNavigationService, dialogService) {
    var blade = $scope.blade;
    blade.updatePermission = 'content:update';
    $scope.validators = validators;

    blade.initialize = function () {
        if (blade.isNew) {
            fillDynamicProperties({});
        } else {
            contentApi.getWithMetadata({
                contentType: blade.contentType,
                storeId: blade.storeId,
                relativeUrl: getBlogBlobName()
            },
            fillDynamicProperties,
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        }
    };

    function fillDynamicProperties(data) {
        dynamicPropertiesApi.query({ id: 'VirtoCommerce.Content.Web.FrontMatterHeaders' }, function (results) {
            _.each(results, function (x) {
                x.displayNames = undefined;
                var metadataRecord = _.findWhere(data.metadata, { name: x.name });
                x.values = metadataRecord ? metadataRecord.values : [];
            });

            blade.currentEntity.dynamicProperties = results;
            blade.origEntity = angular.copy(blade.currentEntity);
            blade.isLoading = false;
        }, function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    }

    $scope.saveChanges = function () {
        blade.isLoading = true;

        contentApi.saveWithMetadata({
            contentType: blade.contentType,
            storeId: blade.storeId,
            folderUrl: ''
        }, {
            dynamicProperties: blade.currentEntity.dynamicProperties,
            content: '',
            name: getBlogBlobName()
        },
        function (data) {
            blade.isLoading = false;
            blade.origEntity = angular.copy(blade.currentEntity);
            if (blade.isNew) {
                $scope.bladeClose();
            }

            blade.parentBlade.refresh();
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); blade.isLoading = false; });
    };

    blade.deleteBlog = function () {
        contentApi.delete({
            contentType: blade.contentType,
            storeId: blade.storeId,
            urls: [blade.currentEntity.url]
        }, function () {
            $scope.bladeClose();
            blade.parentBlade.refresh();
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); blade.isLoading = false; });
    };

    if (!blade.isNew) {
        $scope.blade.toolbarCommands = [
            {
                name: "platform.commands.save", icon: 'fa fa-save',
                executeMethod: $scope.saveChanges,
                canExecuteMethod: canSave,
                permission: blade.updatePermission
            },
            {
                name: "platform.commands.reset", icon: 'fa fa-undo',
                executeMethod: function () {
                    angular.copy(blade.origEntity, blade.currentEntity);
                },
                canExecuteMethod: isDirty,
                permission: blade.updatePermission
            }
            //{
            //    name: "platform.commands.delete", icon: 'fa fa-trash',
            //    executeMethod: function () {
            //        var dialog = {
            //            id: "confirmDeleteContentItem",
            //            title: "content.dialogs.blog-delete.title",
            //            message: "content.dialogs.blog-delete.message",
            //            callback: function (remove) {
            //                if (remove) {
            //                    blade.deleteBlog();                                
            //                }
            //            }
            //        };

            //        dialogService.showConfirmationDialog(dialog);
            //    },
            //    canExecuteMethod: function () { return true; },
            //    permission: blade.updatePermission
            //}
        ];
    }

    function getBlogBlobName() {
        return blade.currentEntity.name + '/' + blade.currentEntity.name + '.md'
    }
    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
    }

    function canSave() {
        return isDirty() && formScope && formScope.$valid;
    }

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, $scope.saveChanges, closeCallback, "content.dialogs.blog-save.title", "content.dialogs.blog-save.message");
    };

    var formScope;
    $scope.setForm = function (form) { $scope.formScope = formScope = form; }

    // $scope.blade.headIcon = 'fa-inbox';

    blade.initialize();
}]);