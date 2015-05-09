angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.editPageController', ['$scope', 'dialogService', 'virtoCommerce.contentModule.stores', 'virtoCommerce.contentModule.pages', '$timeout', 'bladeNavigationService', function ($scope, dialogService, pagesStores, pages, $timeout, bladeNavigationService) {
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
                    
                    $timeout(function () {
                        if (codemirrorEditor) {
                        	codemirrorEditor.refresh();
                            codemirrorEditor.focus();
                        }
                    }, 1);

                    blade.origEntity = angular.copy(blade.currentEntity);
                });

                $scope.bladeToolbarCommands = [
				{
				    name: "Save page", icon: 'fa fa-save',
				    executeMethod: function () {
				        $scope.saveChanges();
				    },
				    canExecuteMethod: function () {
				        return isDirty();
				    },
				    permission: 'content:manage'
				},
				{
				    name: "Reset page", icon: 'fa fa-undo',
				    executeMethod: function () {
				        angular.copy(blade.origEntity, blade.currentEntity);
				    },
				    canExecuteMethod: function () {
				        return isDirty();
				    },
				    permission: 'content:manage'
				},
				{
				    name: "Delete page", icon: 'fa fa-trash-o',
				    executeMethod: function () {
				        deleteEntry();
				    },
				    canExecuteMethod: function () {
				        return !isDirty();
				    },
				    permission: 'content:manage'
				}];
            }
            else {
            	blade.currentEntity.language = blade.defaultStoreLanguage;

                $scope.bladeToolbarCommands = [
				{
					name: "Create", icon: 'fa fa-save',
					executeMethod: function () {
						$scope.saveChanges();
					},
					canExecuteMethod: function () {
						return isDirty();
					},
					permission: 'content:manage'
				}];

                blade.setContentTypeValue();
                blade.isLoading = false;

                blade.origEntity = angular.copy(blade.currentEntity);
            }
        });
    };

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity);
    };

    blade.setContentTypeValue = function () {
    	if (blade.currentEntity.contentType === 'text/html' ||
			blade.contentType === 'application/json' ||
			blade.contentType === 'application/javascript') {

    		return false;
    	}

    	return true;
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

    function deleteEntry() {
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
        if ((isDirty() && !blade.newPage) || (isCanSave() && blade.newPage)) {
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
