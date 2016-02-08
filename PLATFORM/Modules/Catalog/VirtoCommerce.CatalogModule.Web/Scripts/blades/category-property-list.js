angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.categoryPropertyListController', ['$scope', 'virtoCommerce.catalogModule.categories', 'virtoCommerce.catalogModule.properties', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, categories, properties, bladeNavigationService, dialogService) {
    $scope.blade.origEntity = {};

    $scope.blade.refresh = function (parentRefresh) {
        categories.get({ id: $scope.blade.currentEntityId }, function (data) {
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
        categories.update({}, $scope.blade.currentEntity, function (data, headers) {
            $scope.blade.refresh(true);
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    };

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();

        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "catalog.dialogs.category-save.title",
                message: "catalog.dialogs.category-save.message",
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
            title: 'catalog.blades.property-detail.title-category',
            subtitle: 'catalog.blades.property-detail.subtitle-category',
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
		    name: "platform.commands.save", icon: 'fa fa-save',
		    executeMethod: function () {
		        saveChanges();
		    },
		    canExecuteMethod: function () {
		        return isDirty() && formScope && formScope.$valid;
		    },
		    permission: 'catalog:update'
		},
        {
            name: "platform.commands.reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
            },
            canExecuteMethod: isDirty,
            permission: 'catalog:update'
        },
		  {
		      name: "catalog.commands.add-property", icon: 'fa fa-plus',
		      executeMethod: function () {
		      	var newBlade = {
		      		id: 'editCategoryProperty',
		      		categoryId: $scope.blade.currentEntity.id,
		      		title: 'catalog.blades.property-detail.title-category-new',
		      		subtitle: 'catalog.blades.property-detail.subtitle-category-new',
		      		controller: 'virtoCommerce.catalogModule.propertyDetailController',
		      		template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/property-detail.tpl.html'
		      	};

		      	bladeNavigationService.showBlade(newBlade, $scope.blade);
		      },
		      canExecuteMethod: function () {
		          return true;
		      },
		      permission: 'catalog:update'
		  }
    ];

    if ($scope.blade.currentEntity) {
        initializeBlade(angular.copy($scope.blade.currentEntity));
    } else {
        $scope.blade.refresh();
    }
}]);
