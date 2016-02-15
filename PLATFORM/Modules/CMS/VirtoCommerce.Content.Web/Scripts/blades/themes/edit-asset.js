angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.editAssetController', ['$scope', 'platformWebApp.validators', 'platformWebApp.dialogService', 'virtoCommerce.contentModule.themes', '$timeout', 'platformWebApp.bladeNavigationService', function ($scope, validators, dialogService, themes, $timeout, bladeNavigationService) {
    var blade = $scope.blade;
    var codemirrorEditor;

    $scope.validators = validators;
    var formScope;
    $scope.setForm = function (form) { formScope = form; }

    blade.initializeBlade = function () {
        blade.origEntity = angular.copy(blade.currentEntity);

        if (!blade.newAsset) {
            themes.getAsset({ storeId: blade.choosenStoreId, themeId: blade.choosenThemeId, assetId: blade.choosenAssetId }, function (data) {
                blade.isLoading = false;
                blade.currentEntity = data;
                blade.origEntity = angular.copy(blade.currentEntity);

                $timeout(function () {
                    if (codemirrorEditor) {
                        codemirrorEditor.refresh();
                        codemirrorEditor.focus();

                        $scope.blade.toolbarCommands.push(
                            {
                                name: "platform.commands.undo", icon: 'fa fa-rotate-left',
                                executeMethod: function () {
                                    codemirrorEditor.undo();
                                },
                                canExecuteMethod: function () {
                                    var history = codemirrorEditor.historySize();
                                    return history.undo > 1;
                                }
                            });
                        $scope.blade.toolbarCommands.push(
                            {
                                name: "platform.commands.redo", icon: 'fa fa-rotate-right',
                                executeMethod: function () {
                                    codemirrorEditor.redo();
                                },
                                canExecuteMethod: function () {
                                    var history = codemirrorEditor.historySize();
                                    return history.redo > 0;
                                }
                            });
                    }
                    blade.origEntity = angular.copy(blade.currentEntity);
                }, 1);
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });

            $scope.blade.toolbarCommands = [
			{
			    name: "platform.commands.save", icon: 'fa fa-save',
			    executeMethod: $scope.saveChanges,
			    canExecuteMethod: function () {
			        return isDirty();
			    },
			    permission: 'content:update'
			},
			{
			    name: "platform.commands.reset", icon: 'fa fa-undo',
			    executeMethod: function () {
			        angular.copy(blade.origEntity, blade.currentEntity);
			    },
			    canExecuteMethod: function () {
			        return isDirty();
			    },
			    permission: 'content:update'
			},
			{
			    name: "platform.commands.delete", icon: 'fa fa-trash-o',
			    executeMethod: function () {
			        deleteEntry();
			    },
			    canExecuteMethod: function () {
			        return !isDirty();
			    },
			    permission: 'content:delete'
			}];
        }
        else {
            $scope.blade.toolbarCommands = [
			{
			    name: "platform.commands.create", icon: 'fa fa-save',
			    executeMethod: $scope.saveChanges,
			    canExecuteMethod: function () {
			        return isDirty() && formScope.$valid;
			    },
			    permission: 'content:update'
			}];

            blade.isLoading = false;
        }
    };

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity) && !angular.isUndefined(blade.currentEntity.name) && !angular.isUndefined(blade.currentEntity.content);
    };

    $scope.saveChanges = function () {
        blade.isLoading = true;

        blade.currentEntity.id = blade.choosenFolder + '/' + blade.currentEntity.name;

        themes.updateAsset({ storeId: blade.choosenStoreId, themeId: blade.choosenThemeId }, blade.currentEntity, function () {
            blade.origEntity = angular.copy(blade.currentEntity);
            blade.parentBlade.initialize();
            if (blade.newAsset) {
                blade.newAsset = false;
                bladeNavigationService.closeBlade(blade);
            }
            else {
                blade.choosenAssetId = blade.currentEntity.id;
                blade.title = blade.currentEntity.id;
                blade.subtitle = 'Edit ' + blade.currentEntity.name;
                blade.newAsset = false;
                blade.initializeBlade();
            }
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    };

    function deleteEntry() {
        var dialog = {
            id: "confirmDelete",
            title: "content.dialogs.asset-delete.title",
            message: "content.dialogs.asset-delete.message",
            callback: function (remove) {
                if (remove) {
                    $scope.blade.isLoading = true;

                    themes.deleteAsset({ storeId: blade.choosenStoreId, themeId: blade.choosenThemeId, assetIds: blade.choosenAssetId }, function () {
                        $scope.bladeClose();
                        $scope.blade.parentBlade.initialize(true);
                    },
                    function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    function isCanSave() {
        if (!angular.isUndefined(blade.currentEntity)) {
            if (!angular.isUndefined(blade.currentEntity.name) && !angular.isUndefined(blade.currentEntity.content)) {
                return true;
            }
            return false;
        }
        else {
            return false;
        }
    }

    function endsWith(str, suffix) {
        return str.indexOf(suffix, str.length - suffix.length) !== -1;
    }

    function getEditorMode() {
        // mode: { name: "htmlmixed" } // html
        // mode: { name: "javascript" } // javascript
        // mode: { name: "javascript", json: true } // json
        // mode: "liquid-html" // liquid html
        // mode: "liquid-css" // liquid css
        // mode: "liquid-javascript" // liquid css

        if (!blade.newAsset) {
            if (endsWith(blade.choosenAssetId, ".json")) {
                return { name: "javascript", json: true };
            }
            else if (endsWith(blade.choosenAssetId, ".js")) {
                return "javascript";
            }
            else if (endsWith(blade.choosenAssetId, ".js.liquid")) {
                return "liquid-javascript";
            }
            else if (endsWith(blade.choosenAssetId, ".css.liquid")) {
                return "liquid-css";
            }
            else if (endsWith(blade.choosenAssetId, ".css")) {
                return "css";
            }
            else if (endsWith(blade.choosenAssetId, ".scss.liquid")) {
                return "liquid-css";
            }
            else if (endsWith(blade.choosenAssetId, ".liquid")) {
                return "liquid-html";
            }

        }
        return "htmlmixed";
    }

    blade.onClose = function (closeCallback) {
        if ((isDirty() && !blade.newAsset) || (isCanSave() && blade.newAsset)) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "content.dialogs.asset-save.title",
                message: "content.dialogs.asset-save.message",
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

    $scope.blade.headIcon = 'fa-archive';

    // Codemirror configuration
    $scope.editorOptions = {
        lineWrapping: true,
        lineNumbers: true,
        parserfile: "liquid.js",
        extraKeys: { "Ctrl-Q": function (cm) { cm.foldCode(cm.getCursor()); } },
        foldGutter: true,
        gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"],
        onLoad: function (_editor) {
            codemirrorEditor = _editor;
        },
        // mode: 'htmlmixed'
        mode: getEditorMode()
    };

    blade.getBladeStyle = function () {
        var value = $(window).width() - 550;

        return 'width:' + value + 'px';
    }

    blade.initializeBlade();
}]);
