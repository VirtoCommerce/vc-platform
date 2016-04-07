angular.module('platformWebApp')
.controller('virtoCommerce.contentModule.assetUploadController', ['$scope', 'virtoCommerce.contentModule.contentApi', 'platformWebApp.bladeNavigationService', 'FileUploader', function ($scope, contentApi, bladeNavigationService, FileUploader) {
    var blade = $scope.blade;
    var currentEntities;

    function initialize() {
        // Create the uploader
        var uploader = $scope.uploader = new FileUploader({
            scope: $scope,
            headers: { Accept: 'application/json' },
            url: 'api/content/' + blade.contentType + '/' + blade.storeId + '?folderUrl=' + blade.currentEntityId,
            method: 'POST',
            autoUpload: true,
            removeAfterUpload: true
        });

        //uploader.onAfterAddingAll = function (addedItems) {
        //    blade.isLoading = true;
        //    blade.uploadCompleted = false;
        //    bladeNavigationService.setError(null, blade);

        //    // check for asset duplicates
        //    contentApi.query({ folderUrl: blade.currentEntityId },
        //        function (data) {
        //            blade.isLoading = false;
        //            currentEntities = data;

        //            _.each(addedItems, promptUserDecision);
        //            uploader.uploadAll();
        //        }, function (error) {
        //            bladeNavigationService.setError('Error ' + error.status, blade);
        //        });
        //};

        uploader.onErrorItem = function (item, response, status, headers) {
            bladeNavigationService.setError(item._file.name + ' failed: ' + (response.message ? response.message : status), blade);
        };

        uploader.onCompleteAll = function () {
            blade.parentBlade.refresh();
            blade.uploadCompleted = true;
        };

        blade.isLoading = false;
    }

    function promptUserDecision(item) {
        if (_.any(currentEntities, function (x) { return x.type === 'blob' && x.name.toLowerCase() === item.file.name.toLowerCase() })) {
            var result = prompt("File \"" + item.file.name + "\" already exists!\n- Change name / press [OK] to overwrite.\n- Press [Cancel] to skip this file.\nFile name:", item.file.name);
            if (result == null) {
                item.remove();
            } else if (!result) {
                promptUserDecision(item);
            } else if (result !== item.file.name) {
                item.file.name = result;
                promptUserDecision(item);
            } else {
                item.url += "&forceFileOverwrite=true";
            }
        }
    }

    $scope.addImageFromUrl = function () {
        if (blade.newExternalImageUrl) {
            blade.uploadCompleted = false;

            contentApi.save({
                contentType: blade.contentType,
                storeId: blade.storeId,
                folderUrl: blade.currentEntityId,
                url: blade.newExternalImageUrl
            }, null, function (data) {
                blade.parentBlade.refresh();
                blade.newExternalImageUrl = undefined;
                blade.uploadCompleted = true;
            });
        }
    };

    blade.headIcon = 'fa-file-o';

    initialize();
}]);
