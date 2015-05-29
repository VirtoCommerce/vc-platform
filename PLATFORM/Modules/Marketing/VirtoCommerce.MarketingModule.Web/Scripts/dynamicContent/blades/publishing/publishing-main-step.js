angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.addPublishingFirstStepController.expressions', ['$scope', 'virtoCommerce.coreModule.common.countries', function ($scope, countries) {
    $scope.timeZones = countries.getTimeZones();
}])
.controller('virtoCommerce.marketingModule.addPublishingFirstStepController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.contentPublications', 'platformWebApp.bladeNavigationService', 'virtoCommerce.coreModule.common.dynamicExpressionService', function ($scope, contentPublications, bladeNavigationService, dynamicExpressionService) {
    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    var blade = $scope.blade;

    blade.initializeBlade = function () {
        if (!blade.isNew) {
            contentPublications.get({ id: blade.entity.id }, function (data) {
                initializeBlade(data);

                $scope.blade.toolbarCommands = [
				    {
				        name: "Save", icon: 'fa fa-save',
				        executeMethod: function () {
				            blade.saveChanges();
				        },
				        canExecuteMethod: function () {
				            return blade.checkDifferense();
				        },
				        permission: 'marketing:manage'
				    },
				    {
				        name: "Reset", icon: 'fa fa-undo',
				        executeMethod: function () {
				            blade.entity = angular.copy(blade.originalEntity);
				        },
				        canExecuteMethod: function () {
				            return blade.checkDifferense();
				        },
				        permission: 'marketing:manage'
				    },
				    {
				        name: "Delete", icon: 'fa fa-trash',
				        executeMethod: function () {
				            bladeNavigationService.closeBlade(blade);
				            blade.delete();
				        },
				        canExecuteMethod: function () {
				            return true;
				        },
				        permission: 'marketing:manage'
				    }
                ];
            });
        }
        else {
            contentPublications.getNew(initializeBlade);
        }
    }

    function initializeBlade(data) {
        _.each(data.dynamicExpression.children, extendElementBlock);
        groupAvailableChildren(data.dynamicExpression.children[0]);

        blade.entity = data;
        blade.originalEntity = angular.copy(blade.entity);
        blade.isLoading = false;
    }

    blade.addPlaceholders = function () {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'publishing_add_placeholders',
            title: 'Add placeholders elements',
            subtitle: 'Add placeholders elements',
            entity: blade.entity,
            controller: 'virtoCommerce.marketingModule.addPublishingPlaceholdersStepController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/publishing/add-placeholders.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    blade.addContentItems = function () {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'publishing_add_content_items',
            title: 'Add content items elements',
            subtitle: 'Add content items elements',
            entity: blade.entity,
            controller: 'virtoCommerce.marketingModule.addPublishingContentItemsStepController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/publishing/add-content-items.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    blade.closeChildrenBlades = function () {
        angular.forEach(blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    blade.saveChanges = function () {
        blade.closeChildrenBlades();

        blade.isLoading = true;
        blade.entity.dynamicExpression.availableChildren = undefined;
        _.each(blade.entity.dynamicExpression.children, stripOffUiInformation);

        if (blade.isNew) {
            contentPublications.save({}, blade.entity, function (data) {
                blade.entity = data;
                blade.originalEntity = angular.copy(data);

                blade.isNew = false;
                blade.initializeBlade();
                blade.parentBlade.isLoading = true;
                blade.parentBlade.initialize();
            });
        }
        else {
            contentPublications.update({}, blade.entity, function (data) {
                blade.entity = data;
                blade.originalEntity = angular.copy(data);

                blade.isNew = false;
                blade.initializeBlade();
                blade.parentBlade.isLoading = true;
                blade.parentBlade.initialize();
            });
        }
    }

    blade.availableSave = function () {
        return !$scope.formScope.$invalid &&
			blade.entity.contentItems.length > 0 &&
			blade.entity.contentPlaces.length > 0;
    }

    blade.delete = function () {
        contentPublications.delete({ ids: [blade.entity.id] }, function () {
            blade.parentBlade.isLoading = true;
            blade.parentBlade.initialize();
        });
    }

    blade.checkDifferense = function () {
        var retVal = !$scope.formScope.$invalid &&
							blade.entity.contentItems.length > 0 &&
							blade.entity.contentPlaces.length > 0;

        if (retVal) {
            retVal = !angular.equals(blade.entity.name, blade.originalEntity.name) ||
				!angular.equals(blade.entity.description, blade.originalEntity.description) ||
				!angular.equals(blade.entity.priority, blade.originalEntity.priority) ||
				!angular.equals(blade.entity.isActive, blade.originalEntity.isActive) ||
				!angular.equals(blade.entity.startDate, blade.originalEntity.startDate) ||
				!angular.equals(blade.entity.endDate, blade.originalEntity.endDate) ||
				!angular.equals(blade.entity.dynamicExpression, blade.originalEntity.dynamicExpression) ||
				blade.entity.contentItems.length !== blade.originalEntity.contentItems.length ||
				blade.entity.contentPlaces.length !== blade.originalEntity.contentPlaces.length;

            if (!retVal) {
                var ciIdse = blade.entity.contentItems.map(function (v) {
                    return v.id;
                });

                var ciIdsoe = blade.originalEntity.contentItems.map(function (v) {
                    return v.id;
                });

                retVal = _.intersection(ciIdse, ciIdsoe).length < Math.max(ciIdse.length, ciIdsoe.length);
            }

            if (!retVal) {
                var cpIdse = blade.entity.contentPlaces.map(function (v) {
                    return v.id;
                });

                var cpIdsoe = blade.originalEntity.contentPlaces.map(function (v) {
                    return v.id;
                });

                retVal = _.intersection(cpIdse, cpIdsoe).length < Math.max(cpIdse.length, cpIdsoe.length);
            }
        }

        return retVal;
    }

    // datepicker
    $scope.datepickers = {
        endDate: false,
        startDate: false,
    }

    $scope.showWeeks = true;
    $scope.toggleWeeks = function () {
        $scope.showWeeks = !$scope.showWeeks;
    };

    $scope.clear = function () {
        $scope.blade.currentEntity.birthDate = null;
    };
    $scope.today = new Date();

    $scope.open = function ($event, which) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.datepickers[which] = true;
    };

    $scope.dateOptions = {
        'year-format': "'yyyy'",
        'starting-day': 1
    };

    $scope.formats = ['shortDate', 'dd-MMMM-yyyy', 'yyyy/MM/dd'];
    $scope.format = $scope.formats[0];

    $scope.blade.headIcon = 'fa fa-paperclip';

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

    blade.initializeBlade();
}]);