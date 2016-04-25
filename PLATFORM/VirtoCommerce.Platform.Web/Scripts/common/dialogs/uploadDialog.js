angular.module('platformWebApp')
.controller('platformWebApp.uploadDialogController', ['$scope', '$modalInstance', 'dialog', function ($scope, $modalInstance, dialog) {
    angular.extend($scope, dialog);

    $scope.uploader.onAfterAddingAll = function () {
        $modalInstance.close(false);
    }

    $scope.cancel = function () {
        $modalInstance.close(false);
    }
}]);