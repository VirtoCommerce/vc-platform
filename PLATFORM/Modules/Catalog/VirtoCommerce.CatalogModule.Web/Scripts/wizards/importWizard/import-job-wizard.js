angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.importJobWizardController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'virtoCommerce.catalogModule.imports', 'FileUploader', 'platformWebApp.notificationService', function ($scope, bladeNavigationService, dialogService, imports, FileUploader, notificationService) {

    $scope.blade.refresh = function () {
        if ($scope.blade.isNew) {
            $scope.blade.isLoading = false;
        } else {
            $scope.blade.item.$get({}, function (data) {
                $scope.blade.item = angular.copy(data);
                $scope.blade.isLoading = false;
            });
        }
    }

    $scope.createItem = function (execute) {
        $scope.blade.item.$create(null,
            function (dbItem) {
                if (execute) {
                    dbItem.$run(null, function (notify) {
                        // notificationService.task({ title: notify.title, description: notify.description });
                    });
                }

                $scope.blade.parentBlade.refresh();
                $scope.bladeClose();
            });
    }

    $scope.updateItem = function () {
        $scope.blade.item.$update(null,
            function (dbItem) {
                $scope.bladeClose();
                $scope.blade.parentBlade.refresh();
            });
    }

    function initialize() {
        $scope.blade.refresh();

        if (!$scope.uploader) {
            // Creates a uploader
            var uploader = $scope.uploader = new FileUploader({
                scope: $scope,
                headers: { Accept: 'application/json' },
                url: 'api/assets',
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


            uploader.onSuccessItem = function (fileItem, asset, status, headers) {
                $scope.blade.item.templatePath = asset[0].url;
            };
        }
    };

    $scope.canMapColumns = function () {
        return $scope.blade.item.templatePath &&
            $scope.blade.item.catalogId &&
            $scope.formScope &&
            $scope.formScope.$valid &&
            $scope.blade.item.entityImporter;
    }

    $scope.openBlade = function (type) {
        $scope.blade.onClose(function () {
            var newBlade = null;
            switch (type) {
                case 'importer':
                    newBlade = {
                        id: "importJobImporters",
                        title: $scope.blade.item.name,
                        subtitle: 'Import types',
                        controller: 'virtoCommerce.catalogModule.importJobImportersController',
                        template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/importWizard/import-job-wizard-importers-step.tpl.html'
                    };
                    break;
                case 'catalogs':
                    newBlade = {
                        id: "importJobCatalogs",
                        item: $scope.blade.item,
                        title: $scope.blade.item.name,
                        subtitle: 'Catalogs',
                        controller: 'virtoCommerce.catalogModule.importJobCatalogsController',
                        template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/importWizard/import-job-wizard-catalogs-step.tpl.html'
                    };
                    break;
                case 'settings':
                    newBlade = {
                        id: "importJobSettings",
                        item: $scope.blade.item,
                        title: $scope.blade.item.name,
                        subtitle: 'Import settings',
                        controller: 'virtoCommerce.catalogModule.importJobSettingsController',
                        template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/importWizard/import-job-wizard-settings-step.tpl.html'
                    };
                    break;
                case 'mapping':
                    newBlade = {
                        id: "importJobMapping",
                        item: $scope.blade.item,
                        isNew: $scope.blade.isNew,
                        title: $scope.blade.item.name,
                        subtitle: 'Column mapping',
                        controller: 'virtoCommerce.catalogModule.importJobMappingController',
                        template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/importWizard/import-job-wizard-mapping-step.tpl.html'
                    };
                    break;
            }

            if (newBlade != null) {
                bladeNavigationService.showBlade(newBlade, $scope.blade);
            }
        });
    }


    $scope.setForm = function (form) {
        $scope.formScope = form;
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


    initialize();
}]);
