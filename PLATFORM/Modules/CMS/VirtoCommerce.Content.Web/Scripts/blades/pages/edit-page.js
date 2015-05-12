angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.editPageController', ['$scope', 'platformWebApp.dialogService', 'virtoCommerce.contentModule.stores', 'virtoCommerce.contentModule.pages', '$timeout', 'platformWebApp.bladeNavigationService', 'FileUploader', function ($scope, dialogService, pagesStores, pages, $timeout, bladeNavigationService, FileUploader) {
    var blade = $scope.blade;
    var codemirrorEditor;

    blade.initialize = function () {
        pagesStores.get({ id: blade.choosenStoreId }, function (data) {
            blade.languages = data.languages;
            blade.defaultStoreLanguage = data.defaultLanguage;
            blade.parentBlade.initialize();

            if (!blade.newPage) {
                pages.getPage({ storeId: blade.choosenStoreId, language: blade.choosenPageLanguage, pageName: blade.choosenPageName }, function (data) {
                    blade.isLoading = false;
                    blade.currentEntity = data;
                    blade.isByteContent = blade.isFile();

                    if (!blade.isFile()) {
                    $timeout(function () {
                        if (codemirrorEditor) {
                            codemirrorEditor.refresh();
                            codemirrorEditor.focus();
                        }
                        blade.origEntity = angular.copy(blade.currentEntity);
                    }, 1);
                    }
                    else {
                    	if (blade.isImage()) {
                    		blade.image = "data:" + blade.currentEntity.contentType + ";base64," + blade.currentEntity.byteContent;
                    	}
                    }

                    blade.origEntity = angular.copy(blade.currentEntity);
                });
            }
            else {
				blade.currentEntity.language = blade.defaultStoreLanguage;
				blade.isByteContent = blade.isFile();
				blade.isLoading = false;
				blade.origEntity = angular.copy(blade.currentEntity);
			}

            blade.initializeUploader();
            blade.initializeButtons();
        });
    };

    blade.initializeUploader = function () {
    	if (blade.isFile()) {
    		if (!$scope.uploader) {
    			// Creates a uploader
    			var uploader = $scope.uploader = new FileUploader({
    				scope: $scope,
    				headers: { Accept: 'application/json' },
    				url: 'api/assets',
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
    		}
    	}
    }

    blade.initializeButtons = function () {
    	$scope.bladeToolbarCommands = [];

    	if(!blade.newPage){
    		$scope.bladeToolbarCommands.push(
				{
				    name: "Save page", icon: 'fa fa-save',
					executeMethod: function () { $scope.saveChanges(); }, canExecuteMethod: function () { return blade.isDirty(); }, permission: 'content:manage'
				});
    		$scope.bladeToolbarCommands.push(
				{
				    name: "Reset page", icon: 'fa fa-undo',
					executeMethod: function () { angular.copy(blade.origEntity, blade.currentEntity); }, canExecuteMethod: function () { return blade.isDirty(); }, permission: 'content:manage'
				});
    		$scope.bladeToolbarCommands.push(
				{
				    name: "Delete page", icon: 'fa fa-trash-o',
					executeMethod: function () { blade.deleteEntry(); }, canExecuteMethod: function () { return true; }, permission: 'content:manage'
				});
            }
            else {
    		$scope.bladeToolbarCommands.push(
				{
				    name: "Create", icon: 'fa fa-save',
					executeMethod: function () { $scope.saveChanges(); }, canExecuteMethod: function () { return blade.isDirty(); }, permission: 'content:manage'
				});
    	}
    }

    blade.isDirty = function () {
    	var retVal = !angular.equals(blade.currentEntity, blade.origEntity);

    	if (!angular.isUndefined($scope.formScope)) {
    		retVal = retVal && !$scope.formScope.$invalid;
            }
    	return retVal;
    };

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
            pages.checkName({ storeId: blade.choosenStoreId, pageName: blade.currentEntity.name, language: blade.currentEntity.language }, function (data) {
                if (Boolean(data.result)) {
                    pages.update({ storeId: blade.choosenStoreId }, blade.currentEntity, function () {
                        blade.origEntity = angular.copy(blade.currentEntity);
                        blade.newPage = false;
                        bladeNavigationService.closeBlade(blade);
                        blade.parentBlade.initialize();
                    });
                }
                else {
                    blade.isLoading = false;
                    var dialog = {
                        id: "errorInName",
                        title: "Name not unique",
                        message: "Name must be unique for this language!",
                        callback: function (remove) {

                        }
                    }
                    dialogService.showNotificationDialog(dialog);
                }
            });
        }
        else {
            pages.update({ storeId: blade.choosenStoreId }, blade.currentEntity, function () {
                blade.parentBlade.initialize();
                blade.choosenPageName = blade.currentEntity.name;
                blade.choosenPageLanguage = blade.currentEntity.language;
                blade.title = blade.currentEntity.name;
                blade.subtitle = 'Edit page';
                blade.newPage = false;
                blade.initialize();
            });
        }
    };

    blade.deleteEntry = function() {
        var dialog = {
            id: "confirmDelete",
            title: "Delete confirmation",
            message: "Are you sure you want to delete this page?",
            callback: function (remove) {
                if (remove) {
                    blade.isLoading = true;

                    pages.delete({ storeId: blade.choosenStoreId, pageNamesAndLanguges: blade.choosenPageLanguage + '^' + blade.choosenPageName }, function () {
                        $scope.bladeClose();
                        blade.parentBlade.initialize();
                    });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    function isCanSave() {
        return (!(angular.isUndefined(blade.currentEntity.name) || blade.currentEntity.name === null) &&
			!(angular.isUndefined(blade.currentEntity.content) || blade.currentEntity.content === null));
    }

    blade.onClose = function (closeCallback) {
        if ((blade.isDirty() && !blade.newPage) || (isCanSave() && blade.newPage)) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "Save changes",
                message: "The page has been modified. Do you want to save changes?",
                callback: function (needSave) {
                    if (needSave) {
                        $scope.saveChanges();
                    }
                    closeCallback();
                }
            }
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeCallback();
        }
    };

    $scope.bladeHeadIco = 'fa fa-archive';

    blade.getFlag = function (lang) {
        switch (lang) {
            case 'ru-RU':
                return 'ru';

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

    // Codemirror configuration
    $scope.editorOptions = {
        lineWrapping: true,
        lineNumbers: true,
        extraKeys: { "Ctrl-Q": function (cm) { cm.foldCode(cm.getCursor()); } },
        foldGutter: true,
        gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"],
        onLoad: function (_editor) {
            codemirrorEditor = _editor;
        },
        //mode: 'xml'
        mode: 'htmlmixed'
    };

    blade.getBladeStyle = function () {
        var value = $(window).width() - 550;

        return 'width:' + value + 'px';
    }

    blade.initialize();
}]);
