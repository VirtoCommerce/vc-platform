angular.module('platformWebApp')
.controller('platformWebApp.propertyValueListController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.settings', 'platformWebApp.dynamicProperties.dictionaryItemsApi', function ($scope, bladeNavigationService, dialogService, settings, dictionaryItemsApi) {
    var blade = $scope.blade;
    blade.updatePermission = 'platform:dynamic_properties:update';
    blade.headIcon = 'fa-plus-square-o';
    blade.title = "platform.blades.propertyValue-list.title";
    blade.subtitle = "platform.blades.propertyValue-list.subtitle";

    blade.refresh = function () {
        settings.getValues({ id: 'VirtoCommerce.Core.General.Languages' }, function (data) {
            $scope.languages = data;
        });

        blade.data = blade.currentEntity;

        var rawProperties = angular.copy(blade.currentEntity.dynamicProperties);
        _.each(rawProperties, function (x) {
            x.values.sort(function (a, b) {
                return a.value.name ? a.value.name.localeCompare(b.value.name) : a.value.localeCompare(b.value);
            });
        });

        blade.origEntity = rawProperties;
        blade.currentEntities = angular.copy(rawProperties);
        blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals(blade.currentEntities, blade.origEntity) && blade.hasUpdatePermission();
    }

    function canSave() {
        return isDirty() && formScope && formScope.$valid;
    }

    $scope.cancelChanges = function () {
        angular.copy(blade.origEntity, blade.currentEntities);
        $scope.bladeClose();
    };

    $scope.saveChanges = function () {
        if (isDirty()) {
            angular.copy(blade.currentEntities, blade.data.dynamicProperties);
            angular.copy(blade.currentEntities, blade.origEntity);
        }
        $scope.bladeClose();
    };

    var formScope;
    $scope.setForm = function (form) { formScope = form; }

    $scope.editDictionary = function (property) {
        var newBlade = {
            id: "propertyDictionary",
            isApiSave: true,
            currentEntity: property,
            controller: 'platformWebApp.propertyDictionaryController',
            template: '$(Platform)/Scripts/app/dynamicProperties/blades/property-dictionary.tpl.html',
            onChangesConfirmedFn: function () {
                blade.currentEntities = angular.copy(blade.currentEntities);
            }
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, $scope.saveChanges, closeCallback, "platform.dialogs.properties-save.title", "platform.dialogs.properties-save.message");
    };

    blade.toolbarCommands = [
        {
            name: "platform.commands.reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(blade.origEntity, blade.currentEntities);
            },
            canExecuteMethod: isDirty
        },
		{
		    name: "platform.commands.manage-type-properties", icon: 'fa fa-edit',
		    executeMethod: function () {
		        var newBlade = {
		            id: 'dynamicPropertyList',
		            objectType: blade.data.objectType,
		            controller: 'platformWebApp.dynamicPropertyListController',
		            template: '$(Platform)/Scripts/app/dynamicProperties/blades/dynamicProperty-list.tpl.html'
		        };
		        bladeNavigationService.showBlade(newBlade, blade);
		    },
		    canExecuteMethod: function () {
		        return angular.isDefined(blade.data.objectType);
		    }
		}
    ];

    $scope.getDictionaryValues = function (property, callback) {
        dictionaryItemsApi.query({ id: property.objectType, propertyId: property.id }, callback);
    }

    blade.refresh();
}]);
