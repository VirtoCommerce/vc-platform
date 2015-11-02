angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.editPageController', ['$scope', 'platformWebApp.dialogService', 'virtoCommerce.contentModule.stores', 'virtoCommerce.contentModule.pages', '$timeout', 'platformWebApp.bladeNavigationService', 'FileUploader', function ($scope, dialogService, pagesStores, pages, $timeout, bladeNavigationService, FileUploader) {
    var blade = $scope.blade;
    blade.editAsMarkdown = true;
    blade.editAsHtml = false;

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
                        blade.origEntity = angular.copy(blade.currentEntity);

                        var pathParts = blade.currentEntity.name.split('/');
                        var pageNameWithExtension = pathParts[pathParts.length - 1];
                        var pageName = pageNameWithExtension.split('.')[0];
                        blade.currentEntity.pageName = pageName;

                        blade.origEntity = angular.copy(blade.currentEntity);

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
                },
                function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
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
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    };

    blade.initializeUploader = function () {
    	if (blade.isFile()) {
    		if (!$scope.uploader) {
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
    	$scope.blade.toolbarCommands = [];

    	if(!blade.newPage){
    		$scope.blade.toolbarCommands.push(
				{
				    name: "Save page", icon: 'fa fa-save',
					executeMethod: function () { $scope.saveChanges(); }, canExecuteMethod: function () { return blade.isDirty(); }, permission: 'content:update'
				});
    		$scope.blade.toolbarCommands.push(
				{
				    name: "Reset page", icon: 'fa fa-undo',
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
				    canExecuteMethod: function () { return blade.isDirty(); },
				    permission: 'content:update'
				});
    		$scope.blade.toolbarCommands.push(
				{
				    name: "Delete page", icon: 'fa fa-trash-o',
				    executeMethod: function () { blade.deleteEntry(); }, canExecuteMethod: function () { return true; }, permission: 'content:delete'
				});
    		$scope.blade.toolbarCommands.push(
                {
                    name: "Edit as markdown", icon: 'fa fa-code',
                    executeMethod: function () {
                        blade.editAsMarkdown = true;
                        blade.editAsHtml = false;
                        $scope.$broadcast('changeEditType', { editAsMarkdown: true, editAsHtml: false });
                    },
                    canExecuteMethod: function () { return !blade.editAsMarkdown; },
                    permission: 'content:manage'
                });
    		$scope.blade.toolbarCommands.push(
                {
                    name: "Edit as html", icon: 'fa fa-code',
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
    		$scope.blade.toolbarCommands.push(
				{
				    name: "Create", icon: 'fa fa-save',
					executeMethod: function () { $scope.saveChanges(); }, canExecuteMethod: function () { return blade.isDirty(); }, permission: 'content:update'
				});
    	}
    }

    blade.isDirty = function () {
        if (!angular.isUndefined(blade.currentEntity) && !blade.isFile()) {
            if (blade.currentEntity.content.indexOf('---\n') !== -1 || blade.metadata !== '') {
                blade.currentEntity.content = '---\n' + blade.metadata.trim() + '\n---\n' + blade.body.trim();
            }
            else {
                blade.currentEntity.content = blade.body;
            }
        }
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
                    function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
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
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
        }
        else {
            if (blade.origEntity.pageName !== blade.currentEntity.pageName) {
                pages.delete({ storeId: blade.choosenStoreId, pageNamesAndLanguges: blade.choosenPageLanguage + '^' + blade.choosenPageName }, function () {
                    blade.setNewName();
                    blade.update();
                }, function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
            }
            else if (blade.origEntity.language !== blade.currentEntity.language) {
                pages.delete({ storeId: blade.choosenStoreId, pageNamesAndLanguges: blade.choosenPageLanguage + '^' + blade.choosenPageName }, function () {
                    blade.update();
                }, function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
            }
            else {
                blade.update();
            }
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
                    },
                    function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    blade.closeChildrenBlades = function () {
    	if ($scope.blade.childrenBlades) {
    		angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
    			bladeNavigationService.closeBlade(child);
    		});
    	}
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
                message: "The page has been modified. Do you want to save changes?"
            };

            dialog.callback = function (needSave) {
                if (needSave) {
                    $scope.saveChanges();
                }
                closeCallback();
            };
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeCallback();
        }
    };

    $scope.blade.headIcon = 'fa-archive';

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
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    }

    blade.setNewName = function () {
        var pathParts = blade.currentEntity.name.split('/');
        var pageNameWithExtension = pathParts[pathParts.length - 1];
        var parts = pageNameWithExtension.split('.');

        blade.currentEntity.name = blade.currentEntity.id = pathParts.slice(0, pathParts.length - 1).join('/') + (pathParts.length > 1 ? '/' : '') + blade.currentEntity.pageName;
        if (parts.length > 1) {
            blade.currentEntity.name += "." + parts[parts.length - 1];
            blade.currentEntity.id += "." + parts[parts.length - 1];
        }
    }

    blade.initialize();
}]);
