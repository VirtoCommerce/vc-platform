angular.module('virtoCommerce.packaging.wizards.newModule.installWizard', [
// 'virtoCommerce.packaging.wizards.newModule...'
    'angularFileUpload'
])
.controller('installWizardController', ['$scope', 'bladeNavigationService', 'FileUploader', 'modules', function ($scope, bladeNavigationService, FileUploader, modules) {

    $scope.create = function () {
        $scope.blade.isLoading = true;


        //modules.updateitem({ id: $scope.blade.parentBlade.currentEntityId, associations: entriesCopy }, function () {
        //    $scope.bladeClose();
        //    $scope.blade.parentBlade.refresh();
        //});
    }

    function endsWith(str, suffix) {
        return str.indexOf(suffix, str.length - suffix.length) !== -1;
    }

    function initialize() {
        if (!$scope.uploader) {
            // Creates a uploader
            var uploader = $scope.uploader = new FileUploader({
                scope: $scope,
                headers: { Accept: 'application/json' },
                url: 'api/modules',
                autoUpload: true,
                removeAfterUpload: true
            });

            // ADDING FILTERS
            // packages only
            uploader.filters.push({
                name: 'packageFilter',
                fn: function (i /*{File|FileLikeObject}*/, options) {
                    return endsWith(i.name, '.nupkg');
                }
            });

            uploader.onSuccessItem = function (fileItem, data, status, headers) {
                $scope.currentEntity = data;
            };
        }
    };

    /*
    $scope.openBlade = function (type) {
        $scope.blade.onClose(function () {
            var newBlade = null;
            switch (type) {
                case 'progress':
                    newBlade = {
                        id: 'selectCatalog',
                        title: 'Select Catalog',
                        subtitle: 'Adding Associations to product',
                        controller: 'catalogsSelectController',
                        bladeActions: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/wizards/common/wizard-ok-action.tpl.html',
                        template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/catalogs-select.tpl.html'
                    };

                    break;
            }

            if (newBlade != null) {
                bladeNavigationService.showBlade(newBlade, $scope.blade);
            }
        });
    }*/

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        closeCallback();
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    initialize();
    $scope.blade.selection = [];
    $scope.blade.isLoading = false;
}]);


