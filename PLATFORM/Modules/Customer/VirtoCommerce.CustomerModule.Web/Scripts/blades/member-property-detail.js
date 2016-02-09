angular.module('virtoCommerce.customerModule')
.controller('virtoCommerce.customerModule.memberPropertyDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, bladeNavigationService, dialogService) {
    var b = $scope.blade;

    function initializeBlade(data) {
        b.currentEntity = angular.copy(data);
        // b.origEntity = data;
        b.isLoading = false;
    };

    $scope.blade.currentChild = undefined;

    $scope.openChild = function (childType) {
        var newBlade = { id: "customerPropertyChild" };

        switch (childType) {
            case 'valType':
                newBlade.title = 'customer.blades.member-property-valueType.title';
                newBlade.titleValues = { name: b.origEntity.name ? b.origEntity.name : b.currentEntity.name };
                newBlade.subtitle = 'customer.blades.member-property-valueType.subtitle';
                newBlade.valueType = b.currentEntity.valueType
                newBlade.controller = 'virtoCommerce.customerModule.memberPropertyValueTypeController';
                newBlade.template = 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/member-property-valueType.tpl.html';
                break;
        }
        bladeNavigationService.showBlade(newBlade, $scope.blade);
        $scope.blade.currentChild = childType;
    }

    function isDirty() {
        return !angular.equals(b.currentEntity, b.origEntity);
    };
    
    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    $scope.saveChanges = function () {
        if ($scope.blade.currentEntity.isNew) {
            $scope.blade.currentEntity.isNew = undefined;
            $scope.blade.parentBlade.currentEntities.push($scope.blade.currentEntity);
        }

        angular.copy($scope.blade.currentEntity, $scope.blade.origEntity);
        $scope.bladeClose();
    };

    function removeProperty(prop) {
        var dialog = {
            id: "confirmDelete",
            title: "customer.dialogs.property-delete.title",
            message: "customer.dialogs.property-delete.message",
            messageValues: { name: prop.name },
            callback: function (remove) {
                if (remove) {
                    var idx = $scope.blade.parentBlade.currentEntities.indexOf(prop);
                    if (idx >= 0) {
                        $scope.blade.parentBlade.currentEntities.splice(idx, 1);
                        $scope.bladeClose();
                    }
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
                title: "customer.dialogs.property-save.title",
                message: "customer.dialogs.property-save.message",
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

    $scope.blade.headIcon = 'fa-user';

    $scope.blade.toolbarCommands = [
        {
            name: "platform.commands.reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(b.origEntity, b.currentEntity);
            },
            canExecuteMethod: isDirty,
            permission: 'customer:update'
        },
		{
		    name: "platform.commands.delete", icon: 'fa fa-trash-o',
		    executeMethod: function () {
		        removeProperty(b.origEntity);
		    },
		    canExecuteMethod: function () {
		        return !(b.origEntity.isNew || isDirty());
		    },
		    permission: 'customer:update'
		}
    ];

    // actions on load    
    initializeBlade($scope.blade.origEntity);
}]);
