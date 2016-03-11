angular.module('virtoCommerce.pricingModule')
.controller('virtoCommerce.pricingModule.pricesListController', ['$scope', 'virtoCommerce.pricingModule.prices', 'platformWebApp.objCompareService', 'platformWebApp.bladeNavigationService', function ($scope, prices, objCompareService, bladeNavigationService) {
    var blade = $scope.blade;
    blade.updatePermission = 'pricing:update';
    blade.selectedAll = false;

    blade.refresh = function (parentRefresh) {
        if (blade.isApiSave) {
            if (parentRefresh) {
                blade.isLoading = true;
                blade.parentBlade.refresh().then(function (results) {
                    blade.data = _.find(results, function (x) { return x.id === blade.data.id; });
                    if (blade.data.productPrices.length == 0) {
                        blade.data.productPrices.push({ prices: [], productId: blade.itemId });
                    }
                    initializeBlade(blade.data.productPrices[0].prices);
                });
            } else {
                if (blade.data.productPrices.length == 0) {
                    blade.data.productPrices.push({ prices: [], productId: blade.itemId });
                }
                initializeBlade(blade.data.productPrices[0].prices);
            }
        } else {
            initializeBlade(blade.data.prices);
        }
    };

    function initializeBlade(data) {
        blade.currentEntities = angular.copy(data);
        blade.origEntity = data;
        blade.isLoading = false;
    }

    $scope.selectItem = function (listItem) {
        $scope.selectedItemId = listItem.id;
    };

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, $scope.saveChanges, closeCallback, "pricing.dialogs.prices-save.title", "pricing.dialogs.prices-save.message");
    };

    function isDirty() {
        return blade.currentEntities && !objCompareService.equal(blade.origEntity, blade.currentEntities) && blade.hasUpdatePermission()
    }

    function canSave() {
        return isDirty() && $scope.isValid();
    }

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    };

    $scope.isValid = function () {
        return formScope && formScope.$valid &&
             _.all(blade.currentEntities, $scope.isListPriceValid) &&
             _.all(blade.currentEntities, $scope.isUniqueQty) &&
            (blade.currentEntities.length == 0 || _.some(blade.currentEntities, function (x) { return x.minQuantity == 1; }));
    }

    $scope.saveChanges = function () {
        if (blade.isApiSave) {
            blade.isLoading = true;

            angular.copy(blade.currentEntities, blade.data.productPrices[0].prices);
            prices.update({ id: blade.itemId }, blade.data, function (data) {
                blade.refresh(true);
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
        } else {
            angular.copy(blade.currentEntities, blade.data.prices);
            angular.copy(blade.currentEntities, blade.origEntity);
            $scope.bladeClose();
        }
    };

    $scope.delete = function (listItem) {
        if (listItem) {
            blade.currentEntities.splice(blade.currentEntities.indexOf(listItem), 1);
            $scope.selectItem(null);
        }
    };

    var formScope;
    $scope.setForm = function (form) {
        formScope = form;
    }

    blade.headIcon = 'fa-usd';

    blade.toolbarCommands = [
        {
            name: "platform.commands.add", icon: 'fa fa-plus',
            executeMethod: function () {
                var newEntity = { productId: blade.itemId, list: 0, minQuantity: 1, currency: blade.currency };
                blade.currentEntities.push(newEntity);
                $scope.selectItem(newEntity);
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: blade.updatePermission
        },
        {
            name: "platform.commands.save", icon: 'fa fa-save',
            executeMethod: $scope.saveChanges,
            canExecuteMethod: canSave,
            permission: blade.updatePermission
        },
        {
            name: "platform.commands.reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(blade.origEntity, blade.currentEntities);
                blade.selectedAll = false;
            },
            canExecuteMethod: isDirty,
            permission: blade.updatePermission
        },
        {
            name: "platform.commands.delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                var selection = _.where(blade.currentEntities, { _selected: true });
                angular.forEach(selection, function (listItem) {
                    blade.currentEntities.splice(blade.currentEntities.indexOf(listItem), 1);
                });
            },
            canExecuteMethod: function () {
                return _.some(blade.currentEntities, function (x) { return x._selected; });
            },
            permission: blade.updatePermission
        }
    ];

    if (!blade.isApiSave) {
        blade.toolbarCommands.splice(1, 1); // remove save button
    }

    $scope.toggleAll = function () {
        angular.forEach(blade.currentEntities, function (item) {
            item._selected = blade.selectedAll;
        });
    };

    $scope.isListPriceValid = function (data) {
        return data.list > 0;
    }

    $scope.isUniqueQty = function (data) {
        return Math.round(data.minQuantity) > 0 && _.all(blade.currentEntities, function (x) { return x === data || Math.round(x.minQuantity) !== Math.round(data.minQuantity) });
    }

    // actions on load
    blade.refresh();
}]);
