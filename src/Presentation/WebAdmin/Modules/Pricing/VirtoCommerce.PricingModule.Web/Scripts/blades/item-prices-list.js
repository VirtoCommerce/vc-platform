angular.module('virtoCommerce.pricingModule.blades.itemPricesList', [
    'virtoCommerce.pricingModule.resources.pricing',
    'platformWebApp.common.confirmDialog'
])
.controller('itemPricesListController', ['$scope', 'prices', 'bladeNavigationService', 'dialogService', function ($scope, prices, bladeNavigationService, dialogService) {
    $scope.blade.selectedAll = false;
    $scope.selectedItem = null;

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;
        prices.query({ id: $scope.blade.itemId }, function (data) {
            $scope.blade.isLoading = false;
            $scope.blade.entities = angular.copy(data);
            $scope.blade.origItem = data;
            $scope.blade.selectedAll = false;
        }, function (error) {
            $scope.blade.isLoading = false;
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    }

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

    function isItemsChecked() {
        return $scope.blade.entities && _.any($scope.blade.entities, function (x) { return x.selected; });
    }

    function deleteChecked() {
        var selection = _.where($scope.blade.entities, { selected: true });
        _.each(selection, function (item) {
            $scope.blade.entities.splice($scope.blade.entities.indexOf(item), 1);
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
        return !angular.equals($scope.blade.entities, $scope.blade.origItem);
    };

    function saveChanges() {
        $scope.blade.isLoading = true;
        prices.update({ id: $scope.blade.itemId }, $scope.blade.entities, function (data) {
            $scope.blade.refresh(true);
        });
    };

    $scope.bladeToolbarCommands = [
        {
            name: "Refresh", icon: 'fa fa-refresh',
            executeMethod: function () {
                $scope.blade.refresh();
            },
            canExecuteMethod: function () {
                return !isDirty();
            }
        },
        //{
        //    name: "Add", icon: 'fa fa-plus',
        //    executeMethod: function () {
        //        var newEntity = { productId: $scope.blade.itemId, list: 0, minQuantity: 1 };
        //        $scope.blade.entities.push(newEntity);
        //    },
        //    canExecuteMethod: function () {
        //        return true;
        //    }
        //},
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
                angular.copy($scope.blade.origItem, $scope.blade.entities);
                $scope.blade.selectedAll = false;
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        }
        //{
        //    name: "Delete", icon: 'fa fa-trash-o',
        //    executeMethod: function () {
        //        deleteChecked();
        //    },
        //    canExecuteMethod: function () {
        //        return isItemsChecked();
        //    }
        //}
    ];

    $scope.checkAll = function (selected) {
        angular.forEach($scope.blade.entities, function (item) {
            item.selected = selected;
        });
    };

    $scope.blade.refresh();
}]);
