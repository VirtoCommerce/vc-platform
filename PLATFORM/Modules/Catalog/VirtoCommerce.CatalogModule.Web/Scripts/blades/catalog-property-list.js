angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.catalogPropertyListController', ['$scope', 'virtoCommerce.catalogModule.catalogs', 'virtoCommerce.catalogModule.properties', 'platformWebApp.bladeNavigationService', function ($scope, catalogs, properties, bladeNavigationService) {
    var blade = $scope.blade;
    blade.updatePermission = 'catalog:update';
    blade.origEntity = {};

    blade.refresh = function (parentRefresh) {
        catalogs.get({ id: blade.currentEntityId }, function (data) {
            initializeBlade(data);
            if (parentRefresh) {
                blade.parentBlade.refresh();
            }
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    };

    function initializeBlade(data) {
        if (data.properties) {
            var selection = _.where(data.properties, { valueType: 'Number', multivalue: false, dictionary: false });
            _.forEach(selection, function (prop) {
                _.forEach(prop.values, function (value) {
                    value.value = parseFloat(value.value);
                });
            });

            selection = _.where(data.properties, { valueType: 'Boolean' });
            _.forEach(selection, function (prop) {
                _.forEach(prop.values, function (value) {
                    value.value = value.value && value.value.toLowerCase() === 'true';
                });
            });
        }

        blade.currentEntity = angular.copy(data);
        blade.origEntity = data;
        blade.title = data.name;
        blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
    };

    function canSave() {
        return isDirty() && formScope && formScope.$valid;
    }

    function saveChanges() {
        blade.isLoading = true;
        catalogs.update({}, blade.currentEntity, function (data, headers) {
            blade.refresh(true);
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    };

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, saveChanges, closeCallback, "catalog.dialogs.catalog-save.title", "catalog.dialogs.catalog-save.message");
    };

    $scope.editProperty = function (prop) {
        var newBlade = {
            id: 'editCatalogProperty',
            currentEntityId: prop.id,
            catalogId: blade.currentEntity.id,
            title: 'catalog.blades.property-detail.title-catalog',
            subtitle: 'catalog.blades.property-detail.subtitle-catalog',
            controller: 'virtoCommerce.catalogModule.propertyDetailController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/property-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, blade);
    };

    $scope.getPropValues = function (propId, keyword) {
        return properties.values({ propertyId: propId, keyword: keyword }).$promise.then(function (result) {
            return result;
        });
    };

    var formScope;
    $scope.setForm = function (form) { formScope = form; }

    blade.toolbarCommands = [
		{
		    name: "platform.commands.save", icon: 'fa fa-save',
		    executeMethod: function () {
		        saveChanges();
		    },
		    canExecuteMethod: canSave,
		    permission: blade.updatePermission
		},
        {
            name: "platform.commands.reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(blade.origEntity, blade.currentEntity);
            },
            canExecuteMethod: isDirty,
            permission: blade.updatePermission
        },
		  {
		      name: "catalog.commands.add-property", icon: 'fa fa-plus',
		      executeMethod: function () {
		          var newBlade = {
		              id: 'editCatalogProperty',
		              catalogId: blade.currentEntity.id,
		              title: 'catalog.blades.property-detail.title-catalog-new',
		              subtitle: 'catalog.blades.property-detail.subtitle-catalog-new',
		              controller: 'virtoCommerce.catalogModule.propertyDetailController',
		              template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/property-detail.tpl.html'
		          };

		          bladeNavigationService.showBlade(newBlade, blade);
		      },
		      canExecuteMethod: function () {
		          return true;
		      },
		      permission: blade.updatePermission
		  }
    ];

    if (blade.currentEntity) {
        initializeBlade(angular.copy(blade.currentEntity));
    } else {
        blade.refresh();
    }
}]);
