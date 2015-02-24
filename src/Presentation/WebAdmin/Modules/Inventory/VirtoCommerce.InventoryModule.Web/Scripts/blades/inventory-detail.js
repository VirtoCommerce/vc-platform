angular.module('virtoCommerce.inventoryModule.blades')
.controller('inventoryDetailController', ['$scope', 'dialogService', 'inventories', function ($scope, dialogService, inventories) {

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;
        $scope.blade.parentBlade.refresh().then(function (results) {
            var data = _.findWhere(results, { fulfillmentCenterId: $scope.blade.data.fulfillmentCenterId });
            
            // parse date fields
            if (data.preorderAvailabilityDate) {
                data.preorderAvailabilityDate = new Date(data.preorderAvailabilityDate);
            }
            if (data.backorderAvailabilityDate) {
                data.backorderAvailabilityDate = new Date(data.backorderAvailabilityDate);
            }

            initializeBlade(data);
        });
    }

    function initializeBlade(data) {
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
        $scope.blade.isLoading = true;
        inventories.update({ id: $scope.blade.itemId }, $scope.blade.currentEntity, function () {
            $scope.blade.refresh();
        });
    };

    $scope.blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "Save changes",
                message: "Inventory has been modified. Do you want to save changes?",
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

    $scope.bladeToolbarCommands = [
        {
            name: "Save", icon: 'fa fa-save',
            executeMethod: function () {
                $scope.saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty(); // && formScope && formScope.$valid;
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

    // on load
    initializeBlade($scope.blade.data);
}]);