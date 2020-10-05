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
            blade.origEntity = angular.copy(settings.allowedValues);
            initializeBlade(settings);
        });
    }

    function initializeBlade(data) {
        if (!data.allowedValues) {
            data.allowedValues = [];
        }

        blade.title = data.title;
        blade.currentEntity = data;
        currentEntities = blade.currentEntity.allowedValues;
        blade.isLoading = false;
    }

    $scope.dictValueValidator = function (value) {
        if (blade.currentEntity) {
            if (blade.currentEntity.valueType == 'ShortText') {
                return _.all(currentEntities, function (item) { return angular.lowercase(item.value) !== angular.lowercase(value); });
            } else {
                return _.all(currentEntities, function (item) { return item.value !== value; });
            }
        }
        return false;
    };

    $scope.add = function (form) {
        if (form.$valid) {
            currentEntities.push($scope.newValue);
            resetNewValue();
            form.$setPristine();
        }
    };

    $scope.delete = function (index) {
        currentEntities.splice(index, 1);
        $scope.selectedItem = undefined;
    };

    $scope.selectItem = function (listItem) {
        $scope.selectedItem = listItem;
    };

    blade.headIcon = 'fa-wrench';
    blade.subtitle = 'platform.blades.setting-dictionary.subtitle';
    blade.toolbarCommands = [{
        name: "platform.commands.delete", icon: 'fa fa-trash-o',
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
            return !angular.equals(currentEntities, blade.origEntity) && blade.hasUpdatePermission();
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
            icon: 'fa fa-save',
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
                angular.copy(blade.origEntity, currentEntities);
                blade.selectedAll = false;
            },
            canExecuteMethod: isDirty,
        });
        blade.refresh();
    } else {
        $scope.$watch('blade.parentBlade.currentEntities', function (data) {
            if (data) {
                var allEntities = _.flatten(_.map(data, _.values));
                initializeBlade(_.findWhere(allEntities, { name: blade.currentEntityId }));
            }
        });
    }

    $scope.checkAll = function () {
        angular.forEach(currentEntities, function (item) {
            item._selected = blade.selectedAll;
        });
    };

    function resetNewValue() {
        $scope.newValue = { value: null };
    }

    function isItemsChecked() {
        return _.any(currentEntities, function (x) { return x._selected; });
    }

    function deleteChecked() {
        var selection = _.where(currentEntities, { _selected: true });
        angular.forEach(selection, function (listItem) {
            $scope.delete(currentEntities.indexOf(listItem));
        });
    }

    // on load
    resetNewValue();
}]);
