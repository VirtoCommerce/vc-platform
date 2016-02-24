angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.editPageController', ['$scope', 'platformWebApp.validators', 'platformWebApp.dialogService', 'virtoCommerce.contentModule.stores', 'virtoCommerce.contentModule.pages', '$timeout', 'platformWebApp.bladeNavigationService', 'FileUploader', function ($scope, validators, dialogService, pagesStores, pages, $timeout, bladeNavigationService, FileUploader) {
    $scope.validators = validators;
    var formScope;
    $scope.setForm = function (form) { formScope = form; }

    var blade = $scope.blade;
    blade.updatePermission = 'content:update';
    blade.editAsMarkdown = true;
    blade.editAsHtml = false;

    blade.initialize = function () {
        pagesStores.get({ id: blade.choosenStoreId }, function (data) {
            blade.languages = data.languages;
            blade.defaultStoreLanguage = data.defaultLanguage;
            blade.parentBlade.initialize();

            if (!blade.newPage) {
                pages.getPage({ storeId: blade.choosenStoreId, language: blade.choosenPageLanguage ? blade.choosenPageLanguage : "undef", pageName: blade.choosenPageName }, function (data) {
                    blade.isLoading = false;
                    blade.currentEntity = data;

                    blade.isByteContent = blade.isFile();

                    if (!blade.isFile()) {
                        var parts = blade.currentEntity.content.split('---');
                        if (parts.length > 2) {
                            blade.body = parts[2].trim();
                            blade.metadata = parts[1].trim();
                        }
                        else {
                            blade.body = parts[0];
                        }
                    }
                    else {
                        if (blade.isImage()) {
                            blade.image = "data:" + blade.currentEntity.contentType + ";base64," + blade.currentEntity.byteContent;
                        }
                    }

                    var pathParts = blade.currentEntity.name.split('/');
                    var pageNameWithExtension = pathParts[pathParts.length - 1];
                    var pageName = pageNameWithExtension.split('.')[0];
                    blade.currentEntity.pageName = pageName;

                    blade.origEntity = angular.copy(blade.currentEntity);
                },
                function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
            }
            else {
                blade.currentEntity.language = blade.defaultStoreLanguage;
                blade.isByteContent = blade.isFile();
                blade.isLoading = false;
                blade.currentEntity.content = "---\n\n---\n"
                blade.origEntity = angular.copy(blade.currentEntity);

                var parts = blade.currentEntity.content.split('---');
                blade.body = parts[2].trim();
                blade.metadata = parts[1].trim();
            }

            blade.initializeUploader();
            blade.initializeButtons();
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    };

    blade.initializeUploader = function () {
        if (blade.isFile()) {
            if (!$scope.uploader && blade.hasUpdatePermission()) {
                // create the uploader
                var uploader = $scope.uploader = new FileUploader({
                    scope: $scope,
                    headers: { Accept: 'application/json' },
                    url: 'api/platform/assets?folderUrl=pages',
                    autoUpload: true,
                    removeAfterUpload: true
                });

                uploader.onBeforeUploadItem = function (item) {
                    if (blade.newPage) {
                        blade.currentEntity.name = blade.path + item._file.name;
                    }
                };

                uploader.onSuccessItem = function (fileItem, images, status, headers) {
                    angular.forEach(images, function (image) {
                        blade.currentEntity.contentType = image.mimeType;
                        blade.currentEntity.name = blade.currentEntity.name.replace('new_file', image.name);
                        blade.currentEntity.fileUrl = image.url;
                        blade.image = image.url;
                    });
                };

                uploader.onAfterAddingAll = function (addedItems) {
                    bladeNavigationService.setError(null, blade);
                };

                uploader.onErrorItem = function (item, response, status, headers) {
                    bladeNavigationService.setError(item._file.name + ' failed: ' + (response.message ? response.message : status), blade);
                };
            }
        }
    }

    blade.initializeButtons = function () {
        blade.toolbarCommands = [];

        if (!blade.newPage) {
            blade.toolbarCommands.push(
				{
				    name: "content.commands.save-page", icon: 'fa fa-save',
				    executeMethod: $scope.saveChanges,
				    canExecuteMethod: function () { return isDirty() && formScope.$valid; },
				    permission: blade.updatePermission
				});
            blade.toolbarCommands.push(
				{
				    name: "content.commands.reset-page", icon: 'fa fa-undo',
				    executeMethod: function () {
				        angular.copy(blade.origEntity, blade.currentEntity);
				        var parts = blade.currentEntity.content.split('---');
				        if (parts.length > 2) {
				            blade.body = parts[2].trim();
				            blade.metadata = parts[1].trim();
				        }
				        else {
				            blade.body = parts[0];
				        }
				        $scope.$broadcast('resetContent', { body: blade.body });
				    },
				    canExecuteMethod: isDirty,
				    permission: blade.updatePermission
				});
            blade.toolbarCommands.push(
				{
				    name: "content.commands.delete-page", icon: 'fa fa-trash-o',
				    executeMethod: blade.deleteEntry,
				    canExecuteMethod: function () { return true; },
				    permission: 'content:delete'
				});
            blade.toolbarCommands.push(
                {
                    name: "content.commands.edit-as-markdown", icon: 'fa fa-code',
                    executeMethod: function () {
                        blade.editAsMarkdown = true;
                        blade.editAsHtml = false;
                        $scope.$broadcast('changeEditType', { editAsMarkdown: true, editAsHtml: false });
                    },
                    canExecuteMethod: function () { return !blade.editAsMarkdown; },
                    permission: 'content:manage'
                });
            blade.toolbarCommands.push(
                {
                    name: "content.commands.edit-as-html", icon: 'fa fa-code',
                    executeMethod: function () {
                        blade.editAsHtml = true;
                        blade.editAsMarkdown = false;
                        $scope.$broadcast('changeEditType', { editAsHtml: true, editAsMarkdown: false });
                    },
                    canExecuteMethod: function () { return !blade.editAsHtml; },
                    permission: 'content:manage'
                });
        }
        else {
            blade.toolbarCommands.push(
				{
				    name: "platform.commands.create", icon: 'fa fa-save',
				    executeMethod: $scope.saveChanges,
				    canExecuteMethod: function () { return isDirty() && formScope.$valid; },
				    permission: 'content:create'
				});
        }
    }

    function isDirty() {
        if (!angular.isUndefined(blade.currentEntity) && !blade.isFile()) {
            if (blade.currentEntity.content.indexOf('---\n') !== -1 || blade.metadata !== '') {
                blade.currentEntity.content = '---\n' + blade.metadata.trim() + '\n---\n' + blade.body.trim();
            }
            else {
                blade.currentEntity.content = blade.body;
            }
        }
        var retVal = !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
        return retVal;
    }

    blade.isFile = function () {
        if (!angular.isUndefined(blade.currentEntity)) {
            if (blade.currentEntity.contentType === 'text/html' ||
				blade.currentEntity.contentType === 'application/json' ||
				blade.currentEntity.contentType === 'application/javascript') {

                return false;
            }
        }

        return true;
    }

    blade.isImage = function () {
        if (!angular.isUndefined(blade.currentEntity)) {
            if (blade.currentEntity.contentType === 'image/png' ||
				blade.currentEntity.contentType === 'image/bmp' ||
				blade.currentEntity.contentType === 'image/gif' ||
				blade.currentEntity.contentType === 'image/jpeg') {

                return true;
            }
        }

        return false;
    }

    $scope.saveChanges = function () {
        blade.isLoading = true;

        if (blade.newPage) {
            if (blade.isByteContent) {
                blade.currentEntity.language = "files";
            }

            pages.checkName({ storeId: blade.choosenStoreId, pageName: blade.currentEntity.name, language: blade.currentEntity.language }, function (data) {
                if (Boolean(data.result)) {
                    blade.setNewName();

                    pages.update({ storeId: blade.choosenStoreId }, blade.currentEntity, function () {
                        blade.origEntity = angular.copy(blade.currentEntity);
                        blade.newPage = false;
                        blade.parentBlade.initialize();
                        bladeNavigationService.closeBlade(blade);
                    },
                    function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                }
                else {
                    blade.isLoading = false;
                    var dialog = {
                        id: "errorInName",
                        title: "content.dialogs.name-must-unique.title",
                        message: "content.dialogs.name-must-unique.message",
                        callback: function (remove) {

                        }
                    }
                    dialogService.showNotificationDialog(dialog);
                }
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        }
        else {
            if (blade.origEntity.pageName !== blade.currentEntity.pageName) {
                pages.delete({ storeId: blade.choosenStoreId, pageNamesAndLanguges: blade.choosenPageLanguage + '^' + blade.choosenPageName }, function () {
                    blade.setNewName(blade.choosenPageName);
                    blade.update();
                }, function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
            }
            else if (blade.origEntity.language !== blade.currentEntity.language) {
                pages.delete({ storeId: blade.choosenStoreId, pageNamesAndLanguges: blade.choosenPageLanguage + '^' + blade.choosenPageName }, function () {
                    blade.update();
                }, function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
            }
            else {
                blade.update();
            }
        }
    };

    blade.deleteEntry = function () {
        var dialog = {
            id: "confirmDelete",
            title: "content.dialogs.page-delete.title",
            message: "content.dialogs.page-delete.message",
            callback: function (remove) {
                if (remove) {
                    blade.isLoading = true;

                    pages.delete({ storeId: blade.choosenStoreId, pageNamesAndLanguges: blade.choosenPageLanguage + '^' + blade.choosenPageName }, function () {
                        $scope.bladeClose();
                        blade.parentBlade.initialize();
                    },
                    function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    };

    function canSave() {
        return blade.currentEntity && blade.currentEntity.name && ((isDirty() && !blade.newPage) || (blade.currentEntity.content && blade.newPage));
    }

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, $scope.saveChanges, closeCallback, "content.dialogs.page-save.title", "content.dialogs.page-save.message");
    };

    blade.headIcon = 'fa-archive';

    blade.getFlag = function (lang) {
        switch (lang) {
            case 'en-US':
                return 'us';

            case 'fr-FR':
                return 'fr';

            case 'zh-CN':
                return 'ch';

            case 'ru-RU':
                return 'ru';

            case 'ja-JP':
                return 'jp';

            case 'de-DE':
                return 'de';
        }
    };

    blade.update = function () {
        pages.update({ storeId: blade.choosenStoreId }, blade.currentEntity, function () {
            blade.choosenPageName = blade.currentEntity.name;
            blade.choosenPageLanguage = blade.currentEntity.language;
            blade.title = blade.currentEntity.name;
            blade.subtitle = 'Edit page';
            blade.newPage = false;
            blade.parentBlade.initialize();
            blade.isLoading = false,
            blade.origEntity = angular.copy(blade.currentEntity);
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    };

    blade.setNewName = function (pageName) {
        var pathParts = [];
        if (pageName) {
            pathParts = pageName.split('/');
            pathParts = _.initial(pathParts);
            pathParts.push(blade.currentEntity.name);
        }
        else {
            pathParts = blade.currentEntity.name.split('/');
        }
        var pageNameWithExtension = pathParts[pathParts.length - 1];
        var parts = pageNameWithExtension.split('.');

        blade.currentEntity.name = blade.currentEntity.id = _.initial(pathParts).join('/') + (pathParts.length > 1 ? '/' : '') + blade.currentEntity.pageName;
        if (parts.length > 1) {
            blade.currentEntity.name += "." + parts[parts.length - 1];
            blade.currentEntity.id += "." + parts[parts.length - 1];
        }
    }

    blade.initialize();
}]);
