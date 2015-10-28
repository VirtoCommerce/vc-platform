angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.newProductWizardImagesController', ['$scope', '$filter', 'FileUploader', function ($scope, $filter, FileUploader) {
    var blade = $scope.blade;

    blade.currentEntity = angular.copy(blade.item);
    if (!blade.currentEntity.images) {
        blade.currentEntity.images = [];
    }
    blade.isLoading = false;

    $scope.addImageFromUrl = function () {
        if (blade.newExternalImageUrl) {
            blade.currentEntity.images.push({
                name: blade.newExternalImageUrl.substr(blade.newExternalImageUrl.lastIndexOf("/") + 1),
                url: blade.newExternalImageUrl,
                group: 'images'
            });
            blade.newExternalImageUrl = undefined;
        }
    };

    $scope.saveChanges = function () {
        blade.parentBlade.item.images = blade.currentEntity.images;
        $scope.bladeClose();
    };

    function initialize() {
        if (!$scope.uploader) {
            // create the uploader
            var uploader = $scope.uploader = new FileUploader({
                scope: $scope,
                headers: { Accept: 'application/json' },
                url: 'api/platform/assets?folderUrl=catalog/' + blade.currentEntity.code,
                autoUpload: true,
                removeAfterUpload: true
            });

            // ADDING FILTERS
            // Images only
            uploader.filters.push({
                name: 'imageFilter',
                fn: function (i /*{File|FileLikeObject}*/, options) {
                    var type = '|' + i.type.slice(i.type.lastIndexOf('/') + 1) + '|';
                    return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
                }
            });


            uploader.onSuccessItem = function (fileItem, images, status, headers) {
                angular.forEach(images, function (image) {
                    //ADD uploaded image to the item
                    blade.currentEntity.images.push(image);
                });
            };
        }
    };

    $scope.toggleImageSelect = function (e, image) {
        if (e.ctrlKey == 1) {
            image.$selected = !image.$selected;
        } else {
            angular.forEach(blade.currentEntity.images, function (i) {
                i.$selected = false;
            });
            image.$selected = true;
        }
    }

    $scope.removeAction = function (selectedImages) {
        if (selectedImages == undefined) {
            selectedImages = $filter('filter')(blade.currentEntity.images, { $selected: true });
        }

        angular.forEach(selectedImages, function (image) {
            var idx = blade.currentEntity.images.indexOf(image);
            if (idx >= 0) {
                blade.currentEntity.images.splice(idx, 1);
            }
        });
    };

    blade.toolbarCommands = [
		{
		    name: "Remove", icon: 'fa fa-trash-o', executeMethod: function () { $scope.removeAction(); },
		    canExecuteMethod: function () {
		        var selectedImages = $filter('filter')(blade.currentEntity.images, { $selected: true });
		        return selectedImages.length > 0;
		    }
		}
    ];

    $scope.sortableOptions = {
        update: function (e, ui) {
        },
        stop: function (e, ui) {
        }
    };

    initialize();
}]);
