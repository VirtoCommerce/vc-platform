angular.module('platformWebApp').controller('platformWebApp.settingDictionaryController', ['$scope', 'platformWebApp.dialogService', 'platformWebApp.bladeNavigationService', 'platformWebApp.settings', function ($scope, dialogService, bladeNavigationService, settingsApi) {
    var blade = $scope.blade;
    blade.updatePermission = 'platform:setting:update';
    var currentEntities;

    blade.refresh = function (parentRefresh) {
        settingsApi.get({ id: blade.currentEntityId }, function (settings) {
            if (parentRefresh && blade.parentRefresh) {
                blade.parentRefresh(settings.allowedValues);
            }

            settings.allowedValues = _.map(settings.allowedValues, function (x) { return { value: x }; });
            initializeBlade(settings, { allowedValues: angular.copy(settings.allowedValues) });
        });
    }

    function initializeBlade(data, origData) {
        if (!data.allowedValues) {
            data.allowedValues = [];
        }

        if (!origData.allowedValues) {
            origData.allowedValues = [];
        }

        angular.forEach(data.allowedValues, function (item) {
            if (!item.value) {
                item.value = ""; // Small trick to avoid hang on null value
            }
        });

        angular.forEach(origData.allowedValues, function (item) {
            if (!item.value) {
                item.value = "";
            }
        });

        blade.title = `settings.${data.name}.title`;
        blade.currentEntity = data;
        blade.origEntity = origData;
        blade.searchText = "";
        currentEntities = blade.currentEntity.allowedValues;
        $scope.applyOrder();
        blade.isLoading = false;
    }

    $scope.validateDictValue = function (value) {
        if (blade.currentEntity) {
            if (blade.currentEntity.valueType == 'ShortText') {
                return _.all(currentEntities, function (item) { return angular.lowercase(item.value) !== angular.lowercase(value); });
            } else {
                return _.all(currentEntities, function (item) { return item.value !== value; });
            }
        }
        return false;
    };

    $scope.filteredEntities = function () {
        var lowerCasedSearchText = blade.searchText.toLowerCase();
        return _.filter(blade.currentEntity.allowedValues, function (o) { return !o.value || o.value.toLowerCase().includes(lowerCasedSearchText); });
    };

    $scope.delete = function (index) {
        currentEntities.splice(index, 1);
        $scope.selectedItem = undefined;
    };

    $scope.selectItem = function (listItem) {
        if (!$scope.inApply) {
            if ($scope.selectedItem && !$scope.selectedItem.value) {
                // Remove valueless items
                $scope.delete(currentEntities.indexOf($scope.selectedItem));
            }
            if (listItem) {
                $scope.editValue = angular.copy(listItem);
            }
            $scope.error = false;
            $scope.selectedItem = listItem;
            setTimeout(() => $('#dictValue').focus());
        }
    };

    blade.headIcon = 'fa fa-wrench';
    blade.subtitle = 'platform.blades.setting-dictionary.subtitle';
    blade.toolbarCommands = [{
        name: "platform.commands.add", icon: 'menu-ico fas fa-plus',
        executeMethod: function () {
            addNew();
        },
        canExecuteMethod: function () {
            return true;
        }
    },
    {
        name: "platform.commands.delete", icon: 'menu-ico fas fa-trash-alt',
        executeMethod: function () {
            deleteChecked();
        },
        canExecuteMethod: function () {
            return isItemsChecked();
        }
    }];

    if (blade.isApiSave) {
        var formScope;
        $scope.setForm = function (form) {
            formScope = form;
        }

        function isDirty() {
            return blade.origEntity && !angular.equals(currentEntities, blade.origEntity.allowedValues) && blade.hasUpdatePermission();
        }

        function saveChanges() {
            blade.selectedAll = false;
            blade.isLoading = true;
            blade.currentEntity.allowedValues = _.pluck(blade.currentEntity.allowedValues, 'value');

            settingsApi.update(null, [blade.currentEntity], blade.refresh, function (error) {
                bladeNavigationService.setError('Error ' + error.status, $scope.blade);
            });
        }

        blade.toolbarCommands.splice(0, 0, {
            name: "platform.commands.save",
            icon: 'fas fa-save',
            executeMethod: function () {
                saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty() && formScope && formScope.$valid;
            }
        }, {
            name: "platform.commands.reset",
            icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(blade.origEntity.allowedValues, currentEntities);
                blade.selectedAll = false;
            },
            canExecuteMethod: isDirty,
        });
        blade.refresh();
    } else {
        $scope.$watch('blade.parentBlade.currentEntities', function (data) {
            if (data) {
                var allEntities = _.flatten(_.map(data, _.values));
                var origEntities = _.flatten(_.map(blade.parentBlade.origEntity, _.values));
                initializeBlade(_.findWhere(allEntities, { name: blade.currentEntityId }), _.findWhere(origEntities, { name: blade.currentEntityId }));
            }
        });
    }

    $scope.checkAll = function () {
        angular.forEach(currentEntities, function (item) {
            // use a field with $dollar-sign prefix to hide such field from angular.equals to avoid "miss-dirtying" issues
            item.$selected = blade.selectedAll;
        });
    };

    $scope.applyOrder = function () {
        // Order both: current and source arrays to avoid issues with angular.equals (like "miss-dirtying")
        orderBy(currentEntities);
        orderBy(blade.origEntity.allowedValues);
    };

    $scope.applyValue = function (apply) {
        // Check the value has no duplicates
        $scope.error = !$scope.validateDictValue($scope.editValue.value);        
        if ($scope.error) { 
            $scope.inApply = apply;
            setTimeout(() => {
                $('#dictValue').focus();
                $scope.inApply = false;
            });
        }
        else {
            $scope.selectedItem.value = $scope.editValue.value;
            $scope.selectItem(null);
            $scope.applyOrder();
        }
    };

    $scope.inputKeyUp = function ($event) {
        if ($event.keyCode === 13) {
            // Apply value on hit Enter
            $scope.applyValue();
        }
        if ($event.keyCode === 27) {
            // Decline value on hit Esc
            $scope.selectItem(null);
        }
    };

    function orderBy(entities) {
        if (blade.currentEntity.valueType === 'Number') {
            entities.sort(function (a, b) { return a.value - b.value; })
        }
        else {
            entities.sort(function (a, b) { return a.value.localeCompare(b.value); })
        }
        if (blade.orderDesc) {
            entities.reverse();
        }

    }

    function resetNewValue() {
        $scope.newValue = { value: null };
    }

    function isItemsChecked() {
        return _.any(currentEntities, function (x) { return x.$selected; });
    }

    function deleteChecked() {
        var selection = _.where(currentEntities, { $selected: true });
        angular.forEach(selection, function (listItem) {
            $scope.delete(currentEntities.indexOf(listItem));
        });
    }

    function addNew() {
        $scope.inApply = false;
        currentEntities.splice(0, 0, $scope.newValue);
        $scope.selectItem($scope.newValue);
        resetNewValue();
    }

    // on load
    resetNewValue();
}]);
