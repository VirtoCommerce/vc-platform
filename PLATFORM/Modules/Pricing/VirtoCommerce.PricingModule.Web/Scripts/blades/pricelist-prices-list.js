angular.module('virtoCommerce.pricingModule')
.controller('pricelistPricesListController', ['$scope', 'bladeNavigationService', 'dialogService', function ($scope, bladeNavigationService, dialogService) {
    $scope.blade.selectedAll = false;
    $scope.selectedItem = null;

    function initializeBlade(data) {
        $scope.blade.currentEntities = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.isLoading = false;
    };

    $scope.selectItem = function (listItem) {
        $scope.selectedItem = listItem;
    };

    $scope.blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "Save changes",
                message: "The Prices has been modified. Do you want to save changes?"
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

    function isDirty() {
        return !angular.equals($scope.blade.currentEntities, $scope.blade.origEntity);
    };

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    $scope.saveChanges = function () {
        // angular.copy($scope.blade.currentEntities, $scope.blade.data);
        angular.copy($scope.blade.currentEntities, $scope.blade.origEntity);
        $scope.bladeClose();
    };

    $scope.bladeHeadIco = 'fa-usd';

    $scope.bladeToolbarCommands = [
        {
            name: "Add", icon: 'fa fa-plus',
            executeMethod: function () {
                // , currency: $scope.blade.data.currency
                var newEntity = { productId: $scope.blade.data.productId, list: 1, minQuantity: 1 };
                $scope.blade.currentEntities.push(newEntity);
                $scope.selectItem(newEntity);
            },
            canExecuteMethod: function () {
                return true;
            }
        },
        {
            name: "Reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntities);
                $scope.blade.selectedAll = false;
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        },
        {
            name: "Delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                var selection = _.where($scope.blade.currentEntities, { selected: true });
                angular.forEach(selection, function (listItem) {
                    $scope.blade.currentEntities.splice($scope.blade.currentEntities.indexOf(listItem), 1);
                });
            },
            canExecuteMethod: function () {
                return _.some($scope.blade.currentEntities, function (x) { return x.selected; });
            }
        }
    ];

    $scope.delete = function (listItem) {
        if (listItem) {
            $scope.blade.currentEntities.splice($scope.blade.currentEntities.indexOf(listItem), 1);
            $scope.selectItem(null);
        }
    };

    $scope.toggleAll = function () {
        angular.forEach($scope.blade.currentEntities, function (item) {
            item.selected = $scope.blade.selectedAll;
        });
    };
    
    // actions on load
    initializeBlade($scope.blade.data.prices);
}]);
