angular.module('platformWebApp')
.controller('platformWebApp.assets.assetUploadController', ['$scope', 'platformWebApp.assets.api', 'platformWebApp.bladeNavigationService', 'FileUploader', function ($scope, assets, bladeNavigationService, FileUploader) {
    var blade = $scope.blade;
    
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

            uploader.onAfterAddingAll = function () {
                blade.uploadCompleted = false;
            };

            uploader.onCompleteAll = function () {
                blade.parentBlade.refresh();
                blade.uploadCompleted = true;
            };
        }
    }
   
    $scope.addImageFromUrl = function () {
        if (blade.newExternalImageUrl) {
            blade.uploadCompleted = false;

        	assets.uploadFromUrl({ folderUrl: blade.currentEntityId, url: blade.newExternalImageUrl }, function (data) {
        	    blade.parentBlade.refresh();
        	    blade.newExternalImageUrl = undefined;
        	    blade.uploadCompleted = true;
            });
        }
    };

    blade.headIcon = 'fa-file-text-o';
    
    initialize();
    blade.isLoading = false;
}]);
