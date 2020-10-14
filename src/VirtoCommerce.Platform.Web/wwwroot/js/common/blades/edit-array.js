angular.module('platformWebApp').controller('platformWebApp.editArrayController', ['$scope', 'platformWebApp.dialogService', function ($scope, dialogService) {
    var blade = $scope.blade;
    blade.isLoading = false;

    function refresh() {
        blade.selectedAll = false;
        initializeBlade(blade.data);
    }

    function initializeBlade(data) {
        var items = data.map(x => { return { value: x } });
        blade.origEntity = items;
        blade.currentEntities = angular.copy(items);
        resetNewValue();
    }

    function resetNewValue() {
        $scope.newItem = {};
    }

    $scope.add = function (form) {
        if (form.$valid) {
            blade.currentEntities.push($scope.newItem);
            resetNewValue();
            form.$setPristine();
        }
    };

    $scope.delete = function (index) {
        blade.selectedAll = false;
        $scope.toggleAll();
        blade.currentEntities[index].$selected = true;
        deleteChecked();
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    function isDirty() {
        return !angular.equals(blade.currentEntities, blade.origEntity) && blade.hasUpdatePermission();
    }

    $scope.saveChanges = function () {
        var items = blade.currentEntities.map(x => x.value);
        blade.onChangesConfirmedFn(items);
        $scope.bladeClose();
    };

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    blade.toolbarCommands = [{
        name: "platform.commands.reset", icon: 'fa fa-undo',
        executeMethod: function () {
            angular.copy(blade.origEntity, blade.currentEntities);
        },
        canExecuteMethod: isDirty,
        permission: blade.updatePermission
    }, {
        name: "platform.commands.delete", icon: 'fa fa-trash-o',
        executeMethod: function () {
            deleteChecked();
        },
        canExecuteMethod: function () {
            return isItemsChecked();
        },
        permission: blade.updatePermission
    }];

    $scope.toggleAll = function () {
        blade.currentEntities.forEach(x => x.$selected = blade.selectedAll);
    };

    function isItemsChecked() {
        return blade.currentEntities.some(x => x.$selected);
    }

    function deleteChecked() {
        var itemsToDelete = blade.currentEntities.filter(x => x.$selected);
        var dialog = {
            id: "confirmDeleteItem",
            title: "platform.dialogs.dictionary-items-delete.title",
            message: "platform.dialogs.dictionary-items-delete.message",
            messageValues: { quantity: itemsToDelete.length },
            callback: function (remove) {
                if (remove) {
                    blade.currentEntities = blade.currentEntities.filter(x => !x.$selected);
                    blade.selectedAll = false;
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    // on load
    refresh();
}]);
