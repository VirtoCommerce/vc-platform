angular.module('virtoCommerce.marketingModule')
.controller('promotionDetailController', ['$scope', 'bladeNavigationService', 'marketing_res_promotions', 'catalogs', 'stores', 'settings', 'dialogService', 'vaDynamicExpressionService', function ($scope, bladeNavigationService, marketing_res_promotions, catalogs, stores, settings, dialogService, vaDynamicExpressionService) {
    $scope.blade.refresh = function (parentRefresh) {
        if ($scope.blade.isNew) {
            marketing_res_promotions.getNew({}, function (data) {
                initializeBlade(data);
            });
        } else {
            marketing_res_promotions.get({ id: $scope.blade.currentEntityId }, function (data) {
                initializeBlade(data);
                if (parentRefresh) {
                    $scope.blade.parentBlade.refresh();
                }
            });
        }
    };

    function initializeBlade(data) {
        if (!$scope.blade.isNew) {
            $scope.blade.title = data.name;
        }

        if (data.dynamicExpression) {
            _.each(data.dynamicExpression.children, extendElementBlock);
        }

        $scope.blade.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
    };

    $scope.cancelChanges = function () {
        //angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
        $scope.bladeClose();
    };

    $scope.saveChanges = function () {
        bladeNavigationService.setError(null, $scope.blade);
        $scope.blade.isLoading = true;

        _.each($scope.blade.currentEntity.dynamicExpression.children, stripOffUiInformation);

        if ($scope.blade.isNew) {
            marketing_res_promotions.save({}, $scope.blade.currentEntity, function (data) {
                $scope.blade.isNew = undefined;
                $scope.blade.currentEntityId = data.id;
                initializeToolbar();
                $scope.blade.refresh(true);
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, $scope.blade);
            });
        } else {
            marketing_res_promotions.update({}, $scope.blade.currentEntity, function (data) {
                $scope.blade.refresh(true);
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, $scope.blade);
            });
        }
    };

    function stripOffUiInformation(expressionElement) {
        expressionElement.availableChildren = undefined;
        expressionElement.displayName = undefined;
        expressionElement.getValidationError = undefined;
        expressionElement.newChildLabel = undefined;
        expressionElement.templateURL = undefined;

        var selectedCategories = _.where(expressionElement.children, { id: 'ExcludingCategoryCondition' });
        expressionElement.excludingCategoryIds = _.pluck(selectedCategories, 'selectedCategoryId');
        expressionElement.children = _.difference(expressionElement.children, selectedCategories);

        var selectedProducts = _.where(expressionElement.children, { id: 'ExcludingProductCondition' });
        expressionElement.excludingProductIds = _.pluck(selectedProducts, 'productId');
        expressionElement.children = _.difference(expressionElement.children, selectedProducts);

        _.each(expressionElement.children, stripOffUiInformation);
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        if (isDirty() && !$scope.blade.isNew) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "Save changes",
                message: "The promotion has been modified. Do you want to save changes?"
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

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.bladeHeadIco = 'fa-flag';

    function initializeToolbar() {
        if (!$scope.blade.isNew) {
            $scope.bladeToolbarCommands = [
                {
                    name: "Save",
                    icon: 'fa fa-save',
                    executeMethod: function () {
                        $scope.saveChanges();
                    },
                    canExecuteMethod: function () {
                        return isDirty() && $scope.formScope && $scope.formScope.$valid;
                    }
                },
                {
                    name: "Reset",
                    icon: 'fa fa-undo',
                    executeMethod: function () {
                        angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
                    },
                    canExecuteMethod: function () {
                        return isDirty();
                    }
                }
            ];
        }
    }

    // datepicker
    $scope.datepickers = {
        str: false,
        end: false
    }

    $scope.open = function ($event, which) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.datepickers[which] = true;
    };

    $scope.dateOptions = {
        'year-format': "'yyyy'",
    };

    // $scope.formats = ['shortDate', 'dd-MMMM-yyyy', 'yyyy/MM/dd'];
    $scope.format = 'shortDate';

    // Dynamic ExpressionBlock

    function extendElementBlock(expressionBlock) {
        var retVal = vaDynamicExpressionService.expressions[expressionBlock.id];
        if (!retVal) {
            retVal = { displayName: 'unknown element: ' + expressionBlock.id };
        }

        //angular.merge(expressionBlock, retVal);
        //angular.extend(expressionBlock, retVal);
        _.extend(expressionBlock, retVal);

        if (!expressionBlock.children) {
            expressionBlock.children = [];
        }
        _.each(expressionBlock.excludingCategoryIds, function (id) {
            expressionBlock.children.push({ id: 'ExcludingCategoryCondition', selectedCategoryId: id });
        });
        _.each(expressionBlock.excludingProductIds, function (id) {
            expressionBlock.children.push({ id: 'ExcludingProductCondition', productId: id });
        });

        _.each(expressionBlock.children, extendElementBlock);
        _.each(expressionBlock.availableChildren, extendElementBlock);
        return expressionBlock;
    };

    //$scope.$watch('blade.currentEntity.dynamicExpression.children', function (children) {
    //    $scope.blade.isExpresionValid = _.all(children, validateExpression);
    //});
    //function validateExpression(x) {
    //    return !x.getValidationError || !x.getValidationError();
    //}
    //$scope.blade.isExpresionValid = false;
    //$scope.promotionExpressionValidator = function (value) {
    //    var retVal = true;

    //    return retVal;
    //}


    initializeToolbar();
    $scope.blade.refresh(false);
    //$scope.catalogs = catalogs.getCatalogs();
    $scope.stores = stores.query();
    //$scope.exclusivities = settings.getValues({ id: 'VirtoCommerce.Marketing.Promotions.Exclusivities' }, function (data) {
    //    if ($scope.blade.isNew && data && data[0]) {
    //        $scope.blade.currentEntity.exclusivity = data[0];
    //    }
    //});
}]);