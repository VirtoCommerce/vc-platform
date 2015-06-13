angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.catalogPropertyController', ['$scope', 'virtoCommerce.catalogModule.catalogs', 'virtoCommerce.catalogModule.properties', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, catalogs, properties, bladeNavigationService, dialogService) {
    $scope.blade.origEntity = {};

    $scope.blade.refresh = function (parentRefresh) {
        catalogs.get({ id: $scope.blade.currentEntityId }, function (data) {
            initializeBlade(data);
            if (parentRefresh) {
                $scope.blade.parentBlade.refresh();
            }
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    };

    function initializeBlade(data) {
        if (data.properties) {
            var numberProps = _.where(data.properties, { valueType: 'Number', multivalue: false, dictionary: false });
            _.forEach(numberProps, function (prop) {
                _.forEach(prop.values, function (value) {
                    value.value = parseFloat(value.value);
                });
            });
        }

        $scope.blade.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.title = data.name;
        $scope.blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
    };

    function saveChanges() {
        $scope.blade.isLoading = true;
        catalogs.update({}, $scope.blade.currentEntity, function (data, headers) {
            $scope.blade.refresh(true);
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    };

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();

        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "Save changes",
                message: "The catalog has been modified. Do you want to save changes?",
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
            id: 'editCatalogProperty',
            currentEntityId: prop.id,
            catalogId: $scope.blade.currentEntity.id,
            title: 'Edit catalog property',
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

    $scope.blade.toolbarCommands = [
		{
		    name: "Save", icon: 'fa fa-save',
		    executeMethod: function () {
		        saveChanges();
		    },
		    canExecuteMethod: function () {
		        return isDirty() && formScope && formScope.$valid;
		    },
		    permission: 'catalog:catalogs:manage'
		},
        {
            name: "Reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            },
            permission: 'catalog:catalogs:manage'
        },
		  {
		      name: "Add property", icon: 'fa fa-plus',
		      executeMethod: function () {
		          var newBlade = {
		              id: 'editCatalogProperty',
		              catalogId: $scope.blade.currentEntity.id,
		              title: 'New catalog property',
		              subtitle: 'enter property information',
		              controller: 'virtoCommerce.catalogModule.propertyDetailController',
		              template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/property-detail.tpl.html'
		          };

		          bladeNavigationService.showBlade(newBlade, $scope.blade);
		      },
		      canExecuteMethod: function () {
		          return true;
		      },
		      permission: 'catalog:catalogs:manage'
		  }
    ];

    if ($scope.blade.currentEntity) {
        initializeBlade(angular.copy($scope.blade.currentEntity));
    } else {
        $scope.blade.refresh();
    }
}]);
