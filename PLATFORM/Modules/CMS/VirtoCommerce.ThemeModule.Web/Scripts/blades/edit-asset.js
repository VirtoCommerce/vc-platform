angular.module('virtoCommerce.content.themeModule.blades.editAsset', [
	'virtoCommerce.content.themeModule.resources.themes'
])
.controller('editAssetController', ['$scope', 'dialogService', 'themes', '$timeout', function ($scope, dialogService, themes, $timeout) {
    var blade = $scope.blade;
    var codemirrorEditor;

    function initializeBlade() {
        if (!blade.newAsset) {
            themes.getAsset({ storeId: blade.choosenStoreId, themeId: blade.choosenThemeId, assetId: blade.choosenAssetId }, function (data) {
                blade.isLoading = false;
                blade.currentEntity = data;

                $timeout(function () {
                    if (codemirrorEditor) {
                        codemirrorEditor.refresh();
                        codemirrorEditor.focus();
                    }
                    blade.origEntity = angular.copy(blade.currentEntity);
                }, 1);
            });

            $scope.bladeToolbarCommands = [
			{
			    name: "Save", icon: 'fa fa-save',
			    executeMethod: function () {
			        $scope.saveChanges();
			    },
			    canExecuteMethod: function () {
			        return isDirty();
			    }
			},
			{
			    name: "Reset", icon: 'fa fa-undo',
			    executeMethod: function () {
			        angular.copy(blade.origEntity, blade.currentEntity);
			    },
			    canExecuteMethod: function () {
			        return isDirty();
			    }
			},
			{
			    name: "Delete", icon: 'fa fa-trash-o',
			    executeMethod: function () {
			        deleteEntry();
			    },
			    canExecuteMethod: function () {
			        return !isDirty();
			    }
			}];
        }
        else {
            blade.isLoading = false;
        }
    };

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity);
    };

    $scope.saveChanges = function() {
        blade.isLoading = true;

        themes.updateAsset({ storeId: blade.choosenStoreId, themeId: blade.choosenThemeId }, blade.currentEntity, function () {
            blade.parentBlade.refresh(true);
            blade.choosenAssetId = blade.currentEntity.id;
            blade.title = blade.currentEntity.id;
            blade.subtitle = 'Edit asset';
            blade.newAsset = false;
            initializeBlade();
        });
    };

    function deleteEntry() {
        var dialog = {
            id: "confirmDelete",
            title: "Delete confirmation",
            message: "Are you sure you want to delete this asset?",
            callback: function (remove) {
                if (remove) {
                    $scope.blade.isLoading = true;

                    themes.deleteAsset({ storeId: blade.choosenStoreId, themeId: blade.choosenThemeId, assetIds: blade.choosenAssetId }, function () {
                        $scope.bladeClose();
                        $scope.blade.parentBlade.refresh(true);
                    });
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
                title: "Save changes",
                message: "The asset has been modified. Do you want to save changes?",
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

    initializeBlade();
}]);
