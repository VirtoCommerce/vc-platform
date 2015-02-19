angular.module('virtoCommerce.storeModule.blades')
.controller('storeDetailController', ['$scope', 'bladeNavigationService', 'stores', 'catalogs', 'dialogService', function ($scope, bladeNavigationService, stores, catalogs, dialogService) {
    $scope.blade.refresh = function (parentRefresh) {
        //Refresh only when has id
        if (angular.isDefined($scope.blade.currentEntityId)) {
            stores.get({ id: $scope.blade.currentEntityId }, function (data) {
                initializeBlade(data);
                if (parentRefresh) {
                    $scope.blade.parentBlade.refresh();
                }
            });
        }
        else {
            initializeBlade($scope.blade.currentEntity);
        }
    }

    function initializeBlade(data) {
        $scope.blade.currentEntityId = data.id;
        if (angular.isDefined($scope.blade.currentEntityId)) {
            $scope.blade.title = data.name;
        }

        $scope.blade.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.isLoading = false;
    };

    function isDirty() {
        var retVal = angular.isDefined($scope.blade.currentEntityId);
        if (retVal) {
            retVal = angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
        }
        return !retVal;
    };

    $scope.saveChanges = function () {
        $scope.blade.isLoading = true;
        if (angular.isDefined($scope.blade.currentEntityId)) {
            stores.update({}, $scope.blade.currentEntity, function (data) {
                $scope.blade.refresh(true);
            });
        }
        else {
            stores.save({}, $scope.blade.currentEntity, function (data) {
                $scope.blade.parentBlade.refresh();
                $scope.blade.parentBlade.openBlade(data);
            });
        }
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.blade.onClose = function (closeCallback) {
        if (isDirty() && angular.isDefined($scope.blade.currentEntityId)) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "Save changes",
                message: "The Store has been modified. Do you want to save changes?"
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

    if (angular.isDefined($scope.blade.currentEntityId)) {
        $scope.bladeToolbarCommands = [
            {
                name: "Save", icon: 'fa fa-save',
                executeMethod: function () {
                    $scope.saveChanges();
                },
                canExecuteMethod: function () {
                    return isDirty() && $scope.formScope && $scope.formScope.$valid;
                }
            },
            {
                name: "Reset", icon: 'fa fa-undo',
                executeMethod: function () {
                    angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
                },
                canExecuteMethod: function () {
                    return isDirty();
                }
            }
        ];
    }

    $scope.blade.refresh(false);
    catalogs.getCatalogs({}, function (results) {
        $scope.catalogs = results;
    });
    $scope.storeStates = [{ id: 'Open', name: 'Open' }, { id: 'Closed', name: 'Closed' }, { id: 'RestrictedAccess', name: 'Restricted Access' }];
}]);