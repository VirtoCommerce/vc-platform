angular.module('virtoCommerce.marketingModule')
.controller('promotionDetailController', ['$scope', 'bladeNavigationService', 'promotions', 'catalogs', 'stores', 'settings', 'dialogService', 'vaDynamicExpressionService', function ($scope, bladeNavigationService, promotions, catalogs, stores, settings, dialogService, vaDynamicExpressionService) {
    $scope.blade.refresh = function (parentRefresh) {
        if ($scope.blade.isNew) {
            promotions.getNew({}, function (data) {
                initializeBlade(data);
            });
        } else {
            promotions.get({ id: $scope.blade.currentEntityId }, function (data) {
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

        initializeExpressions(data.dynamicExpression);

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
        $scope.blade.isLoading = true;

        _.each($scope.blade.currentEntity.dynamicExpression.children, stripOffUiInformation);

        if ($scope.blade.isNew) {
            promotions.save({}, $scope.blade.currentEntity, function (data) {
                $scope.blade.isNew = undefined;
                $scope.blade.currentEntityId = data.id;
                initializeToolbar();
                $scope.blade.refresh(true);
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, $scope.blade);
            });
        } else {
            promotions.update({}, $scope.blade.currentEntity, function (data) {
                $scope.blade.refresh(true);
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, $scope.blade);
            });
        }
    };

    function stripOffUiInformation(expressionElement) {
        expressionElement.availableChildren = undefined;
        expressionElement.getValidationError = undefined;
        expressionElement.headerElements = undefined;
        expressionElement.newChildLabel = undefined;

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
    function initializeExpressions(data) {
        //data.children = getTestExpressionBlocks();
        _.each(data.children, extendElementBlock);
    }

    function extendElementBlock(expressionBlock) {
        var retVal;
        if (vaDynamicExpressionService.expressions[expressionBlock.id]) {
            retVal = vaDynamicExpressionService.expressions[expressionBlock.id];
        }
        switch (expressionBlock.id) {
            case 'BlockCustomerCondition':
                retVal = {
                    headerElements: constructAllAnyBlock(expressionBlock, 'For visitor with', 'of these eligibilities'),
                    newChildLabel: '+ add usergroup'
                }
                break;
            case 'ConditionIsEveryone':
                retVal = {
                    displayName: 'Everyone',
                    headerElements: [constructLabelElement('Everyone')],
                };
                break;
            case 'ConditionIsFirstTimeBuyer':
                retVal = {
                    displayName: 'First time buyer',
                    headerElements: [constructLabelElement('First time buyer')],
                };
                break;
            case 'ConditionIsRegisteredUser':
                retVal = {
                    displayName: 'Registered user',
                    headerElements: [constructLabelElement('Registered user')],
                };
                break;

            case 'BlockCatalogCondition':
                retVal = {
                    headerElements: constructAllAnyBlock(expressionBlock, 'if', 'of these catalog conditions are true'),
                    newChildLabel: '+ add condition'
                }
                break;
            //case 'ConditionEntryIs':
            //    retVal = {
            //        displayName: 'Product is []',
            //        headerElements: [constructLabelElement('Product is '),
            //                         constructItemSelector(expressionBlock)]
            //    };
            //    break;
            case 'ConditionCurrencyIs':
                retVal = {
                    displayName: 'Currency is []',
                    headerElements: [constructLabelElement('Currency is '),
                                     {
                                         type: 'currency',
                                         $parentElement: expressionBlock,
                                         availableEntries: settings.getValues({ id: 'VirtoCommerce.Core.General.Currencies' })
                                     }]
                };
                break;

            case 'BlockCartCondition':
                retVal = {
                    headerElements: constructAllAnyBlock(expressionBlock, 'if', 'of these cart conditions are true'),
                    newChildLabel: '+ add condition'
                }
                break;
            case 'RewardBlock':
                retVal = {
                    headerElements: [constructLabelElement('They get: ')],
                    newChildLabel: '+ add effect',
                    getValidationError: function (data) {
                        if (data.children && data.children.length) {
                            return undefined;
                        } else {
                            return 'Promotion requires at least one reward';
                        }
                    }
                };
                break;
            case 'RewardCartGetOfAbsSubtotal':
                retVal = {
                    displayName: 'Get $ [] off cart subtotal',
                    headerElements: constructAmountBlock(expressionBlock, 'Get $', 'off cart subtotal')
                };
                break;
            case 'RewardCartGetOfRelSubtotal':
                retVal = {
                    displayName: 'Get [] % off cart subtotal',
                    headerElements: constructAmountBlock(expressionBlock, 'Get ', ' % off cart subtotal')
                };
                break;
            case 'RewardItemGetFreeNumItemOfProduct':
                retVal = {
                    displayName: 'Get [] free items of Product',
                    headerElements: constructTypedBlock(expressionBlock, 'Get ', ' free items of Product', 'numericInput')
                };
                retVal.headerElements.push(constructItemSelector(expressionBlock));
                break;
            case 'RewardItemGetOfAbs':
                retVal = {
                    displayName: 'Get $[] off',
                    headerElements: constructAmountBlock(expressionBlock, 'Get $', ' off')
                };
                retVal.headerElements.push(constructItemSelector(expressionBlock));
                break;
            case 'RewardItemGetOfRel':
                retVal = {
                    displayName: 'Get [] % off',
                    headerElements: constructAmountBlock(expressionBlock, 'Get ', ' % off')
                };
                retVal.headerElements.push(constructItemSelector(expressionBlock));
                break;
            case 'RewardItemGetOfAbsForNum':
                retVal = {
                    displayName: 'Get $[] off [] items',
                    headerElements: constructAmountBlock(expressionBlock, 'Get $', ' off for')
                };
                retVal.headerElements.push({
                    type: 'numericInput',
                    $parentElement: expressionBlock
                });
                retVal.headerElements.push(constructLabelElement('  items'));
                break;
            case 'RewardItemGetOfRelForNum':
                retVal = {
                    displayName: 'Get [] % off [] items',
                    headerElements: constructAmountBlock(expressionBlock, 'Get ', ' % off for')
                };
                retVal.headerElements.push({
                    type: 'numericInput',
                    $parentElement: expressionBlock
                });
                retVal.headerElements.push(constructLabelElement('  items'));
                break;
            case 'RewardShippingGetOfAbsShippingMethod':
                retVal = {
                    displayName: 'Get $[] off shipping',
                    headerElements: constructAmountBlock(expressionBlock, 'Get $', ' off shipping')
                };
                retVal.headerElements.push({
                    type: 'shippingMethod',
                    $parentElement: expressionBlock,
                    availableEntries: shippingMethods
                });
                break;
            case 'RewardShippingGetOfRelShippingMethod':
                retVal = {
                    displayName: 'Get [] % off shipping',
                    headerElements: constructAmountBlock(expressionBlock, 'Get ', ' % off shipping')
                };
                retVal.headerElements.push({
                    type: 'shippingMethod',
                    $parentElement: expressionBlock,
                    availableEntries: shippingMethods
                });
                break;
            default:
                retVal = {
                    displayName: 'unknown element: ' + expressionBlock.id,
                    headerElements: [constructLabelElement('unknown element: ' + expressionBlock.id)]
                };
        }

        //angular.merge(expressionBlock, retVal);
        //angular.extend(expressionBlock, retVal);
        _.extend(expressionBlock, retVal);

        _.each(expressionBlock.children, extendElementBlock);
        _.each(expressionBlock.availableChildren, extendElementBlock);
        return expressionBlock;
    };

    function openCategorySelectWizard(expressionElement) {
        var selectedListEntries = [];
        var newBlade = {
            id: "CatalogCategorySelect",
            title: "Pick Category for promotion condition",
            controller: 'catalogItemSelectController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/common/catalog-items-select.tpl.html',
            breadcrumbs: [],
            bladeToolbarCommands: [
            {
                name: "Pick selected", icon: 'fa fa-plus',
                executeMethod: function (blade) {
                    expressionElement.selectedCategory = selectedListEntries[0];
                    bladeNavigationService.closeBlade(blade);
                },
                canExecuteMethod: function () {
                    return selectedListEntries.length == 1;
                }
            }]
        };

        newBlade.options = {
            allowMultiple: true,
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

    function openItemSelectWizard(expressionElement, parentElement) {
        var selectedListEntries = [];
        var newBlade = {
            id: "CatalogEntrySelect",
            title: "Pick Product for promotion condition",
            controller: 'catalogItemSelectController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/common/catalog-items-select.tpl.html',
            breadcrumbs: [],
            bladeToolbarCommands: [
            {
                name: "Pick selected", icon: 'fa fa-plus',
                executeMethod: function (blade) {
                    expressionElement.selectedListEntry = selectedListEntries[0];
                    parentElement.productId = selectedListEntries[0].id;
                    bladeNavigationService.closeBlade(blade);
                },
                canExecuteMethod: function () {
                    return selectedListEntries.length == 1;
                }
            }]
        };

        newBlade.options = {
            allowMultiple: true,
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

    function constructLabelElement(text) {
        return {
            type: 'label',
            text: text
        };
    };

    function constructAllAnyBlock(parentElement, prefix, postfix) {
        return [
            constructLabelElement(prefix + ' '),
            {
                type: 'allAny',
                $parentElement: parentElement // has .all
            },
            constructLabelElement(' ' + postfix)];
    };

    function constructAmountBlock(parentElement, prefix, postfix) {
        return constructTypedBlock(parentElement, prefix, postfix, 'amountInput');
    };

    function constructTypedBlock(parentElement, prefix, postfix, type) {
        return [
            constructLabelElement(prefix),
            {
                type: type,
                $parentElement: parentElement // has required property
            },
            constructLabelElement(' ' + postfix)];
    };

    var constructCategorySelector = function (selectLabel) {
        selectLabel = selectLabel ? selectLabel : 'select Category';
        var retVal = {
            type: 'customSelector',
            selectedCategory: undefined
        };

        retVal.getDisplayText = function () {
            return retVal.selectedCategory ? retVal.selectedCategory.name : selectLabel;
        };

        retVal.action = function () { openCategorySelectWizard(retVal); };
        return retVal;
    };

    function constructItemSelector(parentElement, selectLabel) {
        selectLabel = selectLabel ? selectLabel : 'select Product';
        var retVal = {
            type: 'customSelector',
            selectedListEntry: undefined
        };
        retVal.getDisplayText = function () {
            return retVal.selectedListEntry ? retVal.selectedListEntry.name : selectLabel;
        };

        retVal.action = function () { openItemSelectWizard(retVal, parentElement); };
        return retVal;
    };

    var getTestExpressionBlocks = function () {
        return [
        {
            headerElements: [
                {
                    type: 'label',
                    text: 'For visitor with '
                },
                {
                    type: 'dictionary',
                    text: 'all'
                },
                {
                    type: 'label',
                    text: ' of these eligibilities'
                }],
            children: [
                {
                    headerElements: [
                        {
                            type: 'label',
                            text: 'Everyone'
                        }
                    ]
                },
                {
                    headerElements: [
                        {
                            type: 'label',
                            text: 'First time buyer'
                        }
                    ]
                },
                {
                    headerElements: [
                        {
                            type: 'label',
                            text: 'Registered user'
                        }
                    ]
                }],
            newChildLabel: '+ add usergroup',
            getValidationError: function (data) {
                if (data.children && data.children.length) {
                    return undefined;
                } else {
                    return 'Promotion requires at least one eligibility';
                }
            }
        },
        {
            headerElements: [
                {
                    type: 'label',
                    text: 'if '
                },
                {
                    type: 'dictionary',
                    text: 'all'
                },
                {
                    type: 'label',
                    text: ' of these conditions are true'
                }],
            children: [
                {
                    headerElements: [
                        {
                            type: 'label',
                            text: 'At least'
                        },
                        {
                            type: 'numericInput',
                            // text: 'qty'
                            number: 0
                        },
                        {
                            type: 'label',
                            text: 'items in shopping cart excluding:'
                        }
                    ],
                    children: [
                        {
                            headerElements: [
                                {
                                    type: 'label',
                                    text: 'items of category'
                                },
                                constructCategorySelector()
                            ]
                        }
                    ],
                    newChildLabel: '+ excluding'
                }
            ],
            newChildLabel: '+ add condition',
            getValidationError: function (data) {
                return undefined;
            }
        },
        {
            headerElements: [
                {
                    type: 'label',
                    text: 'They get:'
                }],
            children: [],
            newChildLabel: '+ add effect',
            getValidationError: function (data) {
                if (data.children && data.children.length) {
                    return undefined;
                } else {
                    return 'Promotion requires at least one reward';
                }
            }
        }
        ];
    };

    initializeToolbar();
    $scope.blade.refresh(false);
    //$scope.catalogs = catalogs.getCatalogs();
    $scope.stores = stores.query();
    //$scope.exclusivities = settings.getValues({ id: 'VirtoCommerce.Marketing.Promotions.Exclusivities' }, function (data) {
    //    if ($scope.blade.isNew && data && data[0]) {
    //        $scope.blade.currentEntity.exclusivity = data[0];
    //    }
    //});
    var shippingMethods = [{ id: 1, name: 'method1' }, { id: 2, name: 'method 2' }];
}]);