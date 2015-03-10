angular.module('virtoCommerce.coreModule.fulfillment.blades')
.controller('fulfillmentCenterDetailController', ['$scope', 'dialogService', 'bladeNavigationService', 'fulfillments', function ($scope, dialogService, bladeNavigationService, fulfillments) {

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

        fulfillments.get({ _id: $scope.blade.currentEntityId }, function (data) {
            initializeBlade(data);
        });
    }

    function initializeBlade(data) {
        $scope.blade.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.isLoading = false;
    };

    var formScope;
    $scope.setForm = function (form) {
        formScope = form;
    }

    function isDirty() {
        return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
    };

    function saveChanges() {
        $scope.blade.isLoading = true;
        
        fulfillments.update($scope.blade.currentEntity, function (data, headers) {
            $scope.blade.refresh();
        });
    };

    $scope.bladeHeadIco = 'fa fa-wrench';

    $scope.bladeToolbarCommands = [
        {
            name: "Save", icon: 'fa fa-save',
            executeMethod: function () {
                saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty() && formScope && formScope.$valid;
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

    $scope.blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "Save changes",
                message: "The fulfillments has been modified. Do you want to save changes?",
                callback: function (needSave) {
                    if (needSave) {
                        saveChanges();
                    }
                    closeCallback();
                }
            };
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeCallback();
        }
    };

    // actions on load
    $scope.blade.refresh();
}]);
