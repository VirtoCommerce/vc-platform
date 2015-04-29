angular.module('virtoCommerce.customerModule')
.controller('virtoCommerce.customerModule.memberPropertyListController', ['$scope', 'bladeNavigationService', 'dialogService', function ($scope, bladeNavigationService, dialogService) {
    $scope.blade.origEntity = {};

    function initializeBlade(properties) {
        var numberProps = _.where(properties, { valueType: 'Decimal' });
        _.forEach(numberProps, function (prop) {
            prop.value = parseFloat(prop.value);
        });

        $scope.blade.currentEntities = angular.copy(properties);
        $scope.blade.origEntity = properties;
        $scope.blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals($scope.blade.currentEntities, $scope.blade.origEntity);
    };

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    $scope.saveChanges = function () {
        angular.copy($scope.blade.currentEntities, $scope.blade.data);
        angular.copy($scope.blade.currentEntities, $scope.blade.origEntity);
        $scope.bladeClose();
    };

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();

        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "Save changes",
                message: "The properties has been modified. Do you want to save changes?",
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

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.editProperty = function (prop) {
        openBlade(prop, 'Edit property');
    };

    function openBlade(data, title) {
        var newBlade = {
            id: 'editCustomerProperty',
            origEntity: data,
            title: title,
            subtitle: 'Enter property information',
            controller: 'virtoCommerce.customerModule.memberPropertyDetailController',
            template: 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/member-property-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    var formScope;
    $scope.setForm = function (form) {
        formScope = form;
    }

    $scope.bladeHeadIco = 'fa fa-user';

    $scope.bladeToolbarCommands = [
        {
            name: "Add property", icon: 'fa fa-plus',
            executeMethod: function () {
                var prop = { isNew: true, valueType: 'ShortText' }
                openBlade(prop, 'New property');
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'customer:manage'
        },
        {
            name: "Reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntities);
            },
            canExecuteMethod: function () {
                return isDirty();
            },
            permission: 'customer:manage'
        }
    ];

    $scope.blade.isLoading = false;
    $scope.$watch('blade.parentBlade.currentEntity.properties', function (currentEntities) {
        initializeBlade(currentEntities);
    });
}]);
