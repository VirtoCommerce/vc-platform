angular.module('platformWebApp')
.controller('platformWebApp.installWizardController', ['$scope', 'platformWebApp.bladeNavigationService', 'FileUploader', 'platformWebApp.modules', function ($scope, bladeNavigationService, FileUploader, modules) {

    $scope.submit = function () {
        $scope.isInstalling = true;

        if ($scope.blade.mode === 'install') {
            modules.install({ fileName: $scope.blade.currentEntity.fileName }, onAfterSubmitted, function (error) {
                $scope.isInstalling = false;
                bladeNavigationService.setError('Error ' + error.status, $scope.blade);
            });
        } else if ($scope.blade.mode === 'update') {
            modules.update({ id: $scope.blade.currentEntity.id, fileName: $scope.blade.currentEntity.fileName }, onAfterSubmitted, function (error) {
                $scope.isInstalling = false;
                bladeNavigationService.setError('Error ' + error.status, $scope.blade);
            });
        }
    };

    function onAfterSubmitted(data) {
        var newBlade = {
            id: 'moduleInstallProgress',
            currentEntity: data,
            title: $scope.blade.title,
            controller: 'platformWebApp.moduleInstallProgressController',
            template: '$(Platform)/Scripts/app/packaging/wizards/newModule/module-wizard-progress-step.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade.parentBlade);
    }

    function endsWith(str, suffix) {
        return str.indexOf(suffix, str.length - suffix.length) !== -1;
    }

    function initialize() {
        if (!$scope.uploader) {
            // create the uploader
            var uploader = $scope.uploader = new FileUploader({
                scope: $scope,
                headers: { Accept: 'application/json' },
                url: 'api/platform/modules',
                autoUpload: true,
                removeAfterUpload: true
            });

            // ADDING FILTERS
            // packages only
            uploader.filters.push({
                name: 'packageFilter',
                fn: function (i /*{File|FileLikeObject}*/, options) {
                    return endsWith(i.name, '.zip');
                }
            });

            uploader.onAfterAddingFile = function (item) {
                bladeNavigationService.setError(null, $scope.blade);
                $scope.blade.isLoading = true;
            };

            uploader.onCompleteAll = function () {
                $scope.blade.isLoading = false;
            }

            uploader.onSuccessItem = function (fileItem, data, status, headers) {
                if (data.tags) {
                    data.tags = data.tags.split(' ');
                }
                $scope.blade.currentEntity = data;
            };

            uploader.onErrorItem = function (item, response, status, headers) {
                if (item.isUploaded) {
                    bladeNavigationService.setError('File uploaded with error status: ' + status, $scope.blade);
                } else {
                    bladeNavigationService.setError('Failed to upload. Error status: ' + status, $scope.blade);
                }
            }
        }
    };

    initialize();
    if ($scope.blade.mode === 'install') {
        $scope.actionButtonTitle = 'Install';
    } else {
        $scope.actionButtonTitle = 'Update';
    }

    $scope.blade.isLoading = false;
}]);
