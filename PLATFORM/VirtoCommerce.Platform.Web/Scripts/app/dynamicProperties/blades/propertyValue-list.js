angular.module('platformWebApp')
.controller('platformWebApp.propertyValueListController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, bladeNavigationService, dialogService) {
    var blade = $scope.blade;
    blade.headIcon = 'fa-plus-square-o';

    function initializeBlade(dynPropertyValues) {
        var properties = _.pluck(dynPropertyValues, 'property');

        //var selectedProps = _.where(properties, { valueType: 'Decimal' });
        //_.forEach(selectedProps, function (prop) {
        //    prop.value = parseFloat(prop.value);
        //});

        var selectedProps = _.where(properties, { valueType: 'Boolean' });
        _.forEach(selectedProps, function (prop) {
            if (angular.isFunction(prop.value.toLowerCase)) {
                prop.value = prop.value.toLowerCase() === 'true';
            }
        });

        blade.currentEntities = angular.copy(dynPropertyValues);
        blade.origEntity = dynPropertyValues;
        blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals(blade.currentEntities, blade.origEntity);
    };

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    $scope.saveChanges = function () {
        angular.copy(blade.currentEntities, blade.origEntity);
        $scope.bladeClose();
    };

    blade.onClose = function (closeCallback) {
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
        angular.forEach(blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.editProperty = function (node) {
        var newBlade = {
            id: "dynamicPropertyDetail",
            title: node.property.objectType,
            subtitle: 'Manage property',
            origEntity: node,
            confirmChangesFn: function (entry) {
                angular.copy(entry, node);
                $scope.saveChanges();
            },
            deleteFn: function () {
                //var idx = blade.currentEntities.indexOf(node);
                //if (idx >= 0) {
                //    blade.currentEntities.splice(idx, 1);
                //}
                dynamicPropertiesApi.delete({ id: blade.currentEntityId, propertyId: node.id },
                    blade.refresh,
                    function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
            },
            controller: 'platformWebApp.dynamicPropertyDetailController',
            template: 'Scripts/app/dynamicProperties/blades/dynamicProperty-detail.tpl.html'
            //controller: 'virtoCommerce.customerModule.memberPropertyDetailController',
            //template: 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/member-property-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
    
    var formScope;
    $scope.setForm = function (form) {
        formScope = form;
    }
    
    blade.toolbarCommands = [
        {
            name: "Reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(blade.origEntity, blade.currentEntities);
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        }
    ];

    // datepicker
    $scope.datepickers = {
        str: false
    }

    $scope.open = function ($event, which) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.datepickers[which] = true;
    };

    $scope.dateOptions = {
        'year-format': "'yyyy'",
    };


    $scope.$watch('blade.parentBlade.currentEntity.dynamicPropertyValues', initializeBlade);
}]);
