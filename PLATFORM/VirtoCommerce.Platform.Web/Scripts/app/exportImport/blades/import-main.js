angular.module('platformWebApp')
.controller('platformWebApp.exportImport.importMainController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.exportImport.resource', 'FileUploader', function ($scope, bladeNavigationService, exportImportResourse, FileUploader) {
    var blade = $scope.blade;
    blade.headIcon = 'fa-download';
    blade.title = 'Data import';
    blade.isLoading = false;
    
    $scope.$on("new-notification-event", function (event, notification) {
        if (blade.notification && notification.id == blade.notification.id) {
            angular.copy(notification, blade.notification);
        }
    });

    $scope.canStartProcess = function () {
        return blade.currentEntities && _.some(blade.currentEntities, function (x) { return x.isChecked; });
    }

    $scope.startProcess = function () {
        blade.isLoading = true;
        var selection = _.where(blade.currentEntities, { isChecked: true });
        exportImportResourse.runImport({
            fileUrl: blade.fileUrl,
            modules: _.pluck(selection, 'id')
        },
        function (data) { blade.notification = data; blade.isLoading = false; },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    }


    if (!$scope.uploader) {
        function endsWith(str, suffix) {
            return str.indexOf(suffix, str.length - suffix.length) !== -1;
        }

        // Creates an uploader
        var uploader = $scope.uploader = new FileUploader({
            scope: $scope,
            headers: { Accept: 'application/json' },
            url: 'api/assets',
            method: 'POST',
            autoUpload: true,
            removeAfterUpload: true
        });
        
        // ADDING FILTERS
        // zip only
        uploader.filters.push({
            name: 'zipFilter',
            fn: function (i /*{File|FileLikeObject}*/, options) {
                return endsWith(i.name.toLowerCase(), '.zip');
            }
        });

        uploader.onBeforeUploadItem = function (fileItem) {
            blade.isLoading = true;
        };

        uploader.onSuccessItem = function (fileItem, asset, status, headers) {
            blade.fileUrl = asset[0].relativeUrl;

            exportImportResourse.getImportInfo({ fileUrl: blade.fileUrl }, function (data) {
                blade.info = data.exportManifest;
                blade.currentEntities = data.modules;
                blade.isLoading = false;
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        };
    }
}]);
