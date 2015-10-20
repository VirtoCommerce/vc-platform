angular.module('virtoCommerce.quoteModule')
.controller('virtoCommerce.quoteModule.quoteAssetController', ['$scope', 'virtoCommerce.catalogModule.items', 'platformWebApp.bladeNavigationService', '$filter', 'FileUploader', 'platformWebApp.dialogService', '$injector', function ($scope, items, bladeNavigationService, $filter, FileUploader, dialogService, $injector) {
    var blade = $scope.blade;
    $scope.currentEntities = blade.currentEntities = blade.currentEntity.attachments;
    
    function initialize() {
        if (!$scope.uploader) {
            // create the uploader
            var uploader = $scope.uploader = new FileUploader({
                scope: $scope,
                headers: { Accept: 'application/json' },
                url: 'api/platform/assets?folderUrl=quote',
                method: 'POST',
                autoUpload: true,
                removeAfterUpload: true
            });

            uploader.onSuccessItem = function (fileItem, assets, status, headers) {
                angular.forEach(assets, function (asset) {
                    //ADD uploaded asset
                    blade.currentEntities.push(asset);
                });
            };
        }
    }
    
    $scope.removeAction = function (asset) {
        var idx = blade.currentEntities.indexOf(asset);
        if (idx >= 0) {
            blade.currentEntities.splice(idx, 1);
        }
    };

    $scope.copyUrl = function (data) {
        window.prompt("Copy to clipboard: Ctrl+C, Enter", data.url);
    };

    blade.headIcon = 'fa-file-text-o';
    
    initialize();
    blade.isLoading = false;
}]);
