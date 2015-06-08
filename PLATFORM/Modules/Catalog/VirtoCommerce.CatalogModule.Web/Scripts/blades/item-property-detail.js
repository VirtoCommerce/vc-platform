angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemPropertyDetailController', ['$scope', 'virtoCommerce.catalogModule.categories', 'virtoCommerce.catalogModule.properties', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, categories, properties, bladeNavigationService, dialogService) {
    var b = $scope.blade;
    $scope.currentChild = undefined;

    function initializeBlade(data) {
        if (data.valueType === 'Number' && data.dictionaryValues) {
            _.forEach(data.dictionaryValues, function (entry) {
                entry.value = parseFloat(entry.value);
            });
        }

        b.currentEntity = angular.copy(data);
        //b.origEntity = data;
        b.isLoading = false;
    };

    $scope.openChild = function (childType) {
        var newBlade = { id: "propertyChild", property: b.currentEntity };
        
        switch (childType) {
            case 'valType':
                newBlade.title = b.origEntity.name ? b.origEntity.name : b.currentEntity.name + ' value type';
                newBlade.subtitle = 'Change value type';
                newBlade.controller = 'virtoCommerce.catalogModule.propertyValueTypeController';
                newBlade.template = 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/property-valueType.tpl.html';
                break;
        }
        bladeNavigationService.showBlade(newBlade, b);
        $scope.currentChild = childType;
    }

    function isDirty() {
        return !angular.equals(b.currentEntity, b.origEntity);
    };

    $scope.cancelChanges = function () {
        angular.copy(b.origEntity, b.currentEntity);
        $scope.bladeClose();
    };

    $scope.saveChanges = function () {
        angular.copy(b.currentEntity, b.origEntity);

        if (b.isNew) {
            b.currentEntities.push(b.origEntity);
        }

        $scope.bladeClose();
    };

    function removeProperty(prop) {
        var dialog = {
            id: "confirmDelete",
            title: "Delete confirmation",
            message: "Are you sure you want to delete Property '" + prop.name + "'?",
            callback: function (remove) {
                if (remove) {
                    var idx = b.currentEntities.indexOf(b.origEntity);
                    b.currentEntities.splice(idx, 1);
                    $scope.bladeClose();
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    b.onClose = function (closeCallback) {
        angular.forEach(b.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });

        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "Save changes",
                message: "The property has been modified. Do you want to save changes?",
                callback: function (needSave) {
                    if (needSave) {
                        $scope.saveChanges();
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
        $scope.formScope = form;
    }

    if (!b.isNew) {
        b.toolbarCommands = [
            {
                name: "Reset", icon: 'fa fa-undo',
                executeMethod: function () {
                    angular.copy(b.origEntity, b.currentEntity);
                },
                canExecuteMethod: function () {
                    return isDirty();
                }
            },
            {
                name: "Delete", icon: 'fa fa-trash-o',
                executeMethod: function () {
                    removeProperty(b.origEntity);
                },
                canExecuteMethod: function () { return true; }
            }
        ];
    }

    $scope.$watch('blade.parentBlade.item.properties', function (data) {
        b.currentEntities = data;
    });

    // actions on load    
    initializeBlade(b.origEntity);
}]);
