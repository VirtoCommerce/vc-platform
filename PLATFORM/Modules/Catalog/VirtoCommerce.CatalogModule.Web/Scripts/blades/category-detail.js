angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.categoryDetailController', ['$rootScope', '$scope', 'platformWebApp.bladeNavigationService', '$injector', 'virtoCommerce.catalogModule.categories', 'platformWebApp.dialogService', function ($rootScope, $scope, bladeNavigationService, $injector, categories, dialogService) {
    $scope.blade.origEntity = {};

    $scope.blade.refresh = function (parentRefresh) {
        return categories.get({ categoryId: $scope.blade.currentEntityId }, function (data) {
            initializeBlade(data);
            if (parentRefresh) {
                $scope.blade.parentBlade.refresh();
            }
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    };

    function initializeBlade(data) {
        $scope.blade.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.title = data.name;
        $scope.blade.isLoading = false;
    };

    $scope.codeValidator = function(value) {
        var pattern = /[$+;=%{}[\]|\\\/@ ~#!^*&()?:'<>,]/;
        return !pattern.test(value);
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

    var formScope;
    $scope.setForm = function (form) {
        formScope = form;
    }

    $scope.blade.toolbarCustomTemplates = ['Scripts/common/templates/toolbar-isActive.tpl.html'];

    $scope.blade.toolbarCommands = [
		{
		    name: "Save", icon: 'fa fa-save',
		    executeMethod: function () {
		        saveChanges();
		    },
		    canExecuteMethod: function () {
		        return isDirty() && formScope && formScope.$valid;
		    },
		    permission: 'catalog:categories:manage'
		},
        {
            name: "Reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            },
            permission: 'catalog:categories:manage'
        }
    ];

    if ($scope.blade.currentEntity) {
        initializeBlade($scope.blade.currentEntity);
    } else {
        $scope.blade.refresh();
    }
}]);
