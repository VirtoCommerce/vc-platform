angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.promotionConditionCurrencyIsController', ['$scope', 'virtoCommerce.coreModule.currency.currencyUtils', function ($scope, currencyUtils) {
    $scope.currencyUtils = currencyUtils;
}])

.controller('virtoCommerce.marketingModule.promotionExpressionsController', ['$scope', 'platformWebApp.authService', 'platformWebApp.bladeNavigationService', function ($scope, authService, bladeNavigationService) {

    $scope.openItemSelectWizard = function (parentElement) {
        if (!authService.checkPermission('marketing:update')) {
            return;
        }

        var selectedListEntries = [];
        var newBlade = {
            id: "CatalogEntrySelect",
            title: "marketing.blades.catalog-items-select.title-product",
            controller: 'virtoCommerce.catalogModule.catalogItemSelectController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/common/catalog-items-select.tpl.html',
            breadcrumbs: [],
            toolbarCommands: [
            {
                name: "platform.commands.pick-selected", icon: 'fa fa-plus',
                executeMethod: function (blade) {
                    //parentElement.selectedListEntry = selectedListEntries[0];
                    parentElement.productId = selectedListEntries[0].id;
                    parentElement.productName = selectedListEntries[0].name;
                    bladeNavigationService.closeBlade(blade);
                },
                canExecuteMethod: function () {
                    return selectedListEntries.length == 1;
                }
            }]
        };

        newBlade.options = {
            showCheckingMultiple: false,
            checkItemFn: function (listItem, isSelected) {
                if (listItem.type == 'category') {
                    newBlade.error = 'Must select Product';
                    listItem.selected = undefined;
                } else {
                    if (isSelected) {
                        if (_.all(selectedListEntries, function (x) { return x.id != listItem.id; })) {
                            selectedListEntries.push(listItem);
                        }
                    }
                    else {
                        selectedListEntries = _.reject(selectedListEntries, function (x) { return x.id == listItem.id; });
                    }
                    newBlade.error = undefined;
                }
            }
        };

        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    $scope.openCategorySelectWizard = function (parentElement) {
        if (!authService.checkPermission('marketing:update')) {
            return;
        }

        var selectedListEntries = [];
        var newBlade = {
            id: "CatalogCategorySelect",
            title: "marketing.blades.catalog-items-select.title-category",
            controller: 'virtoCommerce.catalogModule.catalogItemSelectController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/common/catalog-items-select.tpl.html',
            breadcrumbs: [],
            toolbarCommands: [
            {
                name: "platform.commands.pick-selected", icon: 'fa fa-plus',
                executeMethod: function (blade) {
                    parentElement.categoryId = selectedListEntries[0].id;
                    parentElement.categoryName = selectedListEntries[0].name;
                    bladeNavigationService.closeBlade(blade);
                },
                canExecuteMethod: function () {
                    return selectedListEntries.length == 1;
                }
            }]
        };

        newBlade.options = {
            showCheckingMultiple: false,
            allowCheckingItem: false,
            allowCheckingCategory: true,
            checkItemFn: function (listItem, isSelected) {
                if (listItem.type != 'category') {
                    newBlade.error = 'Must select Category';
                    listItem.selected = undefined;
                } else {
                    if (isSelected) {
                        if (_.all(selectedListEntries, function (x) { return x.id != listItem.id; })) {
                            selectedListEntries.push(listItem);
                        }
                    }
                    else {
                        selectedListEntries = _.reject(selectedListEntries, function (x) { return x.id == listItem.id; });
                    }
                    newBlade.error = undefined;
                }
            }
        };

        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    $scope.shippingMethods = [{ id: '1a', name: 'method1' }, { id: 'ab2', name: 'method 2' }];
}]);