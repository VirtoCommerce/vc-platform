angular.module('virtoCommerce.coreModule.fulfillment')
.controller('fulfillmentCenterDetailController', ['$scope', 'dialogService', 'bladeNavigationService', 'fulfillments', function ($scope, dialogService, bladeNavigationService, fulfillments) {

    $scope.blade.refresh = function (parentRefresh) {
        if ($scope.blade.currentEntityId) {
            $scope.blade.isLoading = true;

            fulfillments.get({ _id: $scope.blade.currentEntityId }, function (data) {
                initializeBlade(data);
                if (parentRefresh) {
                    $scope.blade.parentBlade.refresh();
                }
            });
        } else {
            initializeBlade($scope.blade.currentEntity);
        }
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
                
        if ($scope.blade.currentEntityId) {
            fulfillments.update($scope.blade.currentEntity, function () {
                $scope.blade.refresh(true);
            });
        } else {
            fulfillments.save({}, $scope.blade.currentEntity, function (data) {
                $scope.blade.title = data.displayName;
                $scope.blade.currentEntityId = data.id;
                initializeBlade(data);
                $scope.blade.parentBlade.refresh();
            });
        }
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
