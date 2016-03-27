angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.editAssetController', ['$scope', 'platformWebApp.validators', 'platformWebApp.dialogService', 'virtoCommerce.contentModule.themes', '$timeout', 'platformWebApp.bladeNavigationService', function ($scope, validators, dialogService, themes, $timeout, bladeNavigationService) {
    var blade = $scope.blade;
    blade.updatePermission = 'content:update';
    var codemirrorEditor;

    $scope.validators = validators;
    var formScope;
    $scope.setForm = function (form) { formScope = form; }

    blade.initializeBlade = function () {
        blade.origEntity = angular.copy(blade.currentEntity);

        if (!blade.newAsset) {
            themes.getAsset({ storeId: blade.chosenStoreId, themeId: blade.chosenThemeId, assetId: blade.chosenAssetId }, function (data) {
                blade.isLoading = false;
                blade.currentEntity = data;
                blade.origEntity = angular.copy(blade.currentEntity);

                $timeout(function () {
                    if (codemirrorEditor) {
                        codemirrorEditor.refresh();
                        codemirrorEditor.focus();

                        blade.toolbarCommands.push(
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
                        blade.toolbarCommands.push(
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
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });

            blade.toolbarCommands = [
			{
			    name: "platform.commands.save", icon: 'fa fa-save',
			    executeMethod: $scope.saveChanges,
			    canExecuteMethod: function () {
			        return isDirty();
			    },
			    permission: blade.updatePermission
			},
			{
			    name: "platform.commands.reset", icon: 'fa fa-undo',
			    executeMethod: function () {
			        angular.copy(blade.origEntity, blade.currentEntity);
			    },
			    canExecuteMethod: isDirty,
			    permission: blade.updatePermission
			},
			{
			    name: "platform.commands.delete", icon: 'fa fa-trash-o',
			    executeMethod: deleteEntry,
			    canExecuteMethod: function () {
			        return true;
			    },
			    permission: 'content:delete'
			}];
        }
        else {
            blade.toolbarCommands = [
			{
			    name: "platform.commands.create", icon: 'fa fa-save',
			    executeMethod: $scope.saveChanges,
			    canExecuteMethod: function () {
			        return isDirty() && formScope.$valid;
			    },
			    permission: 'content:create'
			}];

            blade.isLoading = false;
        }
    };

    function isCanSave() {
        return blade.currentEntity && blade.currentEntity.name && blade.currentEntity.content;
    }

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity)
            && isCanSave()
            && (blade.newAsset || blade.hasUpdatePermission());
    };

    $scope.saveChanges = function () {
        blade.isLoading = true;

        blade.currentEntity.id = blade.chosenFolder + '/' + blade.currentEntity.name;

        themes.updateAsset({ storeId: blade.chosenStoreId, themeId: blade.chosenThemeId }, blade.currentEntity, function () {
            blade.origEntity = angular.copy(blade.currentEntity);
            blade.parentBlade.initialize();
            if (blade.newAsset) {
                blade.newAsset = false;
                bladeNavigationService.closeBlade(blade);
            }
            else {
                blade.chosenAssetId = blade.currentEntity.id;
                blade.title = blade.currentEntity.id;
                blade.subtitle = 'Edit ' + blade.currentEntity.name;
                blade.newAsset = false;
                blade.initializeBlade();
            }
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    };

    function deleteEntry() {
        var dialog = {
            id: "confirmDelete",
            title: "content.dialogs.asset-delete.title",
            message: "content.dialogs.asset-delete.message",
            callback: function (remove) {
                if (remove) {
                    blade.isLoading = true;

                    themes.deleteAsset({ storeId: blade.chosenStoreId, themeId: blade.chosenThemeId, assetIds: blade.chosenAssetId }, function () {
                        $scope.bladeClose();
                        blade.parentBlade.initialize(true);
                    },
                    function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
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
            if (endsWith(blade.chosenAssetId, ".json")) {
                return { name: "javascript", json: true };
            }
            else if (endsWith(blade.chosenAssetId, ".js")) {
                return "javascript";
            }
            else if (endsWith(blade.chosenAssetId, ".js.liquid")) {
                return "liquid-javascript";
            }
            else if (endsWith(blade.chosenAssetId, ".css.liquid")) {
                return "liquid-css";
            }
            else if (endsWith(blade.chosenAssetId, ".css")) {
                return "css";
            }
            else if (endsWith(blade.chosenAssetId, ".scss.liquid")) {
                return "liquid-css";
            }
            else if (endsWith(blade.chosenAssetId, ".liquid")) {
                return "liquid-html";
            }

        }
        return "htmlmixed";
    }

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded((isDirty() && !blade.newAsset) || (isCanSave() && blade.newAsset),
            true, blade, $scope.saveChanges, closeCallback, "content.dialogs.asset-save.title", "content.dialogs.asset-save.message");
    };

    blade.headIcon = 'fa-archive';

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
