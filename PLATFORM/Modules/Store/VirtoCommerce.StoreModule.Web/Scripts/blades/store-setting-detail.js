angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storeSettingDetailController', ['$scope', 'platformWebApp.dialogService', function ($scope, dialogService) {
    $scope.types = ["Boolean", "ShortText", "Xml"];

    function initializeBlade(data) {
        if (data.isNew) {
            data.valueType = $scope.types[1];
        }

        $scope.blade.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.saveChanges = function () {
        if ($scope.blade.currentEntity.isNew) {
            $scope.blade.currentEntity.isNew = undefined;
            $scope.blade.parentBlade.currentEntities.push($scope.blade.currentEntity);
        }

        angular.copy($scope.blade.currentEntity, $scope.blade.origEntity);
        $scope.bladeClose();
    };
    
    $scope.isValid = function () {
        return $scope.formScope && $scope.formScope.$valid;
    }

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    $scope.blade.onClose = function (closeCallback) {
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
                    var idx = $scope.blade.parentBlade.currentEntities.indexOf($scope.blade.origEntity);
                    if (idx >= 0) {
                        $scope.blade.parentBlade.currentEntities.splice(idx, 1);
                        $scope.bladeClose();
                    }
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    $scope.bladeHeadIco = 'fa fa-archive';

    $scope.bladeToolbarCommands = [
        {
            name: "Reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
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
                return !$scope.blade.currentEntity.isNew && !isDirty();
            },
            permission: 'store:manage'
        }
    ];

    // on load
    initializeBlade($scope.blade.origEntity);
}]);