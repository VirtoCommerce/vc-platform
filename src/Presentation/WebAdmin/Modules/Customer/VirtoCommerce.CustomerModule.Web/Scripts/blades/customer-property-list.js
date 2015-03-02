angular.module('virtoCommerce.customerModule.blades')
.controller('customerPropertyListController', ['$scope', 'customers', 'bladeNavigationService', 'dialogService', function ($scope, customers, bladeNavigationService, dialogService) {
    $scope.blade.origEntity = {};

    $scope.blade.refresh = function (parentRefresh) {
        if (parentRefresh) {
            $scope.blade.parentBlade.refresh();
            //} else {
            //    customers.get({ id: $scope.blade.currentEntityId }, function (data) {
            //        initializeBlade(data.properties);
            //    });
        }
    };

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

    function saveChanges() {
        $scope.blade.isLoading = true;
        customers.update({}, { id: $scope.blade.currentEntityId, properties: $scope.blade.currentEntities }, function () {
            $scope.blade.refresh(true);
        });
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

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.editProperty = function (prop) {
        var newBlade = {
            id: 'editCustomerProperty',
            origEntity: prop,
            title: 'Edit customer property',
            subtitle: 'Enter property information',
            controller: 'customerPropertyDetailController',
            template: 'Modules/Customer/VirtoCommerce.CustomerModule.Web/Scripts/blades/customer-property-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    var formScope;
    $scope.setForm = function (form) {
        formScope = form;
    }

    $scope.bladeToolbarCommands = [
        {
            name: "Add property", icon: 'fa fa-plus',
            executeMethod: function () {
                var prop = { isNew: true, valueType: 'ShortText' }
                var newBlade = {
                    id: 'editCustomerProperty',
                    origEntity: prop,
                    title: 'New property',
                    subtitle: 'Enter property information',
                    controller: 'customerPropertyDetailController',
                    template: 'Modules/Customer/VirtoCommerce.CustomerModule.Web/Scripts/blades/customer-property-detail.tpl.html'
                };

                bladeNavigationService.showBlade(newBlade, $scope.blade);
            },
            canExecuteMethod: function () {
                return true;
            }
        },
		{
		    name: "Save", icon: 'fa fa-save',
		    executeMethod: function () {
		        saveChanges();
		    },
		    canExecuteMethod: function () {
		        return isDirty() && formScope && formScope.$valid;
		    }
		},
        {
            name: "Reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntities);
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        }
    ];

    $scope.blade.isLoading = false;
    $scope.$watch('blade.parentBlade.currentEntity.properties', function (currentEntities) {
        initializeBlade(currentEntities);
    });
}]);
