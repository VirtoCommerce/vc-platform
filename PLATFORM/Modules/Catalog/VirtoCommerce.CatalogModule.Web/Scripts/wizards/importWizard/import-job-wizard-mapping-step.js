angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.importJobMappingController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.catalogModule.imports', function ($scope, bladeNavigationService, imports) {
    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

        $scope.item = $scope.blade.item;
        if ($scope.blade.isNew) {
            imports.getAutoMapping({ path: $scope.item.templatePath, entityImporter: $scope.item.entityImporter, delimiter: $scope.item.columnDelimiter }, function (result) {
                $scope.blade.isLoading = false;
                $scope.item.propertiesMap = result;
            });
        } else {
            $scope.blade.isLoading = false;
        }
    };


    $scope.saveChanges = function () {
        $scope.blade.parentBlade.item.propertiesMap = $scope.item.propertiesMap;
        $scope.bladeClose();
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.setSelectedItem = function (data) {
        $scope.selectedItem = data;
    }

    $scope.bladeToolbarCommands = [
        {
            name: "Edit",
            icon: 'fa fa-edit',
            executeMethod: function () {
                $scope.editMapping($scope.selectedItem);
            },
            canExecuteMethod: function () {
                return $scope.selectedItem;
            }
        }
    ];

    $scope.editMapping = function (column) {
        var newBlade = {
            id: 'importJobWizardMappingEdit',
            item: column,
            csvColumns: $scope.item.availableCsvColumns,
            title: column.entityColumnName,
            subtitle: 'Edit column mapping',
            controller: 'virtoCommerce.catalogModule.importJobMappingEditController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/importWizard/import-job-wizard-mapping-step-edit.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    $scope.blade.onClose = function (closeCallback) {

        if ($scope.blade.childrenBlades.length > 0) {
            var callback = function () {
                if ($scope.blade.childrenBlades.length == 0) {
                    closeCallback();
                };
            };
            angular.forEach($scope.blade.childrenBlades, function (child) {
                bladeNavigationService.closeBlade(child, callback);
            });
        }
        else {
            closeCallback();
        }
    };

    $scope.blade.refresh();

}]);


