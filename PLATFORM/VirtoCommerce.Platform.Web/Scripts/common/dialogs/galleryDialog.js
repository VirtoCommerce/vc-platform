angular.module('platformWebApp')
.controller('platformWebApp.galleryDialogController', ['$scope', '$modalInstance', 'dialog', function ($scope, $modalInstance, dialog) {
    angular.extend($scope, dialog);

    var imgCount = dialog.images.length;

    $scope.close = function () {
        $modalInstance.close(false);
    }

    $scope.prevImage = function () {
        var imgIndex = dialog.images.indexOf($scope.currentImage);
        if (imgIndex > 0) {
            $scope.currentImage = dialog.images[imgIndex - 1];
        } else {
            $scope.currentImage = dialog.images[imgCount - 1];
        }
    }

    $scope.nextImage = function () {
        var imgIndex = dialog.images.indexOf($scope.currentImage);
        if (imgIndex < imgCount - 1) {
            $scope.currentImage = dialog.images[imgIndex + 1];
        } else {
            $scope.currentImage = dialog.images[0];
        }
    }

    $scope.customImage = function (image) {
        $scope.currentImage = image;
    }
}]);