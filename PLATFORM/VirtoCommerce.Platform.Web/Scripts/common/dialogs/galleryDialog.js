angular.module('platformWebApp')
.controller('platformWebApp.galleryDialogController', ['$scope', '$modalInstance', 'dialog', function ($scope, $modalInstance, dialog) {
    angular.extend($scope, dialog);

    var imgCount = dialog.images.length;

    $scope.close = function () {
        $modalInstance.close(false);
    }

    $scope.prevImage = function (index) {
        var i = index == -1 ? imgCount - 1 : index;
        $scope.currentImage = dialog.images[i];
    }

    $scope.nextImage = function (index) {
        var i = index == imgCount ? 0 : index;
        $scope.currentImage = dialog.images[i];
    }

    $scope.openImage = function (image) {
        $scope.currentImage = image;
    }
}]);