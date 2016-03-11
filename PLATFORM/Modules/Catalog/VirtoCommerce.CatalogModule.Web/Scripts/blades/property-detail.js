angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.propertyDetailController', ['$scope', 'virtoCommerce.catalogModule.properties', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, properties, bladeNavigationService, dialogService) {
    var blade = $scope.blade;
    blade.updatePermission = 'catalog:update';
    blade.origEntity = {};

    $scope.currentChild = undefined;

    blade.refresh = function (parentRefresh) {
        if (blade.currentEntityId) {
            properties.get({ propertyId: blade.currentEntityId }, function (data) {
                initializeBlade(data);
                if (parentRefresh) {
                    blade.parentBlade.refresh(data);
                }
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        } else if (blade.categoryId) {
            properties.newCategoryProperty({ categoryId: blade.categoryId }, function (data) {
                initializeBlade(data);
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        }
        else if (blade.catalogId) {
            properties.newCatalogProperty({ catalogId: blade.catalogId }, function (data) {
                initializeBlade(data);
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        }
    };

    $scope.openChild = function (childType) {
        var newBlade = { id: "propertyChild" };
        newBlade.property = blade.currentEntity;

        switch (childType) {
            case 'attr':
                newBlade.title = 'catalog.blades.property-attributes.title';
                newBlade.titleValues = { name: blade.origEntity.name ? blade.origEntity.name : blade.currentEntity.name };
                newBlade.subtitle = 'catalog.blades.property-attributes.subtitle';
                newBlade.controller = 'virtoCommerce.catalogModule.propertyAttributesController';
                newBlade.template = 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/property-attributes.tpl.html';
                break;
            case 'valType':
                newBlade.title = 'catalog.blades.property-valueType.title';
                newBlade.titleValues = { name: blade.origEntity.name ? blade.origEntity.name : blade.currentEntity.name };
                newBlade.subtitle = 'catalog.blades.property-valueType.subtitle';
                newBlade.controller = 'virtoCommerce.catalogModule.propertyValueTypeController';
                newBlade.template = 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/property-valueType.tpl.html';
                break;
            case 'appliesto':
                newBlade.title = 'catalog.blades.property-type.title';
                newBlade.titleValues = { name: blade.origEntity.name ? blade.origEntity.name : blade.currentEntity.name };
                newBlade.subtitle = 'catalog.blades.property-type.subtitle';
                newBlade.controller = 'virtoCommerce.catalogModule.propertyTypeController';
                newBlade.template = 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/property-type.tpl.html';
                newBlade.availablePropertyTypes = blade.catalogId ? ['Product', 'Variation', 'Category', 'Catalog'] : ['Product', 'Variation', 'Category'];
                break;
            case 'dict':
                newBlade.title = 'catalog.blades.property-dictionary.title';
                newBlade.titleValues = { name: blade.origEntity.name ? blade.origEntity.name : blade.currentEntity.name };
                newBlade.subtitle = 'catalog.blades.property-dictionary.subtitle';
                newBlade.controller = 'virtoCommerce.catalogModule.propertyDictionaryController';
                newBlade.template = 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/property-dictionary.tpl.html';
                break;
        }
        bladeNavigationService.showBlade(newBlade, blade);
        $scope.currentChild = childType;
    }

    function initializeBlade(data) {
        if (data.valueType === 'Number' && data.dictionaryValues) {
            _.forEach(data.dictionaryValues, function (entry) {
                entry.value = parseFloat(entry.value);
            });
        }

        blade.currentEntity = angular.copy(data);
        blade.origEntity = data;
        blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
    }

    function canSave() {
        return (blade.origEntity.isNew || isDirty()) && formScope && formScope.$valid;
    }

    function saveChanges() {
        blade.isLoading = true;
        properties.update(blade.currentEntity, function (data, headers) {
            blade.currentEntityId = data.id;
            blade.refresh(true);
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    };

    function removeProperty(prop) {
        var dialog = {
            id: "confirmDelete",
            title: "catalog.dialogs.property-delete.title",
            message: "catalog.dialogs.property-delete.message",
            messageValues: { name: prop.name },
            callback: function (remove) {
                if (remove) {
                    blade.isLoading = true;

                    properties.remove({ id: prop.id }, function () {
                        $scope.bladeClose();
                        blade.parentBlade.refresh();
                    },
                    function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, saveChanges, closeCallback, "catalog.dialogs.property-save.title", "catalog.dialogs.property-save.message");
    };

    var formScope;
    $scope.setForm = function (form) { formScope = form; }

    blade.headIcon = 'fa-gear';

    blade.toolbarCommands = [
		{
		    name: "platform.commands.save", icon: 'fa fa-save',
		    executeMethod: saveChanges,
		    canExecuteMethod: canSave
		},
        {
            name: "platform.commands.reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(blade.origEntity, blade.currentEntity);
            },
            canExecuteMethod: isDirty
        },
		   {
		       name: "platform.commands.delete", icon: 'fa fa-trash-o',
		       executeMethod: function () {
		           removeProperty(blade.origEntity);
		       },
		       canExecuteMethod: function () {
		           return blade.origEntity.isManageable && !blade.origEntity.isNew;
		       }
		   }
    ];

    // actions on load    
    blade.refresh();
}]);
