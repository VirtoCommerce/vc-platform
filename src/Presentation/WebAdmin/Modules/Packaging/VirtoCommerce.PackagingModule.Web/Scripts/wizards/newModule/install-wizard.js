angular.module('virtoCommerce.packaging.wizards.newModule.installWizard', [
    'virtoCommerce.packaging.wizards.newModule.moduleInstallProgress',
    'angularFileUpload'
])
.controller('installWizardController', ['$scope', 'bladeNavigationService', 'FileUploader', 'modules', function ($scope, bladeNavigationService, FileUploader, modules) {

    $scope.submit = function () {
        $scope.blade.isLoading = true;
        $scope.isInstalling = true;

        var newBlade = {
            id: 'moduleInstallProgress',
            title: $scope.blade.title,
            subtitle: 'Installation progress',
            controller: 'moduleInstallProgressController',
            template: 'Modules/Packaging/VirtoCommerce.PackagingModule.Web/Scripts/wizards/newModule/module-wizard-progress-step.tpl.html'
        };

        if ($scope.blade.mode === 'install') {
            modules.install({ id: $scope.currentEntity.id }, function (data) {
                newBlade.currentEntityId = data;
                bladeNavigationService.showBlade(newBlade, $scope.blade);
            });
        } else if ($scope.blade.mode === 'update') {
            modules.update({ id: $scope.currentEntity.id }, function (data) {
                newBlade.currentEntityId = data;
                bladeNavigationService.showBlade(newBlade, $scope.blade);
            });
        }
    };

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

            uploader.onAfterAddingFile = function (item) {
                $scope.blade.isLoading = true;
            };

            uploader.onCompleteAll = function () {
                $scope.blade.isLoading = false;
            }

            uploader.onSuccessItem = function (fileItem, data, status, headers) {
                $scope.currentEntity = data;
            };
        }
    };

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
    if ($scope.blade.mode === 'install') {
        $scope.actionButtonTitle = 'Install';
    } else {
        $scope.actionButtonTitle = 'Update';
    }

    $scope.blade.isLoading = false;
}]);


