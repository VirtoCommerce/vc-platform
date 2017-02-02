angular.module('platformWebApp')
.controller('platformWebApp.licenseDetailsDialogController', ['$scope', '$window', '$modalInstance', 'dialog', 'FileUploader', 'platformWebApp.licensingApi', function ($scope, $window, $modalInstance, dialog, FileUploader, licensingApi) {
    angular.extend($scope, dialog);

    function refresh() {
        $window.location.reload();
    }

    $scope.activate = function (activationCode) {
        $scope.isLoading = true;
        $scope.activationError = null;
        licensingApi.activateByCode('"' + activationCode + '"', refresh, function (error) {
            $scope.isLoading = false;
            $scope.activationError = error.data.message;
        });
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
        $scope.activationError = null;
    };

    uploader.onErrorItem = function (item, response, status, sdsdsd, assasa) {
        $scope.isLoading = false;
        $scope.activationError = response.message ? response.message : status;
    };

    uploader.onSuccessItem = refresh;
}]);
