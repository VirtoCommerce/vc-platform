angular.module('platformWebApp')
.controller('platformWebApp.assets.assetUploadController', ['$scope', 'platformWebApp.assets.api', 'platformWebApp.bladeNavigationService', 'FileUploader', function ($scope, assets, bladeNavigationService, FileUploader) {
    var blade = $scope.blade;
    // $scope.currentEntities = blade.currentEntities = blade.currentEntity.attachments;
    
    function initialize() {
        if (!$scope.uploader) {
            // Create the uploader
            var uploader = $scope.uploader = new FileUploader({
                scope: $scope,
                headers: { Accept: 'application/json' },
                url: 'api/platform/assets?folderUrl=' + blade.currentEntityId,
                method: 'POST',
                autoUpload: true,
                removeAfterUpload: true
            });

            uploader.onSuccessItem = function (fileItem, assets, status, headers) {
          
            };
        }
    }
   
    $scope.copyUrl = function (data) {
        window.prompt("Copy to clipboard: Ctrl+C, Enter", data.url);
    };

    $scope.addImageFromUrl = function () {
        if (blade.newExternalImageUrl) {
        	assets.uploadFromUrl({ folderUrl: blade.currentEntityId, url: blade.newExternalImageUrl }, function (data) {
                //blade.currentEntity.images.push(data);
                blade.newExternalImageUrl = undefined;
            });
        }
    };

    blade.headIcon = 'fa-file-text-o';
    
    initialize();
    blade.isLoading = false;
}]);
