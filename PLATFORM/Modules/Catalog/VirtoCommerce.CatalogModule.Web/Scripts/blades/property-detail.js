﻿angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.propertyDetailController', ['$scope', 'virtoCommerce.catalogModule.categories', 'virtoCommerce.catalogModule.properties', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, categories, properties, bladeNavigationService, dialogService) {
    var b = $scope.blade;
    var formScope;
    b.origEntity = {};
    
    $scope.currentChild = undefined;

    b.refresh = function (parentRefresh) {
        if (b.currentEntityId) {
            properties.get({ propertyId: b.currentEntityId }, function (data) {
                initializeBlade(data);
                if (parentRefresh) {
                    b.parentBlade.refresh(data);
                }
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
        } else if (b.categoryId) {
            properties.newCategoryProperty({ categoryId: b.categoryId }, function (data) {
                initializeBlade(data);
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
        }
        else if (b.catalogId) {
            properties.newCatalogProperty({ catalogId: b.catalogId }, function (data) {
                initializeBlade(data);
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
        }
    };

    $scope.openChild = function (childType) {
        var newBlade = { id: "propertyChild" };
        newBlade.property = b.currentEntity;

        switch (childType) {
            case 'attr':
                newBlade.title = 'catalog.blades.property-attributes.title';
                newBlade.titleValues = {name: b.origEntity.name ? b.origEntity.name : b.currentEntity.name};
                newBlade.subtitle = 'catalog.blades.property-attributes.subtitle';
                newBlade.controller = 'virtoCommerce.catalogModule.propertyAttributesController';
                newBlade.template = 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/property-attributes.tpl.html';
                break;
            case 'valType':
                newBlade.title = 'catalog.blades.property-valueType.title';
                newBlade.titleValues = {name: b.origEntity.name ? b.origEntity.name : b.currentEntity.name};
                newBlade.subtitle = 'catalog.blades.property-valueType.subtitle';
                newBlade.controller = 'virtoCommerce.catalogModule.propertyValueTypeController';
                newBlade.template = 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/property-valueType.tpl.html';
                break;
            case 'appliesto':
                newBlade.title = 'catalog.blades.property-type.title';
                newBlade.titleValues = { name: b.origEntity.name ? b.origEntity.name : b.currentEntity.name };
                newBlade.subtitle = 'catalog.blades.property-type.subtitle';
                newBlade.controller = 'virtoCommerce.catalogModule.propertyTypeController';
                newBlade.template = 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/property-type.tpl.html';
                newBlade.availablePropertyTypes = b.catalogId ? ['Product', 'Variation', 'Category', 'Catalog'] : ['Product', 'Variation', 'Category'];
                break;
            case 'dict':
                newBlade.title = 'catalog.blades.property-dictionary.title';
                newBlade.titleValues = {name: b.origEntity.name ? b.origEntity.name : b.currentEntity.name};
                newBlade.subtitle = 'catalog.blades.property-dictionary.subtitle';
                newBlade.controller = 'virtoCommerce.catalogModule.propertyDictionaryController';
                newBlade.template = 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/property-dictionary.tpl.html';
                break;
        }
        bladeNavigationService.showBlade(newBlade, $scope.blade);
        $scope.currentChild = childType;
    }

    function initializeBlade(data) {
        if (data.valueType === 'Number' && data.dictionaryValues) {
            _.forEach(data.dictionaryValues, function (entry) {
                entry.value = parseFloat(entry.value);
            });
        }

        b.currentEntity = angular.copy(data);
        b.origEntity = data;
        b.isLoading = false;
    };

    function isDirty() {
        return !angular.equals(b.currentEntity, b.origEntity);
    };

    function saveChanges() {
        b.isLoading = true;
        properties.update(b.currentEntity, function (data, headers) {
            b.currentEntityId = data.id;
            b.refresh(true);
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    };

    function removeProperty(prop) {
        var dialog = {
            id: "confirmDelete",
            title: "catalog.dialogs.property-delete.title",
            message: "catalog.dialogs.property-delete.subtitle",
            messageValues: { name: prop.name },
            callback: function (remove) {
                if (remove) {
                    $scope.blade.isLoading = true;

                    properties.remove({ id: prop.id }, function () {
                        $scope.bladeClose();
                        b.parentBlade.refresh();
                    },
                    function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    b.onClose = function (closeCallback) {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });

        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "catalog.dialogs.property-save.title",
                message: "catalog.dialogs.property-save.message",
                callback: function (needSave) {
                    if (needSave) {
                        saveChanges();
                    }
                    closeCallback();
                }
            };
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeCallback();
        }
    };

    $scope.setForm = function (form) {
        formScope = form;
    }

    $scope.blade.headIcon = 'fa-gear';

    $scope.blade.toolbarCommands = [
		{
		    name: "platform.commands.save", icon: 'fa fa-save',
		    executeMethod: function () {
		        saveChanges();
		    },
		    canExecuteMethod: function () {
		        return (b.origEntity.isNew || isDirty()) && formScope && formScope.$valid;
		    }
		},
        {
            name: "platform.commands.reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(b.origEntity, b.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        },
		   {
		       name: "platform.commands.delete", icon: 'fa fa-trash-o',
		       executeMethod: function () {
		           removeProperty(b.origEntity);
		       },
		       canExecuteMethod: function () {
		           return b.origEntity.isManageable && !(b.origEntity.isNew || isDirty());
		       }
		   }
    ];

    // actions on load    
    b.refresh();
}]);
