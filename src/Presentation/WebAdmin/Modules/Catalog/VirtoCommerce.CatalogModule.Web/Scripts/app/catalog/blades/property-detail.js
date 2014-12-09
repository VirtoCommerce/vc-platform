angular.module('catalogModule.blades.propertyDetail', [
   'catalogModule.resources.categories',
    'catalogModule.resources.properties',
    'catalogModule.blades.propertyAttributes',
    'catalogModule.blades.propertyDictionary',
    'catalogModule.blades.propertyValueType',
    'catalogModule.blades.propertyType'
])
.controller('propertyDetailController', ['$scope', 'categories', 'properties', 'bladeNavigationService', 'dialogService', function ($scope, categories, properties, bladeNavigationService, dialogService) {
    var b = $scope.blade;
    var formScope;
    b.origEntity = {};
    // b.currentEntity = {};

    $scope.currentChild = undefined;

    b.refresh = function (parentRefresh) {
        if (b.currentEntityId) {
            properties.get({ propertyId: b.currentEntityId }, function (data) {
                initializeBlade(data);
                if (parentRefresh) {
                    b.parentBlade.refresh(data);
                }
            });
        } else {
            properties.newProperty({ categoryId: b.parentBlade.currentEntityId }, function (data) {
                initializeBlade(data);
            });
        }
    };

    $scope.openChild = function (childType) {
        var newBlade = { id: "propertyChild" };

        switch (childType) {
            case 'attr':
                newBlade.title = b.origEntity.name ? b.origEntity.name : b.currentEntity.name + ' attributes';
                newBlade.subtitle = 'manage attributes';
                newBlade.controller = 'propertyAttributesController';
                newBlade.template = 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/property-attributes.tpl.html';
                break;
            case 'valType':
                newBlade.title = b.origEntity.name ? b.origEntity.name : b.currentEntity.name + ' value type';
                newBlade.subtitle = 'Change value type';
                newBlade.controller = 'propertyValueTypeController';
                newBlade.template = 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/property-valueType.tpl.html';
                break;
            case 'appliesto':
                newBlade.title = b.origEntity.name ? b.origEntity.name : b.currentEntity.name + ' applies to';
                newBlade.subtitle = 'Change to what it applies';
                newBlade.controller = 'propertyTypeController';
                newBlade.template = 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/property-type.tpl.html';
                break;
            case 'dict':
                newBlade.title = b.origEntity.name ? b.origEntity.name : b.currentEntity.name + ' dictionary';
                newBlade.subtitle = 'Manage dictionary';
                newBlade.controller = 'propertyDictionaryController';
                newBlade.template = 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/property-dictionary.tpl.html';
                break;
        }
        bladeNavigationService.showBlade(newBlade, $scope.blade);
        $scope.currentChild = childType;
    }

    function initializeBlade(data) {
        if (data.valueType === 2 && data.dictionaryValues) {
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
        });
    };

    function removeProperty(prop) {
        var dialog = {
            id: "confirmDelete",
            title: "Delete confirmation",
            message: "Are you sure you want to delete Property '" + prop.name + "'?",
            callback: function (remove) {
                if (remove) {
                    $scope.blade.isLoading = true;

                    properties.remove({ id: prop.id }, function () {
                        $scope.bladeClose();
                        b.parentBlade.refresh();
                    });
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
                title: "Save changes",
                message: "The property has been modified. Do you want to save changes?",
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

    $scope.bladeToolbarCommands = [
		{
		    name: "Save", icon: 'icon-floppy',
		    executeMethod: function () {
		        saveChanges();
		    },
		    canExecuteMethod: function () {
		        return (b.origEntity.isNew || isDirty()) && formScope && formScope.$valid;
		    }
		},
        {
            name: "Reset", icon: 'icon-undo',
            executeMethod: function () {
                angular.copy(b.origEntity, b.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        },
		   {
		       name: "Delete", icon: 'icon-remove',
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
