angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.themeUploadController', ['$scope', 'platformWebApp.dialogService', 'virtoCommerce.contentModule.contentApi', 'FileUploader', 'platformWebApp.bladeNavigationService', function ($scope, dialogService, contentApi, FileUploader, bladeNavigationService) {
    var blade = $scope.blade;

    // create the uploader
    var uploader = $scope.uploader = new FileUploader({
        scope: $scope,
        headers: { Accept: 'application/json' },
        url: 'api/content/themes/' + blade.storeId + '?folderUrl=',
        queueLimit: 1,
        autoUpload: true,
        removeAfterUpload: false
    });

    // ADDING FILTERS
    // Zips only
    uploader.filters.push({
        name: 'zipFilter',
        fn: function (i /*{File|FileLikeObject}*/, options) {
            return i.name.toLowerCase().endsWith('.zip');
        }
    });

    uploader.onAfterAddingFile = function (item) {
        $scope.themeName = item.file.name.substring(0, item.file.name.lastIndexOf('.'));
        blade.isLoading = true;
    };

    uploader.onSuccessItem = function (fileItem, files) {
        contentApi.unpack({
            contentType: 'themes',
            storeId: blade.storeId,
            archivepath: files[0].name,
            destPath: $scope.themeName
        }, function (data) {
            blade.parentBlade.initialize();
            if (blade.parentBlade.parentBlade)
                blade.parentBlade.parentBlade.refresh(blade.storeId, 'themes');
            bladeNavigationService.closeBlade(blade);
        },
        function (error) {
            uploader.clearQueue();
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    };

    uploader.onErrorItem = function (item, response, status, headers) {
        bladeNavigationService.setError(item._file.name + ' failed: ' + (response.message ? response.message : status), blade);
    };

    blade.title = 'content.blades.theme-upload.title',
    blade.isLoading = false;
}]);