angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.addPublishingFirstStepController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.contentPublications', 'platformWebApp.bladeNavigationService', 'virtoCommerce.coreModule.common.dynamicExpressionService', 'virtoCommerce.storeModule.stores', 'platformWebApp.dialogService', function ($scope, contentPublications, bladeNavigationService, dynamicExpressionService, stores, dialogService) {
    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    var blade = $scope.blade;
    blade.updatePermission = 'marketing:update';

    blade.initializeBlade = function () {
        if (!blade.isNew) {
            contentPublications.get({ id: blade.entity.id }, function (data) {
                initializeBlade(data);

                $scope.blade.toolbarCommands = [
				    {
				        name: "platform.commands.save", icon: 'fa fa-save',
				        executeMethod: function () {
				            blade.saveChanges();
				        },
				        canExecuteMethod: function () {
				            return blade.checkDifferense();
				        },
				        permission: blade.updatePermission
				    },
				    {
				        name: "platform.commands.reset", icon: 'fa fa-undo',
				        executeMethod: function () {
				            blade.entity = angular.copy(blade.originalEntity);
				        },
				        canExecuteMethod: function () {
				            return blade.checkDifferense();
				        },
				        permission: blade.updatePermission
				    },
				    {
				        name: "platform.commands.delete", icon: 'fa fa-trash',
				        executeMethod: function () {
				            var dialog = {
				                id: "confirmDeleteContentItem",
				                title: "marketing.dialogs.publication-delete.title",
				                message: "marketing.dialogs.publication-delete.message",
				                callback: function (remove) {
				                    if (remove) {
				                        bladeNavigationService.closeBlade(blade);
				                        blade.delete();
				                    }
				                }
				            };

				            dialogService.showConfirmationDialog(dialog);
				        },
				        canExecuteMethod: function () {
				            return true;
				        },
				        permission: blade.updatePermission
				    }
                ];
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
        }
        else {
            contentPublications.getNew(initializeBlade, function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
        }
    }

    function initializeBlade(data) {
        _.each(data.dynamicExpression.children, extendElementBlock);
        groupAvailableChildren(data.dynamicExpression.children[0]);

        blade.entity = data;
        blade.originalEntity = angular.copy(blade.entity);
        blade.isLoading = false;
        $scope.$watch('blade.entity', blade.autogenerateName, true);
    }

    blade.addPlaceholders = function () {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'publishing_add_placeholders',
            title: 'marketing.blades.publishing.add-placeholders.title',
            subtitle: 'marketing.blades.publishing.add-placeholders.subtitle',
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
            title: 'marketing.blades.publishing.add-content-items.title',
            subtitle: 'marketing.blades.publishing.add-content-items.subtitle',
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
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
        }
        else {
            contentPublications.update({}, blade.entity, function (data) {
                blade.entity = data;
                blade.originalEntity = angular.copy(data);

                blade.isNew = false;
                blade.initializeBlade();
                blade.parentBlade.isLoading = true;
                blade.parentBlade.initialize();
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
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
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    }

    blade.checkDifferense = function () {
        var retVal = !$scope.formScope.$invalid &&
							blade.entity.contentItems.length > 0 &&
							blade.entity.contentPlaces.length > 0;

        if (retVal) {
        	retVal = !angular.equals(blade.entity, blade.originalEntity);

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

    $scope.blade.headIcon = 'fa-paperclip';

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

    String.prototype.trimLeft = function (charlist) {
        if (charlist === undefined)
            charlist = "\s";

        return this.replace(new RegExp("^[" + charlist + "]+"), "");
    };
    String.prototype.trimRight = function (charlist) {
        if (charlist === undefined)
            charlist = "\s";

        return this.replace(new RegExp("[" + charlist + "]+$"), "");
    };

    blade.focusNameInput = false;
    blade.autogenerateName = function () {
        if (!blade.focusNameInput) {
            var placeholderPublicationNamePart = '';
            var contentItemPublicationNamePart = '';

            if (!angular.isUndefined(blade.entity)) {
                if (!angular.isUndefined(blade.entity.contentPlaces) && blade.entity.contentPlaces.length == 1) {
                    placeholderPublicationNamePart = blade.entity.contentPlaces[0].name;
                }

                if (!angular.isUndefined(blade.entity.contentItems) && blade.entity.contentItems.length == 1) {
                    contentItemPublicationNamePart = blade.entity.contentItems[0].name;
                }
            }

            var newName = (placeholderPublicationNamePart + '_' + contentItemPublicationNamePart).trimLeft('_').trimRight('_');

            if (!angular.isUndefined(blade.entity.name) && blade.entity.name !== null && blade.entity.name !== '' && newName !== '_') {
                if (blade.entity.name.indexOf(placeholderPublicationNamePart) >= 0 || blade.entity.name.indexOf(contentItemPublicationNamePart) >= 0) {
                    blade.entity.name = newName;
                }
            }
            else if (newName !== '_') {
                blade.entity.name = newName;
            }
        }
    }

    blade.initializeBlade();
	$scope.stores = stores.query();
}]);