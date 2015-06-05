angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemPropertyListController', ['$scope', 'virtoCommerce.catalogModule.items', 'virtoCommerce.catalogModule.properties', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, items, properties, bladeNavigationService, dialogService) {
    var blade = $scope.blade;

    blade.refresh = function (parentRefresh) {
        items.get({ id: blade.itemId }, function (data) {
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

            blade.item = angular.copy(data);
            blade.origItem = data;
            blade.isLoading = false;
            if (parentRefresh) {
                blade.parentBlade.refresh();
            }
        });
    }

    function isDirty() {
        return !angular.equals(blade.item, blade.origItem);
    };

    function saveChanges() {
        blade.isLoading = true;
        var changes = { id: blade.item.id, properties: blade.item.properties };
        items.updateitem({}, changes, function (data, headers) {
            blade.refresh(true);
        });
    };

    blade.onClose = function (closeCallback) {
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
        if (prop.isManageable) {
            var newBlade = {
                id: 'editCategoryProperty',
                currentEntityId: prop.id,
                title: 'Edit item property',
                subtitle: 'Enter property information',
                controller: 'virtoCommerce.catalogModule.propertyDetailController',
                template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/property-detail.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        } else {
            editUnmanageable({
                title: 'Edit item property',
                origEntity: prop
            });
        }
    };

    function editUnmanageable(bladeData) {
        var newBlade = {
            id: 'editItemProperty',
            subtitle: 'Enter property information',
            controller: 'virtoCommerce.catalogModule.itemPropertyDetailController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-property-detail.tpl.html'
        };
        angular.extend(newBlade, bladeData);

        bladeNavigationService.showBlade(newBlade, blade);
    }

    $scope.getPropValues = function (propId, keyword) {
        return properties.values({ propertyId: propId, keyword: keyword }).$promise.then(function (result) {
            return result;
        });
    };

    var formScope;
    $scope.setForm = function (form) {
        formScope = form;
    }

    blade.toolbarCommands = [
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
                angular.copy(blade.origItem, blade.item);
            },
            canExecuteMethod: function () {
                return isDirty();
            },
            permission: 'catalog:items:manage'
        },
		{
		    name: "Add property", icon: 'fa fa-plus',
		    executeMethod: function () {
		        editUnmanageable({
		            isNew: true,
		            title: 'New item property',
		            origEntity: {
		                type: "Product",
		                valueType: "ShortText",
                        values:[]
		            }
		        });
		    },
		    canExecuteMethod: function () { return true; },
		    permission: 'catalog:items:manage'
		}
    ];

    blade.refresh();
}]);
