angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemPropertyController', ['$scope', 'virtoCommerce.catalogModule.items', 'virtoCommerce.catalogModule.properties', 'bladeNavigationService', 'dialogService', function ($scope, items, properties, bladeNavigationService, dialogService) {
    $scope.blade.origItem = {};
    $scope.blade.item = {};

    $scope.blade.refresh = function (parentRefresh) {
        items.get({ id: $scope.blade.itemId }, function (data) {
            if (data.properties) {
                var numberProps = _.where(data.properties, { valueType: 'Number', multivalue: false, dictionary: false });
                _.forEach(numberProps, function (prop) {
                    _.forEach(prop.values, function (value) {
                        value.value = parseFloat(value.value);
                    });
                });
            }
            //if (data.titularItemId != null) {
            //    $scope.propGroups = [{ title: 'Variation properties', type: 1 }];
            //} else {
            //    $scope.propGroups = [{ title: 'Product properties', type: 0 }];
            //}

            $scope.blade.item = angular.copy(data);
            $scope.blade.origItem = data;
            $scope.blade.isLoading = false;
            if (parentRefresh) {
                $scope.blade.parentBlade.refresh();
            }
        });
    }

    function isDirty() {
        return !angular.equals($scope.blade.item, $scope.blade.origItem);
    };

    function saveChanges() {
        $scope.blade.isLoading = true;
        var changes = { id: $scope.blade.item.id, properties: $scope.blade.item.properties };
        items.updateitem({}, changes, function (data, headers) {
            $scope.blade.refresh(true);
        });
    };

    $scope.blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "Save changes",
                message: "The item has been modified. Do you want to save changes?"
            };
            dialog.callback = function (needSave) {
                if (needSave) {
                    saveChanges();
                }
                closeCallback();
            };
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeCallback();
        }
    };

    $scope.editProperty = function (prop) {
        var newBlade = {
            id: 'editCategoryProperty',
            currentEntityId: prop.id,
            title: 'Edit category property',
            subtitle: 'enter property information',
            controller: 'virtoCommerce.catalogModule.propertyDetailController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/property-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.getPropValues = function (propId, keyword) {
        return properties.values({ propertyId: propId, keyword: keyword }).$promise.then(function (result) {
            return result;
        });
    };

    var formScope;
    $scope.setForm = function (form) {
        formScope = form;
    }

    $scope.bladeToolbarCommands = [
		{
		    name: "Save", icon: 'fa fa-save',
		    executeMethod: function () {
		        saveChanges();
		    },
		    canExecuteMethod: function () {
		        return isDirty() && formScope && formScope.$valid;
		    },
		    permission: 'catalog:items:manage'
		},
        {
            name: "Reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.origItem, $scope.blade.item);
            },
            canExecuteMethod: function () {
                return isDirty();
            },
            permission: 'catalog:items:manage'
        }
    ];

    $scope.blade.refresh();
}]);
