angular.module('virtoCommerce.pricingModule')
.controller('virtoCommerce.pricingModule.pricesListController', ['$scope', 'virtoCommerce.pricingModule.prices', 'platformWebApp.objCompareService', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, prices, objCompareService, bladeNavigationService, dialogService) {
    $scope.blade.selectedAll = false;
    $scope.selectedItem = null;

    $scope.blade.refresh = function (parentRefresh) {
        if ($scope.blade.isApiSave) {
            if (parentRefresh) {
                $scope.blade.isLoading = true;
                $scope.blade.parentBlade.refresh().then(function (results) {
                    $scope.blade.data = _.find(results, function (x) { return x.id === $scope.blade.data.id; });
                    if ($scope.blade.data.productPrices.length == 0) {
                        $scope.blade.data.productPrices.push({ prices: [], productId: $scope.blade.itemId });
                    }
                    initializeBlade($scope.blade.data.productPrices[0].prices);
                });
            } else {
                if ($scope.blade.data.productPrices.length == 0) {
                    $scope.blade.data.productPrices.push({ prices: [], productId: $scope.blade.itemId });
                }
                initializeBlade($scope.blade.data.productPrices[0].prices);
            }
        } else {
            initializeBlade($scope.blade.data.prices);
        }
    }

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
        var retVal = false;
        if ($scope.blade.currentEntities) {
            retVal = !objCompareService.equal($scope.blade.origEntity, $scope.blade.currentEntities);
        }
        return retVal;
    };

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    $scope.isValid = function () {
        return formScope && formScope.$valid &&
             _.all($scope.blade.currentEntities, $scope.isListPriceValid) &&
             _.all($scope.blade.currentEntities, $scope.isUniqueQty) &&
            ($scope.blade.currentEntities.length == 0 || _.some($scope.blade.currentEntities, function (x) { return x.minQuantity == 1; }));
    }

    $scope.saveChanges = function () {
        if ($scope.blade.isApiSave) {
            $scope.blade.isLoading = true;

            angular.copy($scope.blade.currentEntities, $scope.blade.data.productPrices[0].prices);
            prices.update({ id: $scope.blade.itemId }, $scope.blade.data, function (data) {
                $scope.blade.refresh(true);
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
        } else {
            angular.copy($scope.blade.currentEntities, $scope.blade.data.prices);
            angular.copy($scope.blade.currentEntities, $scope.blade.origEntity);
            $scope.bladeClose();
        }
    };

    $scope.delete = function (listItem) {
        if (listItem) {
            $scope.blade.currentEntities.splice($scope.blade.currentEntities.indexOf(listItem), 1);
            $scope.selectItem(null);
        }
    };

    var formScope;
    $scope.setForm = function (form) {
        formScope = form;
    }

    $scope.blade.headIcon = 'fa-usd';

    $scope.blade.toolbarCommands = [
        {
            name: "Add", icon: 'fa fa-plus',
            executeMethod: function () {
                var newEntity = { productId: $scope.blade.itemId, list: 0, minQuantity: 1, currency: $scope.blade.currency };
                $scope.blade.currentEntities.push(newEntity);
                $scope.selectItem(newEntity);
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'pricing:manage'
        },
        {
            name: "Save", icon: 'fa fa-save',
            executeMethod: function () {
                $scope.saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty() && $scope.isValid();
            },
            permission: 'pricing:manage'
        },
        {
            name: "Reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntities);
                $scope.blade.selectedAll = false;
            },
            canExecuteMethod: function () {
                return isDirty();
            },
            permission: 'pricing:manage'
        },
        {
            name: "Delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                var selection = _.where($scope.blade.currentEntities, { _selected: true });
                angular.forEach(selection, function (listItem) {
                    $scope.blade.currentEntities.splice($scope.blade.currentEntities.indexOf(listItem), 1);
                });
            },
            canExecuteMethod: function () {
                return _.some($scope.blade.currentEntities, function (x) { return x._selected; });
            },
            permission: 'pricing:manage'
        }
    ];

    if (!$scope.blade.isApiSave) {
        $scope.blade.toolbarCommands.splice(1, 1); // remove save button
    }

    $scope.toggleAll = function () {
        angular.forEach($scope.blade.currentEntities, function (item) {
            item._selected = $scope.blade.selectedAll;
        });
    };

    $scope.isListPriceValid = function (data) {
        return data.list > 0;
    }

    $scope.isUniqueQty = function (data) {
        var foundPrices = _.where($scope.blade.currentEntities, { minQuantity: data.minQuantity });
        return foundPrices.length < 2;
    }

    // actions on load
    $scope.blade.refresh();
}]);
