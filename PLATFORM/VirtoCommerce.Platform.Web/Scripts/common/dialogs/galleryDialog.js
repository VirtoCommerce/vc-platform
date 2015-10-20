angular.module('platformWebApp')
.controller('platformWebApp.galleryDialogController', ['$scope', '$modalInstance', 'dialog', function ($scope, $modalInstance, dialog) {
    angular.extend($scope, dialog);

    $scope.close = function () {
        $modalInstance.close(false);
    }

    $scope.showImage = function (image) {
        $scope.currentImage = image;

        var imgIndex = dialog.images.indexOf(image);
        var imgCount = dialog.images.length;

        if (imgIndex > 0 && imgIndex < imgCount) {
            $scope.previousImage = dialog.images[imgIndex - 1];
            $scope.nextImage = dialog.images[imgIndex + 1];
        }
        if (imgIndex == 0) {
            $scope.previousImage = dialog.images[imgCount - 1];
            $scope.nextImage = dialog.images[imgIndex + 1];
        }
        if (imgIndex == imgCount - 1) {
            $scope.previousImage = dialog.images[imgIndex - 1];
            $scope.nextImage = dialog.images[0];
        }
    }
}]);