angular.module('platformWebApp')
.controller('platformWebApp.dynamicPropertyDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.settings', 'platformWebApp.dynamicProperties.api', function ($scope, bladeNavigationService, dialogService, settings, dynamicPropertiesApi) {
    var blade = $scope.blade;
    blade.headIcon = 'fa-plus-square-o';

    function refresh() {
        if (blade.isNew) {
            initializeBlade({ valueType: 'ShortText', displayNames: [] });
        } else {
            initializeBlade(blade.origEntity);
        }
    }

    function initializeBlade(data) {
        if (data.isMultilingual || blade.isNew) {
            // load all languages and generate missing value wrappers
            settings.getValues({ id: 'VirtoCommerce.Core.General.Languages' }, function (promiseData) {
                promiseData.sort();

                // add missing languages
                _.each(promiseData, function (x) {
                    if (_.all(data.displayNames, function (dn) { return dn.locale.toLowerCase() !== x.toLowerCase(); })) {
                        data.displayNames.push({ locale: x });
                    }
                });

                initializeBlade2(data);
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        } else {
            initializeBlade2(data);
        }
    }

    function initializeBlade2(data) {
        blade.origEntity = data;
        blade.currentEntity = angular.copy(data);
        blade.isLoading = false;
    }

    $scope.arrayFlagValidator = function (value) {
        return !value || blade.currentEntity.valueType === 'ShortText' || blade.currentEntity.valueType === 'Integer' || blade.currentEntity.valueType === 'Decimal';
    };

    $scope.multilingualFlagValidator = function (value) {
        return !value || blade.currentEntity.valueType === 'ShortText' || blade.currentEntity.valueType === 'LongText';
    };

    $scope.openChild = function (childType) {
        var newBlade = {
            id: "propertyChild",
            currentEntity: blade.currentEntity
        };

        switch (childType) {
            case 'valType':
                newBlade.title = 'Dynamic property value type';
                newBlade.subtitle = 'Change value type';
                newBlade.controller = 'platformWebApp.propertyValueTypeController';
                newBlade.template = 'Scripts/app/dynamicProperties/blades/property-valueType.tpl.html';
                break;
            case 'dict':
                newBlade.title = 'Dictionary values';
                newBlade.subtitle = 'Manage dictionary values';
                newBlade.controller = 'platformWebApp.propertyDictionaryController';
                newBlade.template = 'Scripts/app/dynamicProperties/blades/property-dictionary.tpl.html';
                break;
        }
        bladeNavigationService.showBlade(newBlade, blade);
        $scope.currentChild = childType;
    }

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity);
    };

    $scope.saveChanges = function () {
        if (blade.currentEntity.isMultilingual) {
            blade.currentEntity.displayNames = _.filter(blade.currentEntity.displayNames, function (x) { return x.name; });
        } else {
            blade.currentEntity.displayNames = undefined;
        }

        blade.confirmChangesFn(blade.currentEntity);
        $scope.bladeClose();
    };

    function deleteEntry() {
        var dialog = {
            id: "confirmDelete",
            title: "Delete confirmation",
            message: "Are you sure you want to delete this dynamic property?",
            callback: function (remove) {
                if (remove) {
                    blade.deleteFn();
                    $scope.bladeClose();
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    if (!blade.isNew) {
        blade.toolbarCommands = [
        {
            name: "Save", icon: 'fa fa-save',
            executeMethod: function () {
                $scope.saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty() && $scope.formScope && $scope.formScope.$valid;
            }
        },
        {
            name: "Reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(blade.origEntity, blade.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        },
        {
            name: "Delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                deleteEntry();
            },
            canExecuteMethod: function () {
                return !isDirty() && !blade.isNew;
            }
        }
        ];
    }

    // on load: 
    refresh();
}]);