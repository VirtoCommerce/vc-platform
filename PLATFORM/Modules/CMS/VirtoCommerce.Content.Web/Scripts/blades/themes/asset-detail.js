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
            else {
                // storeId: blade.storeId, themeId: blade.themeId,
                $http.get(blade.currentEntity.url, {
                    responseType: 'text',
                    //cache: false,
                    //contentType: false,
                    //processData: false,
                }).then(function (results) {
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

        function isCanSave() {
            return blade.currentEntity && blade.currentEntity.name && blade.currentEntity.content;
        }

        function isDirty() {
            return !angular.equals(blade.currentEntity, blade.origEntity)
                && isCanSave()
                && (blade.isNew || blade.hasUpdatePermission());
        };

        $scope.saveChanges = function () {
            blade.isLoading = true;

            var fd = new FormData();
            fd.append(blade.currentEntity.name, blade.currentEntity.content);

            $http.post('api/content/' + blade.contentType + '/' + blade.storeId + '?folderUrl=' + blade.folderUrl, fd,
                {
                    transformRequest: angular.identity,
                    headers: { 'Content-Type': undefined }
                }).then(function (results) {
                    blade.parentBlade.refresh();
                    if (blade.isNew) {
                        blade.isNew = false;
                        blade.subtitle = 'Edit ' + blade.currentEntity.name;
                        blade.currentEntity.url = results.data[0].url;
                    }
                    blade.initializeBlade();

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

        blade.onClose = function (closeCallback) {
            bladeNavigationService.showConfirmationIfNeeded((isDirty() && !blade.isNew) || (isCanSave() && blade.isNew),
                true, blade, $scope.saveChanges, closeCallback, "content.dialogs.asset-save.title", "content.dialogs.asset-save.message");
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
