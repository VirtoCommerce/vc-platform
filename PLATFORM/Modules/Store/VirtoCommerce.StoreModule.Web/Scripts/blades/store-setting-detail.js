angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storeSettingDetailController', ['$scope', '$timeout', 'platformWebApp.dialogService', function ($scope, $timeout, dialogService) {
    var blade = $scope.blade;
    var codemirrorEditor;
    $scope.types = ["Boolean", "ShortText", "LongText", "Xml"];

    function initializeBlade(data) {
        if (data.isNew) {
            data.valueType = $scope.types[1];
        } else if (data.valueType == $scope.types[3]) {
            $timeout(function () {
                if (codemirrorEditor) {
                    codemirrorEditor.refresh();
                    codemirrorEditor.focus();
                }
                angular.copy(blade.currentEntity, blade.origEntity);
            }, 1);
        }

        blade.currentEntity = angular.copy(data);
        blade.origEntity = data;
        blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity);
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.saveChanges = function () {
        if (blade.currentEntity.isNew) {
            blade.currentEntity.isNew = undefined;
            blade.parentBlade.currentEntities.push(blade.currentEntity);
        }

        angular.copy(blade.currentEntity, blade.origEntity);
        $scope.bladeClose();
    };

    $scope.isValid = function () {
        return $scope.formScope && $scope.formScope.$valid;
    }

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "Save changes",
                message: "The Setting has been modified. Do you want to save changes?",
                callback: function (needSave) {
                    if (needSave) {
                        saveChanges();
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

    function deleteEntry() {
        var dialog = {
            id: "confirmDelete",
            title: "Delete confirmation",
            message: "Are you sure you want to delete this Setting?",
            callback: function (remove) {
                if (remove) {
                    var idx = blade.parentBlade.currentEntities.indexOf(blade.origEntity);
                    if (idx >= 0) {
                        blade.parentBlade.currentEntities.splice(idx, 1);
                        $scope.bladeClose();
                    }
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    $scope.blade.headIcon = 'fa fa-archive';

    $scope.blade.toolbarCommands = [
        {
            name: "Reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(blade.origEntity, blade.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            },
            permission: 'store:manage'
        },
        {
            name: "Delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                deleteEntry();
            },
            canExecuteMethod: function () {
                return !blade.currentEntity.isNew && !isDirty();
            },
            permission: 'store:manage'
        }
    ];

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
        mode: 'xml'
    };

    // on load
    initializeBlade(blade.origEntity);
}]);