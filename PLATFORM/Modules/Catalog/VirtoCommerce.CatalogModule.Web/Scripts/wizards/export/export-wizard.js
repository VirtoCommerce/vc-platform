angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.exportWizardController', ['$scope', 'bladeNavigationService', 'dialogService', 'virtoCommerce.catalogModule.imports', 'notificationService', function ($scope, bladeNavigationService, dialogService, imports, notificationService) {
    $scope.blade.currentEntity = { types: [] };

    $scope.openBlade = function (type) {
        $scope.blade.onClose(function () {
            var newBlade = null;
            switch (type) {
                case 'catalog':
                    newBlade = {
                        subtitle: 'Select Catalog to export from',
                        controller: 'virtoCommerce.catalogModule.catalogsSelectController',
                        doShowAllCatalogs: true,
                        template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/catalogs-select.tpl.html'
                    };
                    break;
                case 'types':
                    newBlade = {
                        subtitle: 'Select what to export',
                        controller: 'virtoCommerce.catalogModule.exportTypesController',
                        template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/export/export-wizard-types-step.tpl.html'
                    };
                    break;
                case 'format':
                    newBlade = {
                        subtitle: 'Select export data format',
                        controller: 'virtoCommerce.catalogModule.exportFormatController',
                        template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/export/export-wizard-format-step.tpl.html'
                    };
                    break;
            }

            if (newBlade != null) {
                newBlade.id = "exportWizardStep";
                newBlade.title = "Catalog data export",
                bladeNavigationService.showBlade(newBlade, $scope.blade);
            }
        });
    }

    $scope.blade.onAfterCatalogSelected = function (selectedNode) {
        $scope.blade.currentEntity.catalog = selectedNode;
        //catalogId: selectedNode.id,
    };

    $scope.isValid = function () {
        return $scope.blade.currentEntity.catalog && _.any($scope.blade.currentEntity.types) && $scope.blade.currentEntity.format;
    }

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        closeCallback();
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.blade.isLoading = false;
}]);
