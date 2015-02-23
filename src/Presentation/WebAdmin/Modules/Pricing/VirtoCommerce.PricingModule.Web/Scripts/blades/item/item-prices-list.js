angular.module('virtoCommerce.pricingModule.blades.item')
.controller('itemPricesListController', ['$scope', 'prices', 'bladeNavigationService', 'dialogService', function ($scope, prices, bladeNavigationService, dialogService) {
    $scope.blade.selectedAll = false;
    $scope.selectedItem = null;

    $scope.blade.refresh = function (parentRefresh) {
        if (parentRefresh) {
            $scope.blade.isLoading = true;
            $scope.blade.parentBlade.refresh().$promise.then(function (results) {
                var data = _.find(results, function (x) { return x.id === $scope.blade.data.id; });
                initializeBlade(data);
            });
        } else {
            initializeBlade($scope.blade.data);
        }
    }

    function initializeBlade(data) {
        $scope.blade.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.isLoading = false;
    };

    $scope.delete = function () {
        if (isItemsChecked()) {
            deleteChecked();
        } else {
            var dialog = {
                id: "notifyNoTargetCategory",
                title: "Message",
                message: "Nothing selected. Check some Prices first."
            };
            dialogService.showNotificationDialog(dialog);
        }
    };

    //function isItemsChecked() {
    //    return $scope.blade.currentEntity && _.any($scope.blade.currentEntity.prices, function (x) { return x.selected; });
    //}

    function deleteChecked() {
        // var selection = _.where($scope.blade.currentEntity.prices, { selected: true });
        var selection = [$scope.selectedItem];
        _.each(selection, function (item) {
            $scope.blade.currentEntity.prices.splice($scope.blade.currentEntity.prices.indexOf(item), 1);
        });
    }

    $scope.selectItem = function (listItem) {
        $scope.selectedItem = listItem;
    };

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();

        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "Save changes",
                message: "The Prices information has been modified. Do you want to save changes?"
            };
            dialog.callback = function (needSave) {
                if (needSave) {
                    saveChanges();
                }
                closeCallback();
            };
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeCallback();
        }
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    function isDirty() {
        return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
    };

    function saveChanges() {
        $scope.blade.isLoading = true;
        prices.update({ id: $scope.blade.itemId }, $scope.blade.currentEntity, function (data) {
            $scope.blade.refresh(true);
        });
    };

    $scope.bladeToolbarCommands = [
        {
            name: "Add", icon: 'fa fa-plus',
            executeMethod: function () {
                var newEntity = { productId: $scope.blade.itemId, list: 0, minQuantity: 1, currency: $scope.blade.data.currency };
                $scope.blade.currentEntity.prices.push(newEntity);
            },
            canExecuteMethod: function () {
                return true;
            }
        },
        {
            name: "Save", icon: 'fa fa-save',
            executeMethod: function () {
                saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty(); // && formScope && formScope.$valid;
            }
        },
        {
            name: "Reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
                $scope.blade.selectedAll = false;
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        },
        {
            name: "Delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                deleteChecked();
            },
            canExecuteMethod: function () {
                return $scope.selectedItem;
            }
        }
    ];

    $scope.checkAll = function (selected) {
        angular.forEach($scope.blade.currentEntity.prices, function (item) {
            item.selected = selected;
        });
    };

    $scope.blade.refresh();
}]);
