angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.assetDetailController', ['$scope', 'platformWebApp.validators', 'platformWebApp.dialogService', 'virtoCommerce.contentModule.contentApi', '$timeout', 'platformWebApp.bladeNavigationService', '$http',
    function ($scope, validators, dialogService, contentApi, $timeout, bladeNavigationService, $http) {
        var blade = $scope.blade;
        blade.updatePermission = 'content:update';
        var codemirrorEditor;

        $scope.validators = validators;
        var formScope;
        $scope.setForm = function (form) { formScope = form; }

        blade.initializeBlade = function () {
            if (blade.isNew) {
                blade.isLoading = false;
            }
            else {
                contentApi.get({
                    contentType: blade.contentType,
                    storeId: blade.storeId,
                    relativeUrl: blade.currentEntity.relativeUrl
                }, function (results) {
                    blade.isLoading = false;
                    blade.currentEntity.content = results.data;
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
                    //},
                    //{
                    //    name: "platform.commands.delete", icon: 'fa fa-trash-o',
                    //    executeMethod: deleteEntry,
                    //    canExecuteMethod: function () {
                    //        return true;
                    //    },
                    //    permission: 'content:delete'
                }];
            }
        };

        function isDirty() {
            return !angular.equals(blade.currentEntity, blade.origEntity)
                && (blade.isNew || blade.hasUpdatePermission());
        };

        $scope.saveChanges = function () {
            blade.isLoading = true;

            contentApi.save({
                contentType: blade.contentType,
                storeId: blade.storeId,
                folderUrl: blade.folderUrl
            }, blade.currentEntity, function () {
                blade.isLoading = false;
                blade.origEntity = angular.copy(blade.currentEntity);
                if (blade.isNew) {
                    $scope.bladeClose();
                }

                blade.parentBlade.refresh();
            }, function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        };

        function deleteEntry() {
            var dialog = {
                id: "confirmDelete",
                title: "content.dialogs.asset-delete.title",
                message: "content.dialogs.asset-delete.message",
                callback: function (remove) {
                    if (remove) {
                        blade.isLoading = true;

                        contentApi.delete({ contentType: blade.contentType, storeId: blade.storeId, themeId: blade.themeId, assetIds: blade.currentEntity.url }, function () {
                            $scope.bladeClose();
                            blade.parentBlade.initialize(true);
                        },
                        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                    }
                }
            }
            dialogService.showConfirmationDialog(dialog);
        }

        function getEditorMode() {
            // mode: { name: "htmlmixed" } // html
            // mode: { name: "javascript" } // javascript
            // mode: { name: "javascript", json: true } // json
            // mode: "liquid-html" // liquid html
            // mode: "liquid-css" // liquid css
            // mode: "liquid-javascript" // liquid css

            if (!blade.isNew) {
                if (blade.currentEntity.url.endsWith(".json")) {
                    return { name: "javascript", json: true };
                }
                else if (blade.currentEntity.url.endsWith(".js")) {
                    return "javascript";
                }
                else if (blade.currentEntity.url.endsWith(".js.liquid")) {
                    return "liquid-javascript";
                }
                else if (blade.currentEntity.url.endsWith(".css.liquid")) {
                    return "liquid-css";
                }
                else if (blade.currentEntity.url.endsWith(".css")) {
                    return "css";
                }
                else if (blade.currentEntity.url.endsWith(".scss.liquid")) {
                    return "liquid-css";
                }
                else if (blade.currentEntity.url.endsWith(".liquid")) {
                    return "liquid-html";
                }
            }
            return "htmlmixed";
        }

        function canSave() {
            return blade.currentEntity && blade.currentEntity.name && ((isDirty() && !blade.isNew) || (blade.currentEntity.content && blade.isNew));
        }

        blade.onClose = function (closeCallback) {
            bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, $scope.saveChanges, closeCallback, "content.dialogs.asset-save.title", "content.dialogs.asset-save.message");
        };

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

        blade.headIcon = 'fa-file-o';
        blade.initializeBlade();
    }]);
