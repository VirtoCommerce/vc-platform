﻿angular.module('virtoCommerce.pricingModule')
.controller('virtoCommerce.pricingModule.assignmentDetailController', ['$scope', 'virtoCommerce.catalogModule.catalogs', 'virtoCommerce.pricingModule.pricelists', 'virtoCommerce.pricingModule.pricelistAssignments', 'platformWebApp.dialogService', 'platformWebApp.bladeNavigationService', 'virtoCommerce.coreModule.common.dynamicExpressionService', function ($scope, catalogs, pricelists, assignments, dialogService, bladeNavigationService, dynamicExpressionService) {
    var blade = $scope.blade;

    blade.refresh = function (parentRefresh) {
        if (blade.isNew) {
            assignments.getNew(initializeBlade, function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
        } else {
            assignments.get({ id: blade.currentEntityId }, initializeBlade, function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
            if (parentRefresh && angular.isFunction(blade.parentBlade.refresh)) {
                blade.parentBlade.refresh();
            }
        }
    };

    function initializeBlade(data) {
        _.each(data.dynamicExpression.children, extendElementBlock);
        groupAvailableChildren(data.dynamicExpression.children[0]);

        blade.currentEntity = angular.copy(data);
        blade.origEntity = data;
        blade.isLoading = false;

        if (!$scope.blade.isNew) {
            $scope.blade.toolbarCommands = [
                {
                    name: "platform.commands.save",
                    icon: 'fa fa-save',
                    executeMethod: function () {
                        $scope.saveChanges();
                    },
                    canExecuteMethod: function () {
                        return isDirty() && $scope.formScope && $scope.formScope.$valid;
                    },
                    permission: 'pricing:update'
                },
                {
                    name: "platform.commands.reset",
                    icon: 'fa fa-undo',
                    executeMethod: function () {
                        angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
                    },
                    canExecuteMethod: function () {
                        return isDirty();
                    },
                    permission: 'pricing:update'
                }
            ];
        }
    };

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity);
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    //$scope.isValid = function () {
    //    return $scope.formScope && $scope.formScope.$valid;
    //}

    $scope.saveChanges = function () {
        if (blade.isNew) {
            blade.isLoading = true;
            blade.currentEntity.dynamicExpression.availableChildren = undefined;
            _.each(blade.currentEntity.dynamicExpression.children, stripOffUiInformation);

            assignments.save({}, blade.currentEntity, function (data) {
                blade.isNew = undefined;
                blade.currentEntityId = data.id;
                blade.refresh(true);
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        } else {
            blade.isLoading = true;
            blade.currentEntity.dynamicExpression.availableChildren = undefined;
            _.each(blade.currentEntity.dynamicExpression.children, stripOffUiInformation);

            assignments.update({}, blade.currentEntity, function (data) {
                blade.refresh(true);
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        }
    };

    blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "pricing.dialogs.assignment-save.title",
                message: "pricing.dialogs.assignment-save.message",
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

    $scope.blade.headIcon = 'fa-usd';

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

    $scope.format = 'shortDate';

    // Dynamic ExpressionBlock
    function extendElementBlock(expressionBlock) {
        var retVal = dynamicExpressionService.expressions[expressionBlock.id];
        if (!retVal) {
            retVal = { displayName: 'unknown element: ' + expressionBlock.id };
        }

        _.extend(expressionBlock, retVal);

        if (!expressionBlock.children) {
            expressionBlock.children = [];
        }

        _.each(expressionBlock.children, extendElementBlock);
        _.each(expressionBlock.availableChildren, extendElementBlock);
        return expressionBlock;
    }

    function groupAvailableChildren(expressionBlock) {
        results = _.groupBy(expressionBlock.availableChildren, 'groupName');
        expressionBlock.availableChildren = _.map(results, function (items, key) { return { displayName: key, subitems: items }; });
    }

    function stripOffUiInformation(expressionElement) {
        expressionElement.availableChildren = undefined;
        expressionElement.displayName = undefined;
        expressionElement.getValidationError = undefined;
        expressionElement.groupName = undefined;
        expressionElement.newChildLabel = undefined;
        expressionElement.templateURL = undefined;

        _.each(expressionElement.children, stripOffUiInformation);
    };

    // actions on load
    $scope.catalogs = catalogs.query();
    $scope.pricelists = pricelists.query();
    blade.refresh();
}]);