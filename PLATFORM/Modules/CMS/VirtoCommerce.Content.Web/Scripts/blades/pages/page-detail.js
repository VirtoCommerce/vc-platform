angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.pageDetailController', ['$rootScope', '$scope', 'platformWebApp.validators', 'platformWebApp.dialogService', 'virtoCommerce.contentModule.contentApi', '$timeout', 'platformWebApp.bladeNavigationService', 'platformWebApp.dynamicProperties.api', 'platformWebApp.settings', 'FileUploader', function ($rootScope, $scope, validators, dialogService, contentApi, $timeout, bladeNavigationService, dynamicPropertiesApi, settings, FileUploader) {
    var blade = $scope.blade;
    $scope.validators = validators;
    var contentType = blade.contentType.substr(0, 1).toUpperCase() + blade.contentType.substr(1, blade.contentType.length - 1);
    $scope.fileUploader = new FileUploader({
        url: 'api/platform/assets?folderUrl=cms-content/' + contentType + '/' + blade.storeId + '/assets',
        headers: { Accept: 'application/json' },
        autoUpload: true,
        removeAfterUpload: true,
        filters: [{
            name: 'imageFilter',
            fn: function (i, options) {
                var type = '|' + i.type.slice(i.type.lastIndexOf('/') + 1) + '|';
                return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
            }
        }],
        onBeforeUploadItem: function (fileItem) {
            blade.isLoading = true;
        },
        onSuccessItem: function (fileItem, response, status, headers) {
            $scope.$broadcast('filesUploaded', { items: response });
        },
        onErrorItem: function (fileItem, response, status, headers) {
            bladeNavigationService.setError(fileItem._file.name + ' failed: ' + (response.message ? response.message : status), blade);
        },
        onCompleteAll: function () {
            blade.isLoading = false;
        }
    });

    blade.editAsMarkdown = true;
    blade.editAsHtml = false;

    blade.initializeBlade = function () {
        if (blade.isNew) {
            fillMetadata({});
        } else {
            contentApi.getWithMetadata({
                contentType: blade.contentType,
                storeId: blade.storeId,
                relativeUrl: blade.currentEntity.relativeUrl
            },
            fillMetadata,
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        }
    };

    function fillMetadata(data) {
        var blobName = blade.currentEntity.name || '';
        var idx = blobName.lastIndexOf('.');
        if (idx >= 0) {
            blobName = blobName.substring(0, idx);
            idx = blobName.lastIndexOf('.'); // language
            if (idx >= 0) {
                blade.currentEntity.language = blobName.substring(idx + 1);
            }
        }

        blade.currentEntity.content = data.content;

        dynamicPropertiesApi.query({ id: 'VirtoCommerce.Content.Web.FrontMatterHeaders' },
            function (results) {
                fillDynamicProperties(data.metadata, results);
                $scope.$broadcast('resetContent', { body: blade.currentEntity.content });
                $timeout(function () {
                    blade.origEntity = angular.copy(blade.currentEntity);
                });
                blade.isLoading = false;
            }, function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    }

    function fillDynamicProperties(metadata, props) {
        _.each(props, function (x) {
            x.displayNames = undefined;
            var metadataRecord = _.findWhere(metadata, { name: x.name });
            x.values = metadataRecord ? metadataRecord.values : [];
        });

        blade.currentEntity.dynamicProperties = props;
    }

    $scope.saveChanges = function () {
        blade.isLoading = true;

        contentApi.saveWithMetadata({
            contentType: blade.contentType,
            storeId: blade.storeId,
            folderUrl: blade.folderUrl || ''
        }, blade.currentEntity,
        function () {
            blade.isLoading = false;
            blade.origEntity = angular.copy(blade.currentEntity);
            if (blade.isNew) {
                $scope.bladeClose();
                $rootScope.$broadcast("cms-statistics-changed", blade.storeId);
            }

            blade.parentBlade.refresh();
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

    if (!blade.isNew)
        blade.toolbarCommands = [
            {
                name: "platform.commands.save", icon: 'fa fa-save',
                executeMethod: $scope.saveChanges,
                canExecuteMethod: function () { return isDirty() && formScope && formScope.$valid; },
                permission: blade.updatePermission
            },
            {
                name: "platform.commands.reset", icon: 'fa fa-undo',
                executeMethod: function () {
                    angular.copy(blade.origEntity, blade.currentEntity);
                    $scope.$broadcast('resetContent', { body: blade.currentEntity.content });
                },
                canExecuteMethod: isDirty,
                permission: blade.updatePermission
            },
            //{
            //    name: "platform.commands.delete", icon: 'fa fa-trash-o',
            //    executeMethod: blade.deleteEntry,
            //    canExecuteMethod: function () { return true; },
            //    permission: 'content:delete'
            //},
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

    blade.toolbarCommands = blade.toolbarCommands || [];
    blade.toolbarCommands.push(
		{
		    name: "content.commands.manage-metadata", icon: 'fa fa-edit',
		    executeMethod: function () {
		        var newBlade = {
		            id: 'dynamicPropertyList',
		            objectType: 'VirtoCommerce.Content.Web.FrontMatterHeaders',
		            parentRefresh: function (props) { fillDynamicProperties(blade.currentEntity.dynamicProperties, props); },
		            controller: 'platformWebApp.dynamicPropertyListController',
		            template: '$(Platform)/Scripts/app/dynamicProperties/blades/dynamicProperty-list.tpl.html'
		        };
		        bladeNavigationService.showBlade(newBlade, blade);
		    },
		    canExecuteMethod: function () { return true; }
		}
    );

    var formScope;
    $scope.setForm = function (form) { formScope = form; }

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
    }

    function canSave() {
        return blade.currentEntity && blade.currentEntity.name && ((isDirty() && !blade.isNew) || (blade.currentEntity.content && blade.isNew));
    }

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, $scope.saveChanges, closeCallback, "content.dialogs.page-save.title", "content.dialogs.page-save.message");
    };

    // dynamic properties (metadata)
    //$scope.editDictionary = function (property) {
    //    var newBlade = {
    //        id: "propertyDictionary",
    //        isApiSave: true,
    //        currentEntity: property,
    //        controller: 'platformWebApp.propertyDictionaryController',
    //        template: '$(Platform)/Scripts/app/dynamicProperties/blades/property-dictionary.tpl.html',
    //        onChangesConfirmedFn: function () {
    //            // blade.entity.dynamicProperties = angular.copy(blade.entity.dynamicProperties);
    //        }
    //    };
    //    bladeNavigationService.showBlade(newBlade, blade);
    //};

    //$scope.getDictionaryValues = function (property, callback) {
    //    dictionaryItemsApi.query({ id: property.objectType, propertyId: property.id }, callback);
    //}

    ////settings.getValues({ id: 'VirtoCommerce.Core.General.Languages' }, function (data) {
    ////    $scope.languages = data;
    ////});
    //$scope.languages = settings.getValues({ id: 'VirtoCommerce.Core.General.Languages' });

    blade.headIcon = 'fa-file-o';
    blade.initializeBlade();
}]);
