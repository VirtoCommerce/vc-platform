angular.module('virtoCommerce.marketingModule')
.controller('promotionDetailController', ['$scope', 'bladeNavigationService', 'promotions', 'catalogs', 'stores', 'settings', 'dialogService', function ($scope, bladeNavigationService, promotions, catalogs, stores, settings, dialogService) {
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
        //var expressionBlocks = getTestExpressionBlocks();
        //var expressionBlocks = [
        //{
        //    children: [],
        //    newChildLabel: '+ add',
        //    availableChildren: data.availableChildren
        //}];
        var expressionBlocks = data.children;
        _.each(expressionBlocks, function (expressionBlock) {
            //angular.merge(expressionBlock, constructElementBlock(expressionBlock));
            //angular.extend(expressionBlock, constructElementBlock(expressionBlock));
            _.extend(expressionBlock, constructElementBlock(expressionBlock));
        });

        $scope.expressionBlocks = expressionBlocks;
    }

    $scope.addChild = function (availableElement, parent) {

        parent.children.push(constructElementBlock(availableElement));
    };

    $scope.deleteChild = function (child, parentList) {
        parentList.splice(parentList.indexOf(child), 1);
    }

    function constructElementBlock(expressionBlock) {
        var retVal = { id: expressionBlock.id, children: [] };
        switch (expressionBlock.id) {
            case 'CustomerConditionBlock':
                break;
            case 'RewardBlock':
                retVal.headerElements = [
                    {
                        type: 'label',
                        text: 'They get: '
                    }
                ];

                retVal.newChildLabel = '+ add effect';
                retVal.getValidationError = function (data) {
                    if (!data.children.length) {
                        return 'Promotion requires at least one reward';
                    } else {
                        return undefined;
                    }
                };
                break;
            case 'RewardCartGetOfAbsSubtotal':
                retVal.headerElements = [
                    {
                        type: 'label',
                        text: 'Get $'
                    },
                    {
                        type: 'numericInput',
                        number: 0
                    },
                    {
                        type: 'label',
                        text: ' off cart subtotal'
                    }
                ];
                break;
            default:
                retVal.headerElements = [
                    {
                        type: 'label',
                        text: 'unknown element: ' + expressionBlock.id
                    }
                ];
        }

        return retVal;
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

    var constructCategorySelector = function (selectLabel) {
        selectLabel = selectLabel ? selectLabel : 'select category';
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
                if (!data.children.length) {
                    return 'Promotion requires at least one eligibility';
                } else {
                    return undefined;
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
                if (!data.children.length) {
                    return 'Promotion requires at least one reward';
                } else {
                    return undefined;
                }
            }
        }
        ];
    };

    initializeToolbar();
    $scope.blade.refresh(false);
    $scope.catalogs = catalogs.getCatalogs();
    $scope.stores = stores.query();
    //$scope.exclusivities = settings.getValues({ id: 'VirtoCommerce.Marketing.Promotions.Exclusivities' }, function (data) {
    //    if ($scope.blade.isNew && data && data[0]) {
    //        $scope.blade.currentEntity.exclusivity = data[0];
    //    }
    //});
}]);