angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemPropertyDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, bladeNavigationService, dialogService) {
    var blade = $scope.blade;
    blade.updatePermission = 'catalog:update';
    $scope.currentChild = undefined;

    function initializeBlade(data) {
        if (data.valueType === 'Number' && data.dictionaryValues) {
            _.forEach(data.dictionaryValues, function (entry) {
                entry.value = parseFloat(entry.value);
            });
        }

        blade.currentEntity = angular.copy(data);
        //blade.origEntity = data;
        blade.isLoading = false;
    };

    $scope.openChild = function (childType) {
        var newBlade = { id: "propertyChild", property: blade.currentEntity };

        switch (childType) {
            case 'valType':
                newBlade.title = 'catalog.blades.property-valueType.title';
                newBlade.titleValues = { name: blade.origEntity.name ? blade.origEntity.name : blade.currentEntity.name };
                newBlade.subtitle = 'catalog.blades.property-valueType.subtitle';
                newBlade.controller = 'virtoCommerce.catalogModule.propertyValueTypeController';
                newBlade.template = 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/property-valueType.tpl.html';
                break;
        }
        bladeNavigationService.showBlade(newBlade, blade);
        $scope.currentChild = childType;
    }

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
    };

    $scope.cancelChanges = function () {
        angular.copy(blade.origEntity, blade.currentEntity);
        $scope.bladeClose();
    };

    $scope.saveChanges = function () {
        angular.copy(blade.currentEntity, blade.origEntity);

        if (blade.isNew) {
            blade.currentEntities.push(blade.origEntity);
        }

        $scope.bladeClose();
    };

    function removeProperty(prop) {
        var dialog = {
            id: "confirmDelete",
            title: "catalog.dialogs.property-delete.title",
            message: 'catalog.dialogs.property-delete.message',
            messageValues: { name: prop.name },
            callback: function (remove) {
                if (remove) {
                    var idx = blade.currentEntities.indexOf(blade.origEntity);
                    blade.currentEntities.splice(idx, 1);
                    $scope.bladeClose();
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), $scope.formScope && $scope.formScope.$valid, blade, $scope.saveChanges, closeCallback, "catalog.dialogs.property-save.title", "catalog.dialogs.property-save.message");
    };

    $scope.setForm = function (form) { $scope.formScope = form; };

    if (!blade.isNew) {
        blade.headIcon = 'fa-gear';
        blade.toolbarCommands = [
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
                canExecuteMethod: function () { return true; }
            }
        ];
    }

    $scope.$watch('blade.parentBlade.item.properties', function (data) {
        blade.currentEntities = data;
    });

    // actions on load    
    initializeBlade(blade.origEntity);
}]);
