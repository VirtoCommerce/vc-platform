angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.catalogCSVimportWizardController', ['$scope', '$localStorage', 'platformWebApp.bladeNavigationService', 'FileUploader', 'virtoCommerce.catalogModule.import', function ($scope, $localStorage, bladeNavigationService, FileUploader, importResource) {
    var blade = $scope.blade;
    blade.isLoading = false;
    blade.title = 'catalog.wizards.catalog-CSV-import.title';
    blade.subtitle = 'catalog.wizards.catalog-CSV-import.subtitle';
    blade.subtitleVales = { name: blade.catalog.name };

    $scope.columnDelimiters = [
        { name: "catalog.wizards.catalog-CSV-import.labels.space", value: " " },
        { name: "catalog.wizards.catalog-CSV-import.labels.comma", value: "," },
        { name: "catalog.wizards.catalog-CSV-import.labels.semicolon", value: ";" },
        { name: "catalog.wizards.catalog-CSV-import.labels.tab", value: "\t" }
    ];


    if (!$scope.uploader) {
        // create the uploader
        var uploader = $scope.uploader = new FileUploader({
            scope: $scope,
            headers: { Accept: 'application/json' },
            url: 'api/platform/assets?folderUrl=tmp',
            method: 'POST',
            autoUpload: true,
            removeAfterUpload: true
        });

        // ADDING FILTERS
        // Images only
        uploader.filters.push({
            name: 'csvFilter',
            fn: function (i /*{File|FileLikeObject}*/, options) {
                var type = '|' + i.type.slice(i.type.lastIndexOf('/') + 1) + '|';
                return '|csv|vnd.ms-excel|'.indexOf(type) !== -1;
            }
        });

        uploader.onBeforeUploadItem = function (fileItem) {
            blade.isLoading = true;
        };

        uploader.onSuccessItem = function (fileItem, asset, status, headers) {
            blade.csvFileUrl = asset[0].relativeUrl;

            importResource.getMappingConfiguration({ fileUrl: blade.csvFileUrl, delimiter: blade.columnDelimiter }, function (data) {
                if ($localStorage.lastKnownImportData && $localStorage.lastKnownImportData.eTag === data.eTag) {
                    angular.extend(data, $localStorage.lastKnownImportData);
                }

                blade.importConfiguration = data;
                blade.isLoading = false;
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        };

        uploader.onAfterAddingAll = function (addedItems) {
            bladeNavigationService.setError(null, blade);
        };

        uploader.onErrorItem = function (item, response, status, headers) {
            bladeNavigationService.setError(item._file.name + ' failed: ' + (response.message ? response.message : status), blade);
        };
    };

    $scope.canMapColumns = function () {
        return blade.importConfiguration && $scope.formScope && $scope.formScope.$valid;
    }

    $scope.openMappingStep = function () {
        var newBlade = {
            id: "importMapping",
            importConfiguration: blade.importConfiguration,
            title: 'catalog.wizards.catalog-CSV-import-wizard-mapping-step.title',
            subtitle: 'catalog.wizards.catalog-CSV-import-wizard-mapping-step.subtitle',
            controller: 'virtoCommerce.catalogModule.catalogCSVimportWizardMappingStepController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/import/wizard/catalog-CSV-import-wizard-mapping-step.tpl.html'
        };

        blade.canImport = true;
        bladeNavigationService.showBlade(newBlade, blade);
    };

    $scope.startImport = function () {
        $localStorage.lastKnownImportData = {
            eTag: blade.importConfiguration.eTag,
            propertyMaps: blade.importConfiguration.propertyMaps,
            propertyCsvColumns: blade.importConfiguration.propertyCsvColumns
        };

        var exportInfo = { configuration: blade.importConfiguration, fileUrl: blade.csvFileUrl, catalogId: blade.catalog.id };
        importResource.run(exportInfo, function (notification) {
            var newBlade = {
                id: "importProgress",
                catalog: blade.catalog,
                notification: notification,
                importConfiguration: blade.importConfiguration,
                controller: 'virtoCommerce.catalogModule.catalogCSVimportController',
                template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/import/catalog-CSV-import.tpl.html'
            };

            $scope.$on("new-notification-event", function (event, notification) {
                if (notification && notification.id == newBlade.notification.id) {
                    blade.canImport = notification.finished != null;
                }
            });

            blade.canImport = false;
            bladeNavigationService.showBlade(newBlade, blade);

        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, blade);
        });
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    };
    
}]);
