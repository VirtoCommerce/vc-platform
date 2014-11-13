angular.module('catalogModule.blades.categoryPropertyDetail', [
   'catalogModule.resources.categories',
    'catalogModule.resources.properties',
	'ui.bootstrap.typeahead',
    'ngSanitize'
])
.controller('categoryPropertyController', ['$rootScope', '$scope', 'categories', 'properties', 'bladeNavigationService', 'dialogService', '$injector', function ($rootScope, $scope, categories, properties, bladeNavigationService, dialogService, $injector) {
    $scope.blade.origEntity = {};
    $scope.blade.selectedProperty = undefined;

    $scope.blade.refresh = function (parentRefresh) {
        categories.get({ categoryId: $scope.blade.currentEntityId }, function (data) {
            initializeBlade(data);
            if (parentRefresh) {
                $scope.blade.parentBlade.refresh();
            }
        });
    };

    function initializeBlade(data) {
        if (data.properties) {
            var numberProps = _.where(data.properties, { valueType: 2, multivalue: false, dictionary: false });
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
        if (angular.isDefined($scope.blade.currentEntity.properties)) {
            $scope.blade.selectedProperty = $scope.blade.currentEntity.properties.length > 0 ? $scope.blade.currentEntity.properties[0] : undefined;

        };
    };

    function isDirty() {
        return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
    };

    function saveChanges() {
        $scope.blade.isLoading = true;
        categories.update({}, $scope.blade.currentEntity, function (data, headers) {
            $scope.blade.refresh(true);
        });
    };

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();

        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "Save changes",
                message: "The category has been modified. Do you want to save changes?",
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
            id: 'editCategoryProperty',
            currentEntityId: prop.id,
            title: 'Edit category property',
            subtitle: 'enter property information',
            controller: 'propertyDetailController',
            template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/property-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.getPropValues = function (propId, keyword) {
        return properties.query({ propertyId: propId, keyword: keyword }).$promise.then(function (result) {
            return result;
        });
    };

    var formScope;
    $scope.setForm = function (form) {
        formScope = form;
    }

    $scope.bladeToolbarCommands = [
		{
		    name: "Save", icon: 'icon-floppy',
		    executeMethod: function () {
		        saveChanges();
		    },
		    canExecuteMethod: function () {
		        return isDirty() && formScope && formScope.$valid;
		    }
		},
        {
            name: "Reset", icon: 'icon-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        },
		  {
		      name: "Add", icon: 'icon-plus',
		      executeMethod: function () {
		          var newBlade = {
		              id: 'editCategoryProperty',
		              title: 'New category property',
		              subtitle: 'enter property information',
		              controller: 'propertyDetailController',
		              template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/property-detail.tpl.html'
		          };

		          bladeNavigationService.showBlade(newBlade, $scope.blade);
		      },
		      canExecuteMethod: function () {
		          return true;
		      }
		  },
		   {
		       name: "Edit", icon: 'icon-new-tab-2',
		       executeMethod: function () {
		           $scope.editProperty($scope.blade.selectedProperty);
		       },
		       canExecuteMethod: function () {
		           return angular.isDefined($scope.blade.selectedProperty) && $scope.blade.selectedProperty.isManageable;
		       }
		   }
    ];

    if ($scope.blade.currentEntity) {
        initializeBlade($scope.blade.currentEntity);
    } else {
        $scope.blade.refresh();
    }
}]);
