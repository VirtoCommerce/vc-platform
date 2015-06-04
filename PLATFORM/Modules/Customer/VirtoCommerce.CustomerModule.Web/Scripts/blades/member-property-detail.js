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
                newBlade.title = b.origEntity.name ? b.origEntity.name : b.currentEntity.name + ' value type';
                newBlade.subtitle = 'Change value type';
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
            title: "Delete confirmation",
            message: "Are you sure you want to delete Property '" + prop.name + "'?",
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

    $scope.blade.headIcon = 'fa fa-user';

    $scope.blade.toolbarCommands = [
        {
            name: "Reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(b.origEntity, b.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            },
            permission: 'customer:manage'
        },
		{
		    name: "Delete", icon: 'fa fa-trash-o',
		    executeMethod: function () {
		        removeProperty(b.origEntity);
		    },
		    canExecuteMethod: function () {
		        return !(b.origEntity.isNew || isDirty());
		    },
		    permission: 'customer:manage'
		}
    ];

    // actions on load    
    initializeBlade($scope.blade.origEntity);
}]);
