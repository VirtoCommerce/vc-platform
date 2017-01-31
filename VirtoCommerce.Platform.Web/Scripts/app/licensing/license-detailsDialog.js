angular.module('platformWebApp')
.controller('platformWebApp.licenseDetailsDialogController', ['$scope', '$window', '$modalInstance', 'dialog', 'FileUploader', 'platformWebApp.licensingApi', function ($scope, $window, $modalInstance, dialog, FileUploader, licensingApi) {
    angular.extend($scope, dialog);

    function refresh() {
        //licensingApi.get(null, function (license) {
        //    $scope.currentEntity = license;
        //    $scope.isLoading = false;
        //});
    }
    // $scope.currentEntity = { type: "Community deployment", name: "john doe company", expirationDate: new Date().setMonth(new Date().getMonth() + 1) };
    

    $scope.activate = function (activationCode, license) {
        $window.location.reload();
    };
    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    // create the uploader
    var uploader = $scope.uploader = new FileUploader({
        scope: $scope,
        headers: { Accept: 'application/json' },
        url: 'api/platform/assets/localstorage',
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

    refresh();
}]);
