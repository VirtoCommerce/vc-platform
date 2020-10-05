angular.module('platformWebApp').controller('platformWebApp.propertyDictionaryController', ['$scope', 'platformWebApp.dialogService', 'platformWebApp.bladeNavigationService', 'platformWebApp.settings', 'platformWebApp.dynamicProperties.dictionaryItemsApi', function ($scope, dialogService, bladeNavigationService, settings, dictionaryItemsApi) {
    var blade = $scope.blade;
    blade.updatePermission = 'platform:dynamic_properties:update';
    blade.headIcon = 'fa-plus-square-o';
    blade.title = 'platform.blades.property-dictionary.title';
    blade.subtitle = 'platform.blades.property-dictionary.subtitle';

    var availableLanguages;

    function refresh() {
        blade.isLoading = true;
        blade.selectedAll = false;

        if (blade.isApiSave) {
            dictionaryItemsApi.query({ id: blade.currentEntity.objectType, propertyId: blade.currentEntity.id },
                initializeBlade,
                function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        } else {
            initializeBlade(blade.data);
        }
    }

    function initializeBlade(data) {
        blade.origEntity = data;
        blade.currentEntities = angular.copy(data);

        if (blade.currentEntity.isMultilingual && !availableLanguages) {
            settings.getValues({ id: 'VirtoCommerce.Core.General.Languages' }, function (promiseData) {
                availableLanguages = _.map(promiseData.sort(), function (x) { return { locale: x }; });
                resetNewValue();
                blade.isLoading = false;
            },
                function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        } else {
            resetNewValue();
            blade.isLoading = false;
        }
    }

    $scope.dictItemNameValidator = function (value) {
        if (blade.isLoading) {
            return false;
        } else if (blade.currentEntity.isMultilingual) {
            var testEntity = angular.copy($scope.newValue);
            testEntity.name = value;
            return _.all(blade.currentEntities, function (item) {
                return item.name !== value || $scope.selectedItem === item;
            });
        } else {
            return _.all(blade.currentEntities, function (item) { return item.name !== value; });
        }
    };

    $scope.dictValueValidator = function (value, editEntity) {
        if (blade.currentEntity.isMultilingual) {
            var testEntity = angular.copy(editEntity);
            testEntity.value = value;
            return _.all(blade.currentEntities, function (item) {
                return item.value !== value || item.locale !== testEntity.locale || ($scope.selectedItem && _.some($scope.selectedItem.displayNames, function (x) {
                    return angular.equals(x, testEntity);
                }));
            });
        } else {
            return _.all(blade.currentEntities, function (item) { return item.name !== value; });
        }
    };

    $scope.selectItem = function (listItem) {
        $scope.selectedItem = listItem;

        if (blade.currentEntity.isMultilingual) {
            resetNewValue();
        }
    };

    function resetNewValue() {
        if (blade.currentEntity.isMultilingual) {
            // generate input fields for ALL languages
            var newValue = { displayNames: angular.copy(availableLanguages) };

            // add current values
            if ($scope.selectedItem) {
                _.each($scope.selectedItem.displayNames, function (value) {
                    var foundValue = _.findWhere(newValue.displayNames, { locale: value.locale });
                    if (foundValue) {
                        angular.extend(foundValue, value);
                    }
                });
                newValue.id = $scope.selectedItem.id;
                newValue.name = $scope.selectedItem.name;
            }

            $scope.newValue = newValue;
        } else {
            $scope.newValue = {};
        }
    }

    $scope.cancel = function () {
        $scope.selectedItem = undefined;
        resetNewValue();
    }

    $scope.add = function (form) {
        if (form.$valid) {
            if ($scope.newValue.displayNames) {
                $scope.newValue.displayNames = _.filter($scope.newValue.displayNames, function (x) { return x.name; });

                if ($scope.selectedItem) { // editing existing value
                    angular.copy($scope.newValue, $scope.selectedItem);
                    $scope.selectedItem = undefined;
                } else { // adding new value
                    blade.currentEntities.push($scope.newValue);
                }
            } else {
                blade.currentEntities.push($scope.newValue);
            }
            resetNewValue();
            form.$setPristine();
        }
    };

    $scope.delete = function (index) {
        blade.selectedAll = false;
        $scope.toggleAll();

        blade.currentEntities[index].$selected = true;
        deleteChecked();
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    function isDirty() {
        return !angular.equals(blade.currentEntities, blade.origEntity) && blade.hasUpdatePermission();
    }

    $scope.saveChanges = function () {
        if (blade.isApiSave) {
            blade.isLoading = true;

            dictionaryItemsApi.save({ id: blade.currentEntity.objectType, propertyId: blade.currentEntity.id },
                blade.currentEntities,
                function () {
                    refresh();
                    if (blade.onChangesConfirmedFn)
                        blade.onChangesConfirmedFn();
                },
                function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        } else {
            blade.onChangesConfirmedFn(blade.currentEntities);
            $scope.bladeClose();
        }
    };

    blade.toolbarCommands = [
        {
            name: "platform.commands.save", icon: 'fa fa-save',
            executeMethod: function () {
                $scope.saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty() && $scope.formScope && $scope.formScope.$valid;
            },
            permission: blade.updatePermission
        },
        {
            name: "platform.commands.reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(blade.origEntity, blade.currentEntities);
            },
            canExecuteMethod: isDirty,
            permission: blade.updatePermission
        },
        {
            name: "platform.commands.delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                deleteChecked();
            },
            canExecuteMethod: function () {
                return isItemsChecked();
            },
            permission: blade.updatePermission
        }
    ];

    if (!blade.isApiSave) {
        $scope.blade.toolbarCommands.splice(0, 1); // remove save button
    }

    $scope.toggleAll = function () {
        angular.forEach(blade.currentEntities, function (item) {
            item.$selected = blade.selectedAll;
        });
    };

    function isItemsChecked() {
        return _.any(blade.currentEntities, function (x) { return x.$selected; });
    }

    function deleteChecked() {
        var selection = _.where(blade.currentEntities, { $selected: true });
        var ids = _.pluck(selection, 'id');
        var dialog = {
            id: "confirmDeleteItem",
            title: "platform.dialogs.dictionary-items-delete.title",
            message: "platform.dialogs.dictionary-items-delete.message",
            messageValues: { quantity: ids.length },
            callback: function (remove) {
                if (remove) {
                    blade.isLoading = true;
                    dictionaryItemsApi.delete({ id: blade.currentEntity.objectType, propertyId: blade.currentEntity.id, ids: ids }, null,
                        function () {
                            refresh();
                            if (blade.onChangesConfirmedFn)
                                blade.onChangesConfirmedFn();
                        },
                        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    // on load
    refresh();
}]);
