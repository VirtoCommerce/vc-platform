angular.module('platformWebApp')
.controller('platformWebApp.licenseDetailsDialogController', ['$scope', '$window', '$modalInstance', 'dialog', 'FileUploader', 'platformWebApp.licensingApi', function ($scope, $window, $modalInstance, dialog, FileUploader, licensingApi) {
    angular.extend($scope, dialog);

    function refresh() {
        $window.location.reload();
        //licensingApi.get(null, function (license) {
        //    $scope.currentEntity = license;
        //    $scope.isLoading = false;
        //});
    }

    $scope.activate = function (activationCode, license) {
        // TODO:

        refresh();
    };
    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    // create the uploader
    var uploader = $scope.uploader = new FileUploader({
        scope: $scope,
        headers: { Accept: 'application/json' },
        url: 'api/platform/licensing/activateByFile',
        method: 'POST',
        autoUpload: true,
        removeAfterUpload: true
    });

    // ADDING FILTERS
    // lic only
    uploader.filters.push({
        name: 'licFilter',
        fn: function (i /*{File|FileLikeObject}*/, options) {
            return i.name.toLowerCase().endsWith('.lic');
        }
    });

    uploader.onBeforeUploadItem = function (fileItem) {
        $scope.isLoading = true;
    };

    uploader.onErrorItem = function (item, response, status, sdsdsd, assasa) {
        $scope.isLoading = false;
        // bladeNavigationService.setError(item._file.name + ' failed: ' + (response.message ? response.message : status), blade);
    };

    // uploader.onCompleteAll = function () { $scope.isLoading = false; };

    uploader.onSuccessItem = refresh;
}]);
